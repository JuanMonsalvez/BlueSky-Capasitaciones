using System;
using System.Linq;
using System.Web.UI.WebControls;
using bluesky.App_Code;
using bluesky.Models;

namespace bluesky.Admin
{
    public partial class EvaluacionEditar : AdminPage
    {
        private int? EvalId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litTitulo.Text = EvalId.HasValue ? "Editar evaluación" : "Nueva evaluación";
                CargarCombos();
                if (EvalId.HasValue) CargarDatos(EvalId.Value);
            }
        }

        private void CargarCombos()
        {
            using (var db = new ApplicationDbContext())
            {
                ddlCurso.DataSource = db.Cursos
                    .OrderBy(c => c.Titulo)
                    .Select(c => new { c.Id, c.Titulo })
                    .ToList();
                ddlCurso.DataTextField = "Titulo";
                ddlCurso.DataValueField = "Id";
                ddlCurso.DataBind();
                ddlCurso.Items.Insert(0, new ListItem("(Selecciona)", ""));

                // Enum TipoEvaluacion
                ddlTipo.Items.Clear();
                foreach (var v in Enum.GetValues(typeof(TipoEvaluacion)))
                {
                    ddlTipo.Items.Add(new ListItem(v.ToString(), ((int)v).ToString()));
                }
            }
        }

        private void CargarDatos(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var e = db.Evaluaciones.Find(id);
                if (e == null)
                {
                    Response.Redirect("~/Admin/AdminEvaluaciones.aspx");
                    return;
                }

                ddlCurso.SelectedValue = e.CursoId.ToString();
                txtTitulo.Text = e.Titulo;
                ddlTipo.SelectedValue = ((int)e.Tipo).ToString();
                txtNumeroPreguntas.Text = e.NumeroPreguntas.ToString();
                txtTiempo.Text = e.TiempoMinutos.ToString();
                txtPuntaje.Text = e.PuntajeAprobacion.ToString("0.##");
                chkActiva.Checked = e.Activa;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlCurso.SelectedValue))
            {
                lblMsg.Text = "Selecciona un curso.";
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                lblMsg.Text = "El título es obligatorio.";
                return;
            }

            int cursoId = int.Parse(ddlCurso.SelectedValue);
            int numPreg = SafeInt(txtNumeroPreguntas.Text, 15);
            int tiempo = SafeInt(txtTiempo.Text, 30);
            decimal puntaje = SafeDecimal(txtPuntaje.Text, 60m);
            var tipo = (TipoEvaluacion)int.Parse(ddlTipo.SelectedValue);
            var ahora = DateTime.UtcNow;

            using (var db = new ApplicationDbContext())
            {
                Evaluacion evaluacion;
                if (EvalId.HasValue)
                {
                    evaluacion = db.Evaluaciones.Find(EvalId.Value);
                    if (evaluacion == null)
                    {
                        lblMsg.Text = "Evaluación no encontrada.";
                        return;
                    }
                }
                else
                {
                    evaluacion = new Evaluacion
                    {
                        FechaCreacion = ahora,
                        Version = 1,
                        Activa = true
                    };
                    db.Evaluaciones.Add(evaluacion);
                }

                evaluacion.CursoId = cursoId;
                evaluacion.Titulo = txtTitulo.Text.Trim();
                evaluacion.Tipo = tipo;
                evaluacion.NumeroPreguntas = numPreg;
                evaluacion.TiempoMinutos = tiempo;
                evaluacion.PuntajeAprobacion = puntaje;
                evaluacion.Activa = chkActiva.Checked;

                db.SaveChanges();
                Response.Redirect("~/Admin/AdminEvaluaciones.aspx");
            }
        }

        private int SafeInt(string s, int def)
        {
            int v;
            return int.TryParse(s, out v) ? v : def;
        }

        private decimal SafeDecimal(string s, decimal def)
        {
            decimal v;
            return decimal.TryParse(s, out v) ? v : def;
        }
    }
}
