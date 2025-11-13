using System;
using System.Linq;
using System.Web;
using bluesky.App_Code;    // ProtectedPage
using bluesky.Models;

namespace bluesky.Usuario
{
    public partial class ResultadoEvaluacion : ProtectedPage
    {
        protected int? IntentoId
        {
            get { int id; return int.TryParse(Request.QueryString["intentoId"], out id) ? id : (int?)null; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Cargar();
        }

        private void Cargar()
        {
            if (!IntentoId.HasValue) { Error("Intento no especificado."); return; }

            using (var db = new ApplicationDbContext())
            {
                var intento = db.IntentosEvaluacion.FirstOrDefault(i => i.Id == IntentoId.Value);
                if (intento == null) { Error("Intento no encontrado."); return; }

                var eval = db.Evaluaciones.FirstOrDefault(e => e.Id == intento.EvaluacionId);
                if (eval == null) { Error("Evaluación no encontrada."); return; }

                litEval.Text = HttpUtility.HtmlEncode(eval.Titulo);
                litPuntaje.Text = intento.PuntajeObtenido.ToString("0.##");
                litEstado.Text = intento.Aprobado ? "APROBADO ✅" : "REPROBADO ❌";

                lnkVolverCurso.NavigateUrl = ResolveUrl($"~/Usuario/CursoDetalle.aspx?id={eval.CursoId}");

                pnlBody.Visible = true;
                lblMsg.Visible = false;
            }
        }

        private new void Error(string msg)
        {
            pnlBody.Visible = false;
            lblMsg.Text = msg;
            lblMsg.Visible = true;
        }
    }
}
