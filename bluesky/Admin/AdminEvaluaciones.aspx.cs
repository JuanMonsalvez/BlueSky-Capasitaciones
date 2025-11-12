using System;
using System.Linq;
using System.Web.UI.WebControls;
using bluesky.Models;
using bluesky.App_Code;

namespace bluesky.Admin
{
    public partial class AdminEvaluaciones : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarEvaluaciones();
        }

        private void CargarEvaluaciones()
        {
            using (var db = new ApplicationDbContext())
            {
                var evals = db.Evaluaciones
                    .Select(e => new
                    {
                        e.Id,
                        CursoTitulo = e.Curso.Titulo,
                        e.Titulo,
                        Tipo = e.Tipo.ToString(),
                        e.NumeroPreguntas,
                        e.TiempoMinutos,
                        e.PuntajeAprobacion
                    })
                    .OrderByDescending(e => e.Id)
                    .ToList();

                gvEvaluaciones.DataSource = evals;
                gvEvaluaciones.DataBind();
            }
        }

        protected void gvEvaluaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Admin/AdminEvaluacionEditar.aspx?id=" + id);
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                using (var db = new ApplicationDbContext())
                {
                    var eval = db.Evaluaciones.Find(id);
                    if (eval != null)
                    {
                        db.Evaluaciones.Remove(eval);
                        db.SaveChanges();
                    }
                }
                CargarEvaluaciones();
            }
        }

        protected void btnNuevaEval_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/AdminEvaluacionEditar.aspx");
        }
    }
}
