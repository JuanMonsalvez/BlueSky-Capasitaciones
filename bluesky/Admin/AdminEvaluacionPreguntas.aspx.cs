using bluesky.App_Code; // AdminPage
using bluesky.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace bluesky.Admin
{
    public partial class AdminEvaluacionPreguntas : AdminPage
    {
        private int EvalId
        {
            get
            {
                // evalId desde ruta o querystring
                var qs = Request.QueryString["evalId"];
                int id;
                if (!int.TryParse(qs, out id))
                {
                    // intenta FriendlyUrl: /Admin/Evaluaciones/{evalId}/Preguntas
                    var raw = Page.RouteData.Values["evalId"] as string;
                    if (!int.TryParse(raw, out id)) id = 0;
                }
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Cargar();
        }

        private void Cargar()
        {
            if (EvalId <= 0) { lblMsg.Text = "Evaluación inválida."; return; }

            using (var db = new ApplicationDbContext())
            {
                var eval = db.Evaluaciones.Where(e => e.Id == EvalId)
                    .Select(e => new { e.Id, e.Titulo, CursoTitulo = e.Curso.Titulo })
                    .FirstOrDefault();

                if (eval == null) { lblMsg.Text = "Evaluación no encontrada."; return; }

                litEvalTitulo.Text = eval.Titulo;

                // enlaces
                lnkVolver.HRef = ResolveUrl("~/Admin/AdminEvaluaciones.aspx");
                lnkNueva.HRef = GetNuevaPreguntaUrl(EvalId);

                // preguntas + alternativas
                var data = db.Preguntas
                    .Where(p => p.EvaluacionId == EvalId)
                    .OrderBy(p => p.Orden)
                    .Select(p => new
                    {
                        p.Id,
                        p.Enunciado,
                        p.Categoria,
                        p.Dificultad,
                        p.MultipleRespuesta,
                        p.Orden,
                        Alternativas = p.Alternativas
                            .OrderBy(a => a.Orden)
                            .Select(a => new { a.Texto, a.EsCorrecta })
                            .ToList()
                    })
                    .ToList();

                pnlVacio.Visible = data.Count == 0;
                repPreguntas.DataSource = data;
                repPreguntas.DataBind();
            }
        }

        protected void repPreguntas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id;
            if (!int.TryParse((string)e.CommandArgument, out id))
                return;

            if (e.CommandName == "edit")
            {
                Response.Redirect(GetEditarPreguntaUrl(EvalId, id), true);
            }
            else if (e.CommandName == "del")
            {
                using (var db = new ApplicationDbContext())
                {
                    var p = db.Preguntas.FirstOrDefault(x => x.Id == id && x.EvaluacionId == EvalId);
                    if (p != null)
                    {
                        // elimina alternativas primero (FK ON CASCADE podría hacerlo igual)
                        var alts = db.Alternativas.Where(a => a.PreguntaId == p.Id).ToList();
                        db.Alternativas.RemoveRange(alts);
                        db.Preguntas.Remove(p);
                        db.SaveChanges();
                    }
                }
                Cargar();
            }
        }

        private string GetNuevaPreguntaUrl(int evalId) =>
            ResolveUrl($"~/Admin/AdminPreguntaEditar.aspx?evalId={evalId}");

        private string GetEditarPreguntaUrl(int evalId, int id) =>
            ResolveUrl($"~/Admin/AdminPreguntaEditar.aspx?evalId={evalId}&id={id}");
    }
}
