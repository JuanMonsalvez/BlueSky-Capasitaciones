using System;
using System.Linq;
using System.Web.UI.WebControls;
using bluesky.App_Code;
using bluesky.Models;

namespace bluesky.Admin
{
    public partial class AdminEvaluaciones : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCursos();
                BindGrid();
            }
        }

        private void CargarCursos()
        {
            using (var db = new ApplicationDbContext())
            {
                var cursos = db.Cursos
                    .OrderBy(c => c.Titulo)
                    .Select(c => new { c.Id, c.Titulo })
                    .ToList();

                ddlCurso.DataSource = cursos;
                ddlCurso.DataTextField = "Titulo";
                ddlCurso.DataValueField = "Id";
                ddlCurso.DataBind();
                ddlCurso.Items.Insert(0, new ListItem("(Todos los cursos)", ""));
            }
        }

        private void BindGrid()
        {
            int cursoId;
            var filtroTitulo = (txtBuscar.Text ?? "").Trim().ToLower();

            using (var db = new ApplicationDbContext())
            {
                var q = db.Evaluaciones.Select(e => new
                {
                    e.Id,
                    e.Titulo,
                    e.Tipo,
                    e.NumeroPreguntas,
                    e.TiempoMinutos,
                    e.PuntajeAprobacion,
                    e.Activa,
                    CursoTitulo = e.Curso.Titulo,
                    e.CursoId
                });

                if (int.TryParse(ddlCurso.SelectedValue, out cursoId))
                    q = q.Where(x => x.CursoId == cursoId);

                if (!string.IsNullOrEmpty(filtroTitulo))
                    q = q.Where(x => x.Titulo.ToLower().Contains(filtroTitulo));

                gvEval.DataSource = q.OrderBy(x => x.CursoTitulo).ThenBy(x => x.Titulo).ToList();
                gvEval.DataBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e) => BindGrid();

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/AdminEvaluacionEditar.aspx");
        }

        protected void gvEval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editEval")
            {
                Response.Redirect("~/Admin/AdminEvaluacionEditar.aspx?id=" + e.CommandArgument);
            }
            else if (e.CommandName == "delEval")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                using (var db = new ApplicationDbContext())
                {
                    var eval = db.Evaluaciones.Find(id);
                    if (eval == null)
                    {
                        lblMsg.Text = "Evaluación no encontrada.";
                        return;
                    }

                    // Eliminar en orden seguro: Intentos -> Respuestas -> Preguntas -> Alternativas -> Evaluación
                    // (Si definiste cascade delete en FKs de MySQL, esto será más simple,
                    // pero lo hacemos explícito para evitar errores).
                    var preguntas = db.Preguntas.Where(p => p.EvaluacionId == id).ToList();
                    var pregIds = preguntas.Select(p => p.Id).ToList();

                    var alternativas = db.Alternativas.Where(a => pregIds.Contains(a.PreguntaId)).ToList();
                    db.Alternativas.RemoveRange(alternativas);

                    var intentos = db.IntentosEvaluacion.Where(i => i.EvaluacionId == id).ToList();
                    var intentoIds = intentos.Select(i => i.Id).ToList();

                    var respuestas = db.IntentosRespuesta.Where(r => intentoIds.Contains(r.IntentoEvaluacionId)).ToList();
                    db.IntentosRespuesta.RemoveRange(respuestas);
                    db.IntentosEvaluacion.RemoveRange(intentos);

                    db.Preguntas.RemoveRange(preguntas);
                    db.Evaluaciones.Remove(eval);

                    db.SaveChanges();
                }
                BindGrid();
            }
        }
    }
}
