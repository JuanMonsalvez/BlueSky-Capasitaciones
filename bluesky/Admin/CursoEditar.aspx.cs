using bluesky.App_Code; // AdminPage
using bluesky.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace bluesky.Admin
{
    public partial class CursoEditar : AdminPage
    {
        private int? CursoId
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
                litTitulo.Text = CursoId.HasValue ? "Editar curso" : "Nuevo curso";
                CargarCombos();
                if (CursoId.HasValue) CargarCurso(CursoId.Value);
            }
        }

        private void CargarCombos()
        {
            using (var db = new ApplicationDbContext())
            {
                // Áreas
                ddlArea.DataSource = db.Areas.OrderBy(a => a.Nombre).ToList();
                ddlArea.DataTextField = "Nombre";
                ddlArea.DataValueField = "Id";
                ddlArea.DataBind();
                ddlArea.Items.Insert(0, new System.Web.UI.WebControls.ListItem("(sin área)", ""));

                // Instructores (si no hay rol Instructor, listamos todos)
                var instructores = db.Usuarios
                    .OrderBy(u => u.NombreCompleto)
                    .Select(u => new { u.Id, u.NombreCompleto })
                    .ToList();

                ddlInstructor.DataSource = instructores;
                ddlInstructor.DataTextField = "NombreCompleto";
                ddlInstructor.DataValueField = "Id";
                ddlInstructor.DataBind();
                ddlInstructor.Items.Insert(0, new System.Web.UI.WebControls.ListItem("(sin instructor)", ""));

                // Niveles (asegúrate que en el .aspx los <asp:ListItem> tengan Value = 0/1/2)
                // Si los creas por código, podrías hacer:
                // ddlNivel.Items.Clear();
                // ddlNivel.Items.Add(new ListItem("Básico", "0"));
                // ddlNivel.Items.Add(new ListItem("Intermedio", "1"));
                // ddlNivel.Items.Add(new ListItem("Avanzado", "2"));
            }
        }

        private void CargarCurso(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var c = db.Cursos.Find(id);
                if (c == null)
                {
                    Response.Redirect("~/Admin/AdminCursos.aspx");
                    return;
                }

                txtTitulo.Text = c.Titulo;
                txtDescripcion.Text = c.Descripcion;
                if (c.AreaId.HasValue) ddlArea.SelectedValue = c.AreaId.Value.ToString();
                if (c.InstructorId.HasValue) ddlInstructor.SelectedValue = c.InstructorId.Value.ToString();
                txtDuracion.Text = c.DuracionHoras.ToString();

                // ✅ Usa el entero del enum, porque las opciones del ddl son "0/1/2"
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

            int duracion = 0;
            int.TryParse(txtDuracion.Text, out duracion);

            using (var db = new ApplicationDbContext())
            {
                Curso curso;
                var ahora = DateTime.UtcNow;

                if (CursoId.HasValue)
                {
                    curso = db.Cursos.Find(CursoId.Value);
                    if (curso == null)
                    {
                        lblMsg.Text = "Curso no encontrado.";
                        return;
                    }
                }
                else
                {
                    curso = new Curso
                    {
                        FechaCreacion = ahora,
                        Activo = true
                    };
                    db.Cursos.Add(curso);
                }

                curso.Titulo = txtTitulo.Text.Trim();
                curso.Descripcion = txtDescripcion.Text.Trim();
                curso.AreaId = areaId;
                curso.InstructorId = instructorId;
                curso.DuracionHoras = duracion;

                // ✅ Convertir desde value "0/1/2" a enum
                int nivelVal;
                if (!int.TryParse(ddlNivel.SelectedValue, out nivelVal))
                {
                    lblMsg.Text = "Nivel inválido.";
                    return;
                }
                curso.Nivel = (NivelCurso)nivelVal;

                curso.Activo = chkActivo.Checked;
                curso.FechaActualizacion = ahora;

                // Guarda primero para asegurar Id (si es nuevo)
                db.SaveChanges();

                // ===== Subida de portada (si hay archivo) =====
                if (fuPortada.HasFile)
                {
                    var ext = Path.GetExtension(fuPortada.FileName).ToLowerInvariant();
                    var okExt = ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".webp";
                    if (!okExt)
                    {
                        lblMsg.Text = "Formato de imagen no permitido. Usa jpg, png o webp.";
                        return;
                    }

                    // Tamaño máximo (ej.: 5 MB)
                    const int MAX_MB = 5;
                    var maxBytes = MAX_MB * 1024 * 1024;
                    if (fuPortada.PostedFile.ContentLength > maxBytes)
                    {
                        lblMsg.Text = $"La imagen supera los {MAX_MB} MB.";
                        return;
                    }

                    // Validación MIME simple (opcional)
                    var contentType = fuPortada.PostedFile.ContentType.ToLowerInvariant();
                    var okMime = contentType.Contains("jpeg") || contentType.Contains("png") || contentType.Contains("webp");
                    if (!okMime)
                    {
                        // Algunos IIS reportan application/octet-stream; si eso pasa y confías en la ext, omite esto
                        // lblMsg.Text = "Tipo de contenido no válido.";
                        // return;
                    }

                    // Carpeta destino: ~/Uploads/cursos/{id}/
                    var carpetaVirtual = $"~/Uploads/cursos/{curso.Id}/";
                    var carpetaFs = Server.MapPath(carpetaVirtual);
                    if (!Directory.Exists(carpetaFs)) Directory.CreateDirectory(carpetaFs);

                    // Borra portada anterior si existe
                    if (!string.IsNullOrEmpty(curso.PortadaUrl))
                    {
                        try
                        {
                            var oldFs = Server.MapPath(curso.PortadaUrl);
                            if (File.Exists(oldFs)) File.Delete(oldFs);
                        }
                        catch { /* no romper por cleanup */ }
                    }

                    // Guarda nueva portada con nombre fijo
                    var fileName = "portada" + ext;
                    var destinoFs = Path.Combine(carpetaFs, fileName);
                    fuPortada.SaveAs(destinoFs);

                    var relative = VirtualPathUtility.ToAbsolute(carpetaVirtual + fileName);
                    curso.PortadaUrl = relative;

                    db.SaveChanges();

                    imgPreview.ImageUrl = relative;
                    imgPreview.Visible = true;
                }

                Response.Redirect("~/Admin/AdminCursos.aspx");
            }
        }
    }
}
