using System;
using System.Linq;
using System.Web.UI.WebControls;
using bluesky.Models;
using bluesky.App_Code;

namespace bluesky.Usuario
{
    public partial class CursoDetalle : ProtectedPage
    {
        protected int CursoId { get { int id; int.TryParse(Request.QueryString["id"], out id); return id; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CargarDatos();
        }

        private void CargarDatos()
        {
            using (var db = new ApplicationDbContext())
            {
                var curso = db.Cursos.FirstOrDefault(c => c.Id == CursoId && c.Activo);
                if (curso == null) { lblMsg.Text = "Curso no encontrado."; return; }

                litTitulo.Text = curso.Titulo;
                litDescripcion.Text = curso.Descripcion;

                var mats = db.CursoMateriales
                    .Where(m => m.CursoId == CursoId && m.Activo)
                    .Select(m => new { m.NombreArchivo, Tipo = m.Tipo.ToString(), m.RutaAlmacenamiento })
                    .OrderBy(m => m.NombreArchivo).ToList();
                repMateriales.DataSource = mats;
                repMateriales.DataBind();

                var evals = db.Evaluaciones
                    .Where(e => e.CursoId == CursoId && e.Activa)
                    .Select(e => new { e.Id, e.Titulo, e.NumeroPreguntas, e.TiempoMinutos })
                    .OrderBy(e => e.Id).ToList();
                repEvaluaciones.DataSource = evals;
                repEvaluaciones.DataBind();
            }
        }

        protected void repEvaluaciones_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Rendir")
            {
                int evalId = Convert.ToInt32(e.CommandArgument);
                using (var db = new ApplicationDbContext())
                {
                    int countPreg = db.Preguntas.Count(p => p.EvaluacionId == evalId);
                    if (countPreg == 0)
                    {
                        lblMsg.Text = "⚠️ Esta evaluación aún no tiene preguntas disponibles.";
                        return;
                    }
                }

                Response.Redirect("~/Usuario/RendirEvaluacion.aspx?evaluacionId=" + evalId);
            }
        }

    }
}
