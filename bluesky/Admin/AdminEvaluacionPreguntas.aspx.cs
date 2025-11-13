using System;
using System.Linq;
using System.Web.UI.WebControls;
using bluesky.App_Code;
using bluesky.Models;

namespace bluesky.Admin
{
    public partial class AdminEvaluacionPreguntas : AdminPage
    {
        private int? EvalId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["evaluacionId"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!EvalId.HasValue)
                {
                    lblMsg.Text = "Evaluación no especificada.";
                    btnNuevaPregunta.Enabled = false;
                    return;
                }

                CargarCabeceraYLista();
            }
        }

        private void CargarCabeceraYLista()
        {
            if (!EvalId.HasValue) return;

            using (var db = new ApplicationDbContext())
            {
                var eva = db.Evaluaciones.Find(EvalId.Value);
                if (eva == null)
                {
                    lblMsg.Text = "Evaluación no encontrada.";
                    btnNuevaPregunta.Enabled = false;
                    return;
                }

                var curso = db.Cursos.Find(eva.CursoId);

                hfEvalId.Value = eva.Id.ToString();
                lblCurso.Text = curso != null ? curso.Titulo : "(curso sin título)";
                lblEvaluacion.Text = eva.Titulo;

                var preguntas = db.Preguntas
                    .Where(p => p.EvaluacionId == eva.Id && p.Activa)
                    .OrderBy(p => p.Orden)
                    .ThenBy(p => p.Id)
                    .Select(p => new
                    {
                        p.Id,
                        p.Orden,
                        EnunciadoResumen = p.Enunciado.Length > 120
                            ? p.Enunciado.Substring(0, 120) + "..."
                            : p.Enunciado,
                        p.Categoria,
                        DificultadTexto =
                            p.Dificultad == DificultadPregunta.Facil ? "Básica" :
                            p.Dificultad == DificultadPregunta.Media ? "Media" : "Avanzada",
                        AlternativasCount = db.Alternativas
                            .Count(a => a.PreguntaId == p.Id && a.Activa)
                    })
                    .ToList();

                gvPreguntas.DataSource = preguntas;
                gvPreguntas.DataBind();
            }
        }

        protected void btnNuevaPregunta_Click(object sender, EventArgs e)
        {
            if (!EvalId.HasValue) return;
            Response.Redirect("~/Admin/AdminPreguntaEditar.aspx?evaluacionId=" + EvalId.Value);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/AdminEvaluaciones.aspx");
        }

        protected void gvPreguntas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditarPregunta")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("~/Admin/AdminPreguntaEditar.aspx?evaluacionId=" + EvalId + "&preguntaId=" + id);
            }
            else if (e.CommandName == "EliminarPregunta")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                using (var db = new ApplicationDbContext())
                {
                    var pregunta = db.Preguntas.FirstOrDefault(p => p.Id == id);
                    if (pregunta != null)
                    {
                        pregunta.Activa = false;

                        var alts = db.Alternativas
                            .Where(a => a.PreguntaId == pregunta.Id)
                            .ToList();

                        foreach (var a in alts)
                            a.Activa = false;

                        db.SaveChanges();
                    }
                }
                CargarCabeceraYLista();
            }
        }
    }
}
