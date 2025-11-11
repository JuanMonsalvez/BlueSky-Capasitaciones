using bluesky.Models;
using bluesky.Usuario;
using System;
using System.Linq;
using System.Web.UI;
using bluesky.App_Code;

namespace bluesky.Admin
{
    // Solo Admin
    public partial class CursoEditar : AdminPage
    {
        private int? Id
        {
            get
            {
                int v;
                return int.TryParse(Request.QueryString["id"], out v) ? (int?)v : null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litTitulo.Text = Id.HasValue ? "Editar curso" : "Nuevo curso";
                if (Id.HasValue) CargarCurso(Id.Value);
            }
        }

        private void CargarCurso(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var curso = db.Cursos.FirstOrDefault(c => c.Id == id);
                if (curso == null)
                {
                    lblMsg.Text = "El curso no existe.";
                    return;
                }

                // ⚠ Ajusta nombres: si tu modelo usa Nombre en vez de Titulo, cámbialo
                txtTitulo.Text = curso.Titulo;
                txtDescripcion.Text = curso.Descripcion;
                chkActivo.Checked = curso.Activo;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (Id.HasValue)
                    {
                        var curso = db.Cursos.FirstOrDefault(c => c.Id == Id.Value);
                        if (curso == null)
                        {
                            lblMsg.Text = "El curso no existe.";
                            return;
                        }

                        curso.Titulo = txtTitulo.Text.Trim();          // ⚠ ajusta si tu propiedad es Nombre
                        curso.Descripcion = txtDescripcion.Text.Trim();
                        curso.Activo = chkActivo.Checked;
                        db.SaveChanges();
                    }
                    else
                    {
                        var nuevo = new Curso
                        {
                            Titulo = txtTitulo.Text.Trim(),            // ⚠ ajusta si tu propiedad es Nombre
                            Descripcion = txtDescripcion.Text.Trim(),
                            Activo = chkActivo.Checked,
                            FechaCreacion = DateTime.UtcNow
                        };
                        db.Cursos.Add(nuevo);
                        db.SaveChanges();
                    }
                }

                Response.Redirect("~/Admin/AdminCursos.aspx", endResponse: true);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "No se pudo guardar el curso.";
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
