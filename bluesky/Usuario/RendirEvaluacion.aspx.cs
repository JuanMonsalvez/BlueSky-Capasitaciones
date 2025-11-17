using System;
using System.Linq;
using System.Web.UI;
using System.Data.Entity;
using bluesky.Models;
using bluesky.Services.Security;

// Alias para evitar confusión con otras clases Evaluacion en el namespace bluesky.Usuario
using EvaluacionModel = bluesky.Models.Evaluacion;
using PoliticaIntentosEvaluacionModel = bluesky.Models.PoliticaIntentosEvaluacion;

namespace bluesky.Usuario
{
    public partial class RendirEvaluacion : Page
    {
        /// <summary>
        /// Id de la evaluación recibido por querystring (?evaluacionId=###)
        /// </summary>
        private int EvaluacionId
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["evaluacionId"], out id))
                    return id;

                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AuthHelper.EnsureAuthenticatedOrRedirect("~/Auth/IniciarSesion.aspx");

                if (EvaluacionId <= 0)
                {
                    lblError.Text = "Evaluación no especificada.";
                    btnComenzar.Enabled = false;
                    return;
                }

                CargarEvaluacionYIntentos();
            }
        }

        private void CargarEvaluacionYIntentos()
        {
            var userId = AuthHelper.GetCurrentUserId();
            if (userId == null)
            {
                lblError.Text = "Debes iniciar sesión.";
                btnComenzar.Enabled = false;
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                EvaluacionModel eva = db.Evaluaciones
                    .Include(e => e.Curso)
                    .FirstOrDefault(e => e.Id == EvaluacionId);

                if (eva == null)
                {
                    lblError.Text = "Evaluación no encontrada.";
                    btnComenzar.Enabled = false;
                    return;
                }

                // Datos básicos
                hfEvaluacionId.Value = eva.Id.ToString();
                litTituloCurso.Text = eva.Curso != null ? eva.Curso.Titulo : "(Curso)";
                litTituloEvaluacion.Text = eva.Titulo;
                litTiempo.Text = eva.TiempoMinutos.ToString();
                litPreguntas.Text = eva.NumeroPreguntas.ToString();

                // Link volver al curso
                lnkVolverCurso.HRef = "~/Usuario/CursoDetalle.aspx?cursoId=" + eva.CursoId;

                // Descripción de política
                string politicaDesc;
                switch (eva.PoliticaIntentos)
                {
                    case PoliticaIntentosEvaluacionModel.Bloqueado:
                        politicaDesc = "Bloqueada: no se permiten intentos.";
                        break;

                    case PoliticaIntentosEvaluacionModel.Ilimitado:
                        politicaDesc = "Ilimitado (sin límite de intentos).";
                        break;

                    case PoliticaIntentosEvaluacionModel.MaximoPorDia:
                    default:
                        int max = eva.MaxIntentosPorDia <= 0 ? 3 : eva.MaxIntentosPorDia;
                        int horas = eva.CooldownHoras <= 0 ? 24 : eva.CooldownHoras;
                        politicaDesc = $"{max} intento(s) en una ventana de {horas} horas.";
                        break;
                }
                litPolitica.Text = politicaDesc;

                // Calcular intentos en ventana
                int usados, restantes;
                DateTime? proximoUtc;
                CalcularIntentosVentana(db, eva, userId.Value, out usados, out restantes, out proximoUtc);

                // Manejo por política
                if (eva.PoliticaIntentos == PoliticaIntentosEvaluacionModel.Bloqueado)
                {
                    lblIntentosInfo.Text = "Esta evaluación está bloqueada para todos los usuarios.";
                    lblIntentoActualInfo.Text = "";
                    lblCooldownInfo.Text = "";
                    btnComenzar.Enabled = false;
                    return;
                }

                if (eva.PoliticaIntentos == PoliticaIntentosEvaluacionModel.Ilimitado)
                {
                    lblIntentosInfo.Text = "Intentos ilimitados para esta evaluación.";
                    lblIntentoActualInfo.Text = "";
                    lblCooldownInfo.Text = "";
                    btnComenzar.Enabled = true;
                    return;
                }

                int maxIntentos = eva.MaxIntentosPorDia <= 0 ? 3 : eva.MaxIntentosPorDia;

                lblIntentosInfo.Text =
                    $"Intentos usados en la ventana actual: {usados}/{maxIntentos}. " +
                    $"Te quedan {Math.Max(restantes, 0)} intento(s).";

                int numeroIntentoActual = Math.Min(usados + 1, maxIntentos);
                lblIntentoActualInfo.Text =
                    $"Si comienzas ahora, este será tu intento {numeroIntentoActual}/{maxIntentos}.";

                if (restantes <= 0 && proximoUtc.HasValue)
                {
                    var local = proximoUtc.Value.ToLocalTime();
                    lblCooldownInfo.Text =
                        "Ya utilizaste todos los intentos permitidos. " +
                        $"Podrás volver a intentarlo el {local:dd/MM/yyyy HH:mm}.";
                    btnComenzar.Enabled = false;
                }
                else
                {
                    lblCooldownInfo.Text = "";
                    btnComenzar.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Lógica que contabiliza intentos dentro de la ventana y calcula el próximo posible.
        /// </summary>
        private static void CalcularIntentosVentana(
            ApplicationDbContext db,
            EvaluacionModel eva,
            int userId,
            out int usados,
            out int restantes,
            out DateTime? proximoIntentoUtc)
        {
            usados = 0;
            restantes = int.MaxValue;
            proximoIntentoUtc = null;

            if (eva.PoliticaIntentos == PoliticaIntentosEvaluacionModel.Bloqueado)
            {
                restantes = 0;
                return;
            }

            if (eva.PoliticaIntentos == PoliticaIntentosEvaluacionModel.Ilimitado)
            {
                restantes = int.MaxValue;
                return;
            }

            // MaximoPorDia / ventana
            int max = eva.MaxIntentosPorDia <= 0 ? 3 : eva.MaxIntentosPorDia;
            int horas = eva.CooldownHoras <= 0 ? 24 : eva.CooldownHoras;

            DateTime ahora = DateTime.UtcNow;
            DateTime desde = ahora.AddHours(-horas);

            var intentosVentana = db.IntentosEvaluacion
                .Where(i => i.UsuarioId == userId
                         && i.EvaluacionId == eva.Id
                         && i.FechaInicio >= desde)
                .OrderBy(i => i.FechaInicio)
                .ToList();

            usados = intentosVentana.Count;
            restantes = Math.Max(0, max - usados);

            if (restantes <= 0 && intentosVentana.Any())
            {
                DateTime primero = intentosVentana.First().FechaInicio;
                proximoIntentoUtc = primero.AddHours(horas);
            }
        }

        protected void btnComenzar_Click(object sender, EventArgs e)
        {
            var userId = AuthHelper.GetCurrentUserId();
            if (userId == null)
            {
                AuthHelper.EnsureAuthenticatedOrRedirect("~/Auth/IniciarSesion.aspx");
                return;
            }

            int evalId;
            if (!int.TryParse(hfEvaluacionId.Value, out evalId) || evalId <= 0)
            {
                lblError.Text = "Evaluación inválida.";
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                EvaluacionModel eva = db.Evaluaciones.Find(evalId);
                if (eva == null)
                {
                    lblError.Text = "Evaluación no encontrada.";
                    return;
                }

                int usados, restantes;
                DateTime? proximoUtc;
                CalcularIntentosVentana(db, eva, userId.Value, out usados, out restantes, out proximoUtc);

                if (eva.PoliticaIntentos == PoliticaIntentosEvaluacionModel.Bloqueado)
                {
                    lblError.Text = "Esta evaluación está bloqueada.";
                    return;
                }

                if (eva.PoliticaIntentos == PoliticaIntentosEvaluacionModel.Ilimitado)
                {
                    CrearIntentoYRedirigir(db, eva, userId.Value, null);
                    return;
                }

                int maxIntentos = eva.MaxIntentosPorDia <= 0 ? 3 : eva.MaxIntentosPorDia;

                if (restantes <= 0)
                {
                    if (proximoUtc.HasValue)
                    {
                        var local = proximoUtc.Value.ToLocalTime();
                        lblError.Text =
                            "Ya utilizaste todos los intentos permitidos para esta evaluación. " +
                            $"Podrás volver a intentarlo el {local:dd/MM/yyyy HH:mm}.";
                    }
                    else
                    {
                        lblError.Text =
                            "Ya utilizaste todos los intentos permitidos para esta evaluación.";
                    }
                    return;
                }

                int numeroIntentoActual = Math.Min(usados + 1, maxIntentos);
                CrearIntentoYRedirigir(db, eva, userId.Value, numeroIntentoActual);
            }
        }

        private void CrearIntentoYRedirigir(
            ApplicationDbContext db,
            EvaluacionModel eva,
            int userId,
            int? numeroIntento)
        {
            var intento = new IntentoEvaluacion
            {
                UsuarioId = userId,
                EvaluacionId = eva.Id,
                FechaInicio = DateTime.UtcNow,
                PuntajeObtenido = 0m,
                Aprobado = false
            };

            db.IntentosEvaluacion.Add(intento);
            db.SaveChanges();

            int maxIntentos = eva.MaxIntentosPorDia <= 0 ? 3 : eva.MaxIntentosPorDia;
            int n = numeroIntento ?? 1;

            string url =
                $"~/Usuario/RendirEvaluacionPreguntas.aspx?intentoId={intento.Id}&nIntento={n}&maxIntentos={maxIntentos}";
            Response.Redirect(url, false);
        }
    }
}
