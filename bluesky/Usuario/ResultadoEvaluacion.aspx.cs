using System;
using System.Linq;
using System.Web.UI;
using bluesky.Models;
using bluesky.App_Code;

namespace bluesky.Usuario
{
    public partial class ResultadoEvaluacion : ProtectedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int intentoId;
            if (!int.TryParse(Request.QueryString["intentoId"], out intentoId))
            {
                MostrarMensaje("No se especificó el intento de evaluación.");
                return;
            }

            CargarResultado(intentoId);
        }

        private void MostrarMensaje(string msg)
        {
            pnlResultado.Visible = false;
            lblMensaje.Text = msg;
            lblMensaje.Visible = true;
        }

        private void CargarResultado(int intentoId)
        {
            using (var db = new ApplicationDbContext())
            {
                var intento = db.IntentosEvaluacion.Find(intentoId);
                if (intento == null)
                {
                    MostrarMensaje("Intento de evaluación no encontrado.");
                    return;
                }

                var evaluacion = db.Evaluaciones.Find(intento.EvaluacionId);
                if (evaluacion == null)
                {
                    MostrarMensaje("Evaluación no encontrada.");
                    return;
                }

                var curso = db.Cursos.Find(evaluacion.CursoId);
                if (curso == null)
                {
                    MostrarMensaje("Curso no encontrado.");
                    return;
                }

                // --- Número de intento calculado ---
                var numeroIntento = db.IntentosEvaluacion
                    .Where(i => i.UsuarioId == intento.UsuarioId &&
                                i.EvaluacionId == intento.EvaluacionId &&
                                i.Id <= intento.Id)
                    .Count();

                // --- Respuestas del intento ---
                var respuestas = db.IntentosRespuesta
                    .Where(r => r.IntentoEvaluacionId == intento.Id)
                    .ToList();

                var totalPreguntas = respuestas.Count;

                // ✅ Aquí el cambio: sólo usamos EsCorrecta
                var correctas = respuestas.Count(r => r.EsCorrecta);
                var incorrectas = totalPreguntas - correctas;

                decimal porcentaje = 0;
                if (totalPreguntas > 0)
                    porcentaje = (decimal)correctas * 100m / totalPreguntas;

                // --- Llenar labels ---
                pnlResultado.Visible = true;
                lblMensaje.Visible = false;

                lblCurso.Text = curso.Titulo;
                lblEvaluacion.Text = evaluacion.Titulo;
                lblIntento.Text = $"{numeroIntento}";

                lblTotalPreguntas.Text = totalPreguntas.ToString();
                lblCorrectas.Text = correctas.ToString();
                lblIncorrectas.Text = incorrectas.ToString();
                lblPorcentaje.Text = porcentaje.ToString("0.0") + " %";

                lblResultado.Text = intento.Aprobado ? "APROBADO" : "REPROBADO";

                var fechaFin = intento.FechaFin ?? DateTime.UtcNow;
                lblFechaTermino.Text = fechaFin.ToString("dd/MM/yyyy HH:mm");

                // Link "Volver al curso"
                lnkVolverCurso.HRef = "~/Usuario/CursoDetalle.aspx?Id=" + curso.Id;
            }
        }
    }
}
