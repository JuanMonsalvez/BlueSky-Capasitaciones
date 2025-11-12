using bluesky.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace bluesky.Admin
{
    public partial class CursoEditar : System.Web.UI.Page
    {
        private int? CursoId
        {
            get { int id; return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litTitulo.Text = CursoId.HasValue ? "Editar curso" : "Nuevo curso";
                CargarCombos();
                if (CursoId.HasValue) CargarCurso(CursoId.Value);
                CargarMateriales();
            }
        }

        private void CargarCombos()
        {
            using (var db = new ApplicationDbContext())
            {
                ddlArea.DataSource = db.Areas.OrderBy(a => a.Nombre).ToList();
                ddlArea.DataTextField = "Nombre";
                ddlArea.DataValueField = "Id";
                ddlArea.DataBind();
                ddlArea.Items.Insert(0, new ListItem("(sin área)", ""));

                var instructores = db.Usuarios
                    .OrderBy(u => u.NombreCompleto)
                    .Select(u => new { u.Id, u.NombreCompleto })
                    .ToList();

                ddlInstructor.DataSource = instructores;
                ddlInstructor.DataTextField = "NombreCompleto";
                ddlInstructor.DataValueField = "Id";
                ddlInstructor.DataBind();
                ddlInstructor.Items.Insert(0, new ListItem("(sin instructor)", ""));
            }
        }

        private void CargarCurso(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var c = db.Cursos.Find(id);
                if (c == null) { Response.Redirect("~/Admin/AdminCursos.aspx"); return; }

                txtTitulo.Text = c.Titulo;
                txtDescripcion.Text = c.Descripcion;
                if (c.AreaId.HasValue) ddlArea.SelectedValue = c.AreaId.Value.ToString();
                if (c.InstructorId.HasValue) ddlInstructor.SelectedValue = c.InstructorId.Value.ToString();
                txtDuracion.Text = c.DuracionHoras.ToString();
                ddlNivel.SelectedValue = ((int)c.Nivel).ToString();
                chkActivo.Checked = c.Activo;

                if (!string.IsNullOrEmpty(c.PortadaUrl))
                {
                    imgPreview.ImageUrl = ResolveUrl(c.PortadaUrl);
                    imgPreview.Visible = true;
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int? areaId = null, instructorId = null;
            int tmp;
            if (int.TryParse(ddlArea.SelectedValue, out tmp)) areaId = tmp;
            if (int.TryParse(ddlInstructor.SelectedValue, out tmp)) instructorId = tmp;

            int duracion = 0; int.TryParse(txtDuracion.Text, out duracion);

            using (var db = new ApplicationDbContext())
            {
                var ahora = DateTime.UtcNow;
                Curso curso = CursoId.HasValue ? db.Cursos.Find(CursoId.Value) : new Curso { FechaCreacion = ahora, Activo = true };
                if (curso == null) { lblMsg.Text = "Curso no encontrado."; return; }
                if (!CursoId.HasValue) db.Cursos.Add(curso);

                curso.Titulo = txtTitulo.Text.Trim();
                curso.Descripcion = txtDescripcion.Text.Trim();
                curso.AreaId = areaId;
                curso.InstructorId = instructorId;
                curso.DuracionHoras = duracion;
                curso.Nivel = (NivelCurso)int.Parse(ddlNivel.SelectedValue);
                curso.Activo = chkActivo.Checked;
                curso.FechaActualizacion = ahora;

                db.SaveChanges();

                if (fuPortada.HasFile)
                {
                    var ext = Path.GetExtension(fuPortada.FileName).ToLowerInvariant();
                    if (!(ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".webp"))
                    { lblMsg.Text = "Formato de imagen no permitido."; return; }

                    var carpeta = $"~/Uploads/cursos/{curso.Id}/";
                    var carpetaFs = Server.MapPath(carpeta);
                    if (!Directory.Exists(carpetaFs)) Directory.CreateDirectory(carpetaFs);
                    var destinoFs = Path.Combine(carpetaFs, "portada" + ext);
                    fuPortada.SaveAs(destinoFs);

                    curso.PortadaUrl = VirtualPathUtility.ToAbsolute(carpeta + "portada" + ext);
                    db.SaveChanges();

                    imgPreview.ImageUrl = curso.PortadaUrl;
                    imgPreview.Visible = true;
                }

                Response.Redirect("~/Admin/AdminCursos.aspx");
            }
        }

        private void CargarMateriales()
        {
            if (!CursoId.HasValue) { gvMateriales.DataSource = null; gvMateriales.DataBind(); return; }
            using (var db = new ApplicationDbContext())
            {
                var items = db.CursoMateriales
                    .Where(m => m.CursoId == CursoId.Value && m.Activo)
                    .Select(m => new
                    {
                        m.Id,
                        m.NombreArchivo,
                        Tipo = m.Tipo.ToString(),
                        RutaAlmacenamiento = m.RutaAlmacenamiento
                    })
                    .OrderBy(m => m.NombreArchivo)
                    .ToList();

                gvMateriales.DataSource = items;
                gvMateriales.DataBind();
            }
        }

        protected void btnSubirMaterial_Click(object sender, EventArgs e)
        {
            if (!CursoId.HasValue) { lblMatMsg.Text = "Guarda el curso antes de subir materiales."; return; }
            if (!fuMaterial.HasFile) { lblMatMsg.Text = "Selecciona un archivo."; return; }

            var ext = Path.GetExtension(fuMaterial.FileName).ToLowerInvariant();
            var ok = ext == ".pdf" || ext == ".ppt" || ext == ".pptx";
            if (!ok) { lblMatMsg.Text = "Sólo PDF/PPT/PPTX."; return; }

            var carpeta = $"~/Uploads/cursos/{CursoId.Value}/materiales/";
            var carpetaFs = Server.MapPath(carpeta);
            if (!Directory.Exists(carpetaFs)) Directory.CreateDirectory(carpetaFs);

            var fileName = Path.GetFileName(fuMaterial.FileName);
            fuMaterial.SaveAs(Path.Combine(carpetaFs, fileName));
            var ruta = VirtualPathUtility.ToAbsolute(carpeta + fileName);

            using (var db = new ApplicationDbContext())
            {
                db.CursoMateriales.Add(new CursoMaterial
                {
                    CursoId = CursoId.Value,
                    NombreArchivo = fileName,
                    RutaAlmacenamiento = ruta,
                    Tipo = (TipoMaterial)int.Parse(ddlTipoMaterial.SelectedValue),
                    Activo = true,
                    FechaSubida = DateTime.UtcNow
                });
                db.SaveChanges();
            }

            lblMatMsg.Text = "";
            CargarMateriales();
        }

        public void gvMateriales_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = (int)gvMateriales.DataKeys[e.RowIndex].Value;
            using (var db = new ApplicationDbContext())
            {
                var m = db.CursoMateriales.Find(id);
                if (m != null) { m.Activo = false; db.SaveChanges(); }
            }
            CargarMateriales();
        }
    }
}
