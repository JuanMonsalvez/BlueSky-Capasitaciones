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
            get { int id; return int.TryParse(Request.QueryString["evaluacionId"], out id) ? id : (int?)null; }
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

            using (var db = new ApplicationDbContext())
            {
                var eval = db.Evaluaciones.FirstOrDefault(x => x.Id == EvaluacionId.Value && x.Activa);
                if (eval == null)
                {
                    MostrarError("La evaluación no existe o no está activa.");
                    return;
                }

                // UI
                litTitulo.Text = HttpUtility.HtmlEncode(eval.Titulo);
                litCurso.Text = eval.Curso != null ? HttpUtility.HtmlEncode(eval.Curso.Titulo) : $"Curso #{eval.CursoId}";
                litTipo.Text = eval.Tipo.ToString();
                litPreguntas.Text = eval.NumeroPreguntas.ToString();
                litTiempo.Text = eval.TiempoMinutos.ToString();
                litAprob.Text = eval.PuntajeAprobacion.ToString("0");

                hdnEvaluacionId.Value = eval.Id.ToString();

                // Si ya hay intento abierto (sin FechaFin), reusar:
                var uid = AuthHelper.GetCurrentUserId();
                var intentoAbierto = db.IntentosEvaluacion
                    .Where(i => i.UsuarioId == uid && i.EvaluacionId == eval.Id && i.FechaFin == null)
                    .OrderByDescending(i => i.Id)
                    .FirstOrDefault();

                if (intentoAbierto != null)
                {
                    hdnIntentoId.Value = intentoAbierto.Id.ToString();
                    lblInfo.Text = "Tienes un intento en curso. Te redirigiremos a las preguntas.";
                    lblInfo.Visible = true;
                    Response.Redirect($"~/Usuario/RendirEvaluacionPreguntas.aspx?intentoId={intentoAbierto.Id}", true);
                    return;
                }

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
                // Seguridad: evitar duplicar intentos abiertos
                var existente = db.IntentosEvaluacion
                    .FirstOrDefault(i => i.UsuarioId == uid && i.EvaluacionId == evalId && i.FechaFin == null);

                if (existente != null)
                {
                    Response.Redirect($"~/Usuario/RendirEvaluacionPreguntas.aspx?intentoId={existente.Id}", true);
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

                Response.Redirect($"~/Usuario/RendirEvaluacionPreguntas.aspx?intentoId={intento.Id}", true);
            }
        }

        private void MostrarError(string msg)
        {
            pnlBody.Visible = false;
            lblMsg.Text = msg;
            lblMsg.Visible = true;
        }
    }
}
