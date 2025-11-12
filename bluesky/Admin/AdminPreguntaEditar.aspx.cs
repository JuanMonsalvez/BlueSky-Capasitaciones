using bluesky.App_Code; // AdminPage
using bluesky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bluesky.Admin
{
    public partial class AdminPreguntaEditar : AdminPage
    {
        private int EvalId
        {
            get
            {
                var qs = Request.QueryString["evalId"];
                int id;
                if (!int.TryParse(qs, out id))
                {
                    var raw = Page.RouteData.Values["evalId"] as string;
                    if (!int.TryParse(raw, out id)) id = 0;
                }
                return id;
            }
        }

        private int? PreguntaId
        {
            get
            {
                var qs = Request.QueryString["id"];
                int id;
                if (!int.TryParse(qs, out id))
                {
                    var raw = Page.RouteData.Values["id"] as string;
                    if (!int.TryParse(raw, out id)) return null;
                }
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCabeceraYEnlaces();
                CargarFormulario();
            }
        }

        private void CargarCabeceraYEnlaces()
        {
            if (EvalId <= 0) { lblMsg.Text = "Evaluación inválida."; return; }

            using (var db = new ApplicationDbContext())
            {
                var eval = db.Evaluaciones.Where(e => e.Id == EvalId)
                    .Select(e => new { e.Id, e.Titulo })
                    .FirstOrDefault();

                if (eval == null) { lblMsg.Text = "Evaluación no encontrada."; return; }

                litTitulo.Text = (PreguntaId.HasValue ? "Editar pregunta" : "Nueva pregunta") +
                                 " · " + eval.Titulo;

                lnkVolver.NavigateUrl = ResolveUrl($"~/Admin/AdminEvaluacionPreguntas.aspx?evalId={EvalId}");
            }
        }

        private void CargarFormulario()
        {
            if (!PreguntaId.HasValue)
            {
                // carga 4 alternativas por defecto
                var seed = Enumerable.Range(1, 4).Select(i => new AltVM
                {
                    Orden = i,
                    Texto = "",
                    EsCorrecta = (i == 1)
                }).ToList();

                repAlternativas.DataSource = seed;
                repAlternativas.DataBind();
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var p = db.Preguntas.FirstOrDefault(x => x.Id == PreguntaId.Value && x.EvaluacionId == EvalId);
                if (p == null) { lblMsg.Text = "Pregunta no encontrada."; return; }

                txtEnunciado.Text = p.Enunciado;
                txtCategoria.Text = p.Categoria;
                txtDificultad.Text = p.Dificultad.ToString();
                txtOrden.Text = p.Orden.ToString();
                chkMultiple.Checked = p.MultipleRespuesta;
                chkActiva.Checked = p.Activa;

                var alts = p.Alternativas
                    .OrderBy(a => a.Orden)
                    .Select(a => new AltVM { Orden = a.Orden, Texto = a.Texto, EsCorrecta = a.EsCorrecta })
                    .ToList();

                if (alts.Count < 2)
                {
                    while (alts.Count < 2) alts.Add(new AltVM { Orden = alts.Count + 1 });
                }
                repAlternativas.DataSource = alts;
                repAlternativas.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (EvalId <= 0) { lblMsg.Text = "Evaluación inválida."; return; }

            int dificultad, orden;
            if (!int.TryParse(txtDificultad.Text, out dificultad)) dificultad = 1;
            if (!int.TryParse(txtOrden.Text, out orden)) orden = 1;

            var alts = LeerAlternativasDesdeUI();

            // Validaciones básicas
            if (alts.Count < 2) { lblMsg.Text = "Debes registrar al menos 2 alternativas."; return; }
            if (!alts.Any(a => a.EsCorrecta)) { lblMsg.Text = "Debe haber al menos una alternativa correcta."; return; }
            if (alts.Count > 6) { lblMsg.Text = "Máximo 6 alternativas."; return; }

            using (var db = new ApplicationDbContext())
            {
                // asegura que la eval exista
                var eval = db.Evaluaciones.FirstOrDefault(x => x.Id == EvalId);
                if (eval == null) { lblMsg.Text = "Evaluación no encontrada."; return; }

                Pregunta p;
                if (PreguntaId.HasValue)
                {
                    p = db.Preguntas.FirstOrDefault(x => x.Id == PreguntaId.Value && x.EvaluacionId == EvalId);
                    if (p == null) { lblMsg.Text = "Pregunta no encontrada."; return; }
                }
                else
                {
                    p = new Pregunta
                    {
                        EvaluacionId = EvalId,
                        Activa = true
                    };
                    db.Preguntas.Add(p);
                }

                p.Enunciado = txtEnunciado.Text.Trim();
                p.Categoria = string.IsNullOrWhiteSpace(txtCategoria.Text) ? null : txtCategoria.Text.Trim();
                p.Dificultad = (DificultadPregunta)dificultad;
                p.MultipleRespuesta = chkMultiple.Checked;
                p.Orden = orden;
                p.Activa = chkActiva.Checked;

                // Guardamos para tener Id si es nueva
                db.SaveChanges();

                // Alternativas: estrategia simple -> borrar y recrear
                var actuales = db.Alternativas.Where(a => a.PreguntaId == p.Id).ToList();
                db.Alternativas.RemoveRange(actuales);
                db.SaveChanges();

                int idx = 1;
                foreach (var a in alts.OrderBy(x => x.Orden))
                {
                    if (string.IsNullOrWhiteSpace(a.Texto)) continue;

                    db.Alternativas.Add(new Alternativa
                    {
                        PreguntaId = p.Id,
                        Texto = a.Texto.Trim(),
                        EsCorrecta = a.EsCorrecta,
                        Orden = idx++,
                        Activa = true
                    });
                }
                db.SaveChanges();

                Response.Redirect($"~/Admin/AdminEvaluacionPreguntas.aspx?evalId={EvalId}");
            }
        }

        private List<AltVM> LeerAlternativasDesdeUI()
        {
            var list = new List<AltVM>();
            foreach (RepeaterItem it in repAlternativas.Items)
            {
                var txtOrd = (TextBox)it.FindControl("txtAltOrden");
                var txtTxt = (TextBox)it.FindControl("txtAltTexto");
                var chkOk = (CheckBox)it.FindControl("chkAltCorrecta");

                int orden;
                if (!int.TryParse(txtOrd.Text, out orden)) orden = 999;

                list.Add(new AltVM
                {
                    Orden = orden,
                    Texto = txtTxt.Text,
                    EsCorrecta = chkOk.Checked
                });
            }
            // filtra completamente vacías
            return list.Where(a => !string.IsNullOrWhiteSpace(a.Texto)).ToList();
        }

        private class AltVM
        {
            public int Orden { get; set; }
            public string Texto { get; set; }
            public bool EsCorrecta { get; set; }
        }
    }
}
