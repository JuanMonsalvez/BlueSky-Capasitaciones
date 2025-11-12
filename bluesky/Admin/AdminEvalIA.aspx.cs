using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using bluesky.Models;

namespace bluesky.Admin
{
    public partial class AdminEvalIA : App_Code.AdminPage
    {
        private int EvalId
        {
            get
            {
                int id;
                if (!int.TryParse(Request.QueryString["evalId"], out id)) return 0;
                return id;
            }
        }

        protected Evaluacion EvaluacionActual;
        protected Curso CursoActual;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEval();
            }
        }

        private void CargarEval()
        {
            using (var db = new ApplicationDbContext())
            {
                EvaluacionActual = db.Evaluaciones.Find(EvalId);
                if (EvaluacionActual == null) { Response.Redirect("~/Admin/AdminEvaluaciones.aspx"); return; }
                CursoActual = db.Cursos.Find(EvaluacionActual.CursoId);
                lblHdr.Text = $"Curso: <strong>{CursoActual.Titulo}</strong> — Evaluación #{EvaluacionActual.Id}";
            }
        }

        private string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            var text = Regex.Replace(html, "<.*?>", " ");
            text = System.Web.HttpUtility.HtmlDecode(text);
            return Regex.Replace(text, @"\s+", " ").Trim();
        }

        private string BuildCourseCorpus()
        {
            using (var db = new ApplicationDbContext())
            {
                var lecciones = db.CursoLecciones
                    .Where(x => x.Modulo.CursoId == CursoActual.Id)
                    .OrderBy(x => x.ModuloId).ThenBy(x => x.Orden)
                    .Select(x => new { x.Titulo, x.ContenidoHtml })
                    .ToList();

                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"Curso: {CursoActual.Titulo}");
                sb.AppendLine($"Descripción: {CursoActual.Descripcion}");
                sb.AppendLine();

                foreach (var l in lecciones)
                {
                    sb.AppendLine($"Lección: {l.Titulo}");
                    sb.AppendLine(StripHtml(l.ContenidoHtml));
                    sb.AppendLine();
                }

                // Aquí, si quieres, agrega texto de PDFs/PPTX leyendo CursoMateriales.
                // Para mantenerlo sin dependencias, lo dejamos comentado.

                return sb.ToString();
            }
        }

        // Generador "stub" local para que funcione hoy mismo sin Gemini:
        private System.Collections.Generic.List<dynamic> GeneratePreviewLocal(string corpus, int n = 3)
        {
            var list = new System.Collections.Generic.List<dynamic>();
            for (int i = 1; i <= n; i++)
            {
                list.Add(new
                {
                    Pregunta = $"(DEMO) Según el contenido del curso, ¿enunciado #{i}?",
                    A = "Opción A",
                    B = "Opción B",
                    C = "Opción C",
                    D = "Opción D",
                    Correcta = "A"
                });
            }
            return list;
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CargarEval();

            var corpus = BuildCourseCorpus();
            if (string.IsNullOrWhiteSpace(corpus))
            {
                lblMsg.Text = "No hay contenido en las lecciones del curso.";
                return;
            }

            // HOY: demo local. MAÑANA: llamada a Gemini con tu cliente existente.
            var preview = GeneratePreviewLocal(corpus, 3);
            gvPreview.DataSource = preview;
            gvPreview.DataBind();
            pPreview.Visible = true;
            lblMsg.Text = "";
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            CargarEval();
            var corpus = BuildCourseCorpus();
            if (string.IsNullOrWhiteSpace(corpus))
            {
                lblMsg.Text = "No hay contenido en las lecciones del curso.";
                return;
            }

            int total = int.Parse(ddlNum.SelectedValue);
            int tiempo = int.Parse(ddlTiempo.SelectedValue);
            decimal aprob = decimal.Parse(ddlAprob.SelectedValue);

            // DEMO: genera preguntas dummy y guarda. Sustituye por tu GeminiClient cuando quieras.
            var gen = GeneratePreviewLocal(corpus, total);

            using (var db = new ApplicationDbContext())
            {
                var eva = db.Evaluaciones.Find(EvalId);
                if (eva == null) { lblMsg.Text = "Evaluación no encontrada."; return; }

                eva.Tipo = (TipoEvaluacion)int.Parse(ddlTipo.SelectedValue);
                eva.NumeroPreguntas = total;
                eva.TiempoMinutos = tiempo;
                eva.PuntajeAprobacion = aprob;
                eva.Activa = true;
                eva.FechaCreacion = DateTime.UtcNow;

                db.SaveChanges();

                // Limpia preguntas previas si quieres regenerar
                var prevPregs = db.Preguntas.Where(p => p.EvaluacionId == eva.Id).ToList();
                var prevIds = prevPregs.Select(p => p.Id).ToList();
                var prevAlts = db.Alternativas.Where(a => prevIds.Contains(a.PreguntaId)).ToList();

                if (prevAlts.Any()) db.Alternativas.RemoveRange(prevAlts);
                if (prevPregs.Any()) db.Preguntas.RemoveRange(prevPregs);
                db.SaveChanges();

                int orden = 1;
                foreach (dynamic q in gen)
                {
                    var p = new Pregunta
                    {
                        EvaluacionId = eva.Id,
                        Enunciado = q.Pregunta,
                        Categoria = null,
                        Dificultad = (DificultadPregunta)(ddlDif.SelectedValue == "Avanzada" ? 3 : (ddlDif.SelectedValue == "Intermedia" ? 2 : 1)),
                        MultipleRespuesta = false,
                        Orden = orden++,
                        Activa = true
                    };
                    db.Preguntas.Add(p);
                    db.SaveChanges();

                    // Alternativas
                    var alts = new[]
                    {
                        new { Texto = (string)q.A, Key="A" },
                        new { Texto = (string)q.B, Key="B" },
                        new { Texto = (string)q.C, Key="C" },
                        new { Texto = (string)q.D, Key="D" }
                    };
                    int ord = 1;
                    foreach (var a in alts)
                    {
                        db.Alternativas.Add(new Alternativa
                        {
                            PreguntaId = p.Id,
                            Texto = a.Texto,
                            EsCorrecta = (string)q.Correcta == a.Key,
                            Orden = ord++,
                            Activa = true
                        });
                    }
                    db.SaveChanges();
                }
            }

            lblMsg.CssClass = "text-success";
            lblMsg.Text = "Evaluación generada y guardada. Puedes editar las preguntas en ‘Preguntas (Manual)’.";
        }
    }
}
