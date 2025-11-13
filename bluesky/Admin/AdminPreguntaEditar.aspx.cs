using System;
using System.Linq;
using bluesky.App_Code;
using bluesky.Models;

namespace bluesky.Admin
{
    public partial class AdminPreguntaEditar : AdminPage
    {
        private int? PreguntaId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["preguntaId"], out id) ? id : (int?)null;
            }
        }

        private int? EvaluacionIdFromQuery
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
                int? evalId = EvaluacionIdFromQuery;
                if (!evalId.HasValue)
                {
                    lblMsg.Text = "Evaluación no especificada.";
                    btnGuardar.Enabled = false;
                    return;
                }

                hfEvalId.Value = evalId.Value.ToString();
                hfPreguntaId.Value = PreguntaId.HasValue ? PreguntaId.Value.ToString() : "";

                CargarCabecera(evalId.Value);

                litTitulo.Text = PreguntaId.HasValue ? "Editar pregunta" : "Nueva pregunta";

                if (PreguntaId.HasValue)
                    CargarPregunta(PreguntaId.Value);
            }
        }

        private void CargarCabecera(int evalId)
        {
            using (var db = new ApplicationDbContext())
            {
                var eva = db.Evaluaciones.Find(evalId);
                if (eva == null)
                {
                    lblMsg.Text = "Evaluación no encontrada.";
                    btnGuardar.Enabled = false;
                    return;
                }

                var curso = db.Cursos.Find(eva.CursoId);

                lblCurso.Text = curso != null ? curso.Titulo : "(curso sin título)";
                lblEvaluacion.Text = eva.Titulo;
            }
        }

        private void CargarPregunta(int preguntaId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = db.Preguntas.Find(preguntaId);
                if (p == null)
                {
                    lblMsg.Text = "Pregunta no encontrada.";
                    return;
                }

                txtEnunciado.Text = p.Enunciado;
                txtCategoria.Text = p.Categoria;
                ddlDificultad.SelectedValue = p.Dificultad.ToString();

                var alts = db.Alternativas
                    .Where(a => a.PreguntaId == p.Id && a.Activa)
                    .OrderBy(a => a.Orden)
                    .ToList();

                if (alts.Count > 0)
                {
                    if (alts.Count > 0) txtAlt1.Text = alts.ElementAtOrDefault(0)?.Texto;
                    if (alts.Count > 1) txtAlt2.Text = alts.ElementAtOrDefault(1)?.Texto;
                    if (alts.Count > 2) txtAlt3.Text = alts.ElementAtOrDefault(2)?.Texto;
                    if (alts.Count > 3) txtAlt4.Text = alts.ElementAtOrDefault(3)?.Texto;

                    rbCorrecta1.Checked = alts.ElementAtOrDefault(0)?.EsCorrecta ?? false;
                    rbCorrecta2.Checked = alts.ElementAtOrDefault(1)?.EsCorrecta ?? false;
                    rbCorrecta3.Checked = alts.ElementAtOrDefault(2)?.EsCorrecta ?? false;
                    rbCorrecta4.Checked = alts.ElementAtOrDefault(3)?.EsCorrecta ?? false;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int evalId;
            if (!int.TryParse(hfEvalId.Value, out evalId))
            {
                lblMsg.Text = "Evaluación inválida.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEnunciado.Text))
            {
                lblMsg.Text = "El enunciado es obligatorio.";
                return;
            }

            // Al menos una alternativa con texto
            var alt1 = txtAlt1.Text.Trim();
            var alt2 = txtAlt2.Text.Trim();
            var alt3 = txtAlt3.Text.Trim();
            var alt4 = txtAlt4.Text.Trim();

            if (string.IsNullOrWhiteSpace(alt1) &&
                string.IsNullOrWhiteSpace(alt2) &&
                string.IsNullOrWhiteSpace(alt3) &&
                string.IsNullOrWhiteSpace(alt4))
            {
                lblMsg.Text = "Debes ingresar al menos una alternativa.";
                return;
            }

            // Debe haber una correcta
            int indexCorrecta = -1;
            if (rbCorrecta1.Checked) indexCorrecta = 0;
            else if (rbCorrecta2.Checked) indexCorrecta = 1;
            else if (rbCorrecta3.Checked) indexCorrecta = 2;
            else if (rbCorrecta4.Checked) indexCorrecta = 3;

            if (indexCorrecta == -1)
            {
                lblMsg.Text = "Debes marcar cuál alternativa es la correcta.";
                return;
            }

            int dificultad = 2;
            int.TryParse(ddlDificultad.SelectedValue, out dificultad);

            using (var db = new ApplicationDbContext())
            {
                Pregunta pregunta;
                if (!string.IsNullOrEmpty(hfPreguntaId.Value))
                {
                    int pregId = int.Parse(hfPreguntaId.Value);
                    pregunta = db.Preguntas.FirstOrDefault(p => p.Id == pregId);
                    if (pregunta == null)
                    {
                        lblMsg.Text = "Pregunta no encontrada.";
                        return;
                    }
                }
                else
                {
                    // Nuevo -> calcular orden secuencial
                    var maxOrden = db.Preguntas
                        .Where(p => p.EvaluacionId == evalId && p.Activa)
                        .Select(p => (int?)p.Orden)
                        .Max() ?? 0;

                    pregunta = new Pregunta
                    {
                        EvaluacionId = evalId,
                        Orden = maxOrden + 1,
                        Activa = true,
                        MultipleRespuesta = false
                    };
                    db.Preguntas.Add(pregunta);
                }

                // Datos base
                pregunta.Enunciado = txtEnunciado.Text.Trim();
                pregunta.Categoria = string.IsNullOrWhiteSpace(txtCategoria.Text)
                    ? null
                    : txtCategoria.Text.Trim();
                pregunta.Dificultad = (DificultadPregunta)dificultad;
                pregunta.MultipleRespuesta = false; // por ahora, solo una correcta

                db.SaveChanges();

                // Alternativas
                var existentes = db.Alternativas
                    .Where(a => a.PreguntaId == pregunta.Id)
                    .OrderBy(a => a.Orden)
                    .ToList();

                string[] textos = { alt1, alt2, alt3, alt4 };

                for (int i = 0; i < 4; i++)
                {
                    var t = textos[i];
                    if (string.IsNullOrWhiteSpace(t)) t = "Opción";

                    Alternativa alt;
                    if (i < existentes.Count)
                    {
                        alt = existentes[i];
                    }
                    else
                    {
                        alt = new Alternativa
                        {
                            PreguntaId = pregunta.Id
                        };
                        db.Alternativas.Add(alt);
                    }

                    alt.Texto = t;
                    alt.EsCorrecta = (i == indexCorrecta);
                    alt.Orden = i + 1;
                    alt.Activa = true;
                }

                // Si habían más de 4 en BD, las marcamos inactivas
                if (existentes.Count > 4)
                {
                    foreach (var extra in existentes.Skip(4))
                        extra.Activa = false;
                }

                db.SaveChanges();
            }

            Response.Redirect("~/Admin/AdminEvaluacionPreguntas.aspx?evaluacionId=" + evalId);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            int evalId;
            if (!int.TryParse(hfEvalId.Value, out evalId))
                Response.Redirect("~/Admin/AdminEvaluaciones.aspx");
            else
                Response.Redirect("~/Admin/AdminEvaluacionPreguntas.aspx?evaluacionId=" + evalId);
        }
    }
}
