using System;
using System.Linq;
using System.Web;
using bluesky.App_Code;                // ProtectedPage
using bluesky.Models;
using bluesky.Services.Security;       // AuthHelper

namespace bluesky.Usuario
{
    public partial class RendirEvaluacion : ProtectedPage
    {
        protected int? EvaluacionId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["evaluacionId"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Cargar();
        }

        private void Cargar()
        {
            if (!EvaluacionId.HasValue)
            {
                MostrarError("Evaluación no especificada.");
                return;
            }

            var uid = AuthHelper.GetCurrentUserId();
            if (uid == null)
            {
                MostrarError("Tu sesión expiró. Inicia sesión nuevamente.");
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var eval = db.Evaluaciones.FirstOrDefault(x => x.Id == EvaluacionId.Value && x.Activa);
                if (eval == null)
                {
                    MostrarError("La evaluación no existe o no está activa.");
                    return;
                }

                // ¿Intento abierto?
                var intentoAbierto = db.IntentosEvaluacion
                    .Where(i => i.UsuarioId == uid && i.EvaluacionId == eval.Id && i.FechaFin == null)
                    .OrderByDescending(i => i.Id)
                    .FirstOrDefault();

                if (intentoAbierto != null)
                {
                    hdnIntentoId.Value = intentoAbierto.Id.ToString();
                    lblInfo.Text = "Tienes un intento en curso. Te redirigiremos a las preguntas.";
                    lblInfo.Visible = true;
                    Response.Redirect("~/Usuario/RendirEvaluacionPreguntas.aspx?intentoId=" + intentoAbierto.Id, true);
                    return;
                }

                // Validar política de intentos
                string mensajeBloqueo;
                if (!PuedeIniciarIntento(db, eval, uid.Value, out mensajeBloqueo))
                {
                    MostrarError(mensajeBloqueo);
                    return;
                }

                // UI
                litTitulo.Text = HttpUtility.HtmlEncode(eval.Titulo);
                litCurso.Text = eval.Curso != null
                    ? HttpUtility.HtmlEncode(eval.Curso.Titulo)
                    : "Curso #" + eval.CursoId;

                litTipo.Text = eval.Tipo.ToString();
                litPreguntas.Text = eval.NumeroPreguntas.ToString();
                litTiempo.Text = eval.TiempoMinutos.ToString();
                litAprob.Text = eval.PuntajeAprobacion.ToString("0");

                hdnEvaluacionId.Value = eval.Id.ToString();

                pnlBody.Visible = true;
                lblMsg.Visible = false;
            }
        }

        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            int evalId;
            if (!int.TryParse(hdnEvaluacionId.Value, out evalId))
            {
                MostrarError("Evaluación no válida.");
                return;
            }

            var uid = AuthHelper.GetCurrentUserId();
            if (uid == null)
            {
                MostrarError("Tu sesión expiró. Inicia sesión nuevamente.");
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var eval = db.Evaluaciones.FirstOrDefault(x => x.Id == evalId && x.Activa);
                if (eval == null)
                {
                    MostrarError("La evaluación no existe o no está activa.");
                    return;
                }

                var existente = db.IntentosEvaluacion
                    .FirstOrDefault(i => i.UsuarioId == uid && i.EvaluacionId == evalId && i.FechaFin == null);

                if (existente != null)
                {
                    Response.Redirect("~/Usuario/RendirEvaluacionPreguntas.aspx?intentoId=" + existente.Id, true);
                    return;
                }

                // Validar antes de crear
                string mensajeBloqueo;
                if (!PuedeIniciarIntento(db, eval, uid.Value, out mensajeBloqueo))
                {
                    MostrarError(mensajeBloqueo);
                    return;
                }

                var intento = new IntentoEvaluacion
                {
                    UsuarioId = uid.Value,
                    EvaluacionId = evalId,
                    FechaInicio = DateTime.UtcNow,
                    PuntajeObtenido = 0m,
                    Aprobado = false
                };

                db.IntentosEvaluacion.Add(intento);
                db.SaveChanges();

                Response.Redirect("~/Usuario/RendirEvaluacionPreguntas.aspx?intentoId=" + intento.Id, true);
            }
        }

        /// <summary>
        /// Aplica la política de intentos configurada en Evaluación.
        /// </summary>
        private bool PuedeIniciarIntento(
            ApplicationDbContext db,
            bluesky.Models.Evaluacion eval,   // <- aquí forzamos el modelo correcto
            int userId,
            out string mensajeBloqueo)
        {
            mensajeBloqueo = null;

            // Bloqueado por admin
            if (eval.PoliticaIntentos == PoliticaIntentosEvaluacion.Bloqueado)
            {
                mensajeBloqueo = "Esta evaluación está bloqueada por el administrador.";
                return false;
            }

            var ahora = DateTime.UtcNow;

            var intentos = db.IntentosEvaluacion
                .Where(i => i.UsuarioId == userId && i.EvaluacionId == eval.Id)
                .OrderByDescending(i => i.FechaInicio)
                .ToList();

            switch (eval.PoliticaIntentos)
            {
                case PoliticaIntentosEvaluacion.MaximoPorDia:
                    {
                        var hoy = ahora.Date;
                        var mañana = hoy.AddDays(1);

                        var intentosHoy = intentos.Count(i =>
                            i.FechaInicio >= hoy && i.FechaInicio < mañana);

                        if (intentosHoy >= eval.MaxIntentosPorDia)
                        {
                            mensajeBloqueo = "Ya utilizaste todos los intentos permitidos para esta evaluación en día de hoy.";
                            return false;
                        }
                        break;
                    }

                case PoliticaIntentosEvaluacion.IlimitadoConCooldown:
                    {
                        var ultimo = intentos.FirstOrDefault();
                        if (ultimo != null)
                        {
                            var baseTime = ultimo.FechaFin ?? ultimo.FechaInicio;
                            var proximo = baseTime.AddHours(eval.CooldownHoras);

                            if (proximo > ahora)
                            {
                                var restante = proximo - ahora;
                                var horas = (int)Math.Ceiling(restante.TotalHours);
                                if (horas <= 0) horas = 1;

                                mensajeBloqueo =
                                    "Debes esperar aproximadamente " + horas +
                                    " hora(s) antes de volver a intentar esta evaluación.";
                                return false;
                            }
                        }
                        break;
                    }

                case PoliticaIntentosEvaluacion.Ilimitado:
                default:
                    // sin restricciones
                    break;
            }

            return true;
        }

        private void MostrarError(string msg)
        {
            pnlBody.Visible = false;
            lblMsg.Text = msg;
            lblMsg.Visible = true;
        }
    }
}
