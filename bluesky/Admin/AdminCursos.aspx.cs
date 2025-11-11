using System;
using System.Linq;
using bluesky.Services;            // si tienes CursosService
using bluesky.Models;
using bluesky.App_Code;

namespace bluesky.Admin
{
    // Solo Admin
    public partial class AdminCursos : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }

        private void BindGrid()
        {
            var filtro = (txtFiltro.Text ?? "").Trim();

            using (var db = new ApplicationDbContext())
            {
                // Campos mínimos para evitar discrepancias con tu modelo.
                // Ajusta si usas otros nombres (e.g., Nombre en vez de Titulo).
                var query = db.Cursos.Select(c => new
                {
                    c.Id,
                    c.Titulo,   // ⚠ si tu propiedad es Nombre, cámbialo a c.Nombre
                    c.Activo
                });

                if (!string.IsNullOrEmpty(filtro))
                    query = query.Where(c => c.Titulo.Contains(filtro));

                gvCursos.DataSource = query
                    .OrderByDescending(c => c.Id)
                    .ToList();
                gvCursos.DataBind();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e) => BindGrid();

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            BindGrid();
        }

        protected void gvCursos_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvCursos.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnEliminar_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            try
            {
                var id = int.Parse(e.CommandArgument.ToString());
                using (var db = new ApplicationDbContext())
                {
                    var curso = db.Cursos.FirstOrDefault(x => x.Id == id);
                    if (curso == null)
                    {
                        lblMsg.Text = "El curso no existe o ya fue eliminado.";
                        return;
                    }

                    db.Cursos.Remove(curso);
                    db.SaveChanges();
                }
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al eliminar el curso.";
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
