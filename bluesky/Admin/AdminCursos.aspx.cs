using bluesky.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace bluesky.Admin
{
    public partial class AdminCursos : App_Code.AdminPage
    {
        private const string PLACEHOLDER = "~/img/curso-placeholder.jpg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }

        private void BindGrid()
        {
            using (var db = new ApplicationDbContext())
            {
                var data = db.Cursos
                    .OrderByDescending(c => c.FechaCreacion)
                    .Select(c => new
                    {
                        c.Id,
                        c.Titulo,
                        Nivel = c.Nivel.ToString(),
                        c.Activo,
                        c.FechaCreacion,
                        c.PortadaUrl
                    })
                    .ToList();

                gvCursos.DataSource = data;
                gvCursos.DataBind();
            }
        }

        protected void gvCursos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCursos.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvCursos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var img = e.Row.FindControl("imgPortada") as Image;
            if (img == null) return;

            var dataItem = (dynamic)e.Row.DataItem;
            var url = (string)dataItem.PortadaUrl;

            if (string.IsNullOrWhiteSpace(url))
            {
                img.ImageUrl = ResolveUrl(PLACEHOLDER);
            }
            else
            {
                // Usa portada si existe físicamente, de lo contrario placeholder
                var fs = Server.MapPath(url);
                img.ImageUrl = File.Exists(fs) ? ResolveUrl(url) : ResolveUrl(PLACEHOLDER);
            }
        }

        protected void gvCursos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int id;
                if (int.TryParse(e.CommandArgument.ToString(), out id))
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var c = db.Cursos.Find(id);
                        if (c == null) { lblMsg.Text = "Curso no encontrado."; return; }

                        // Limpia carpeta de portada
                        if (!string.IsNullOrWhiteSpace(c.PortadaUrl))
                        {
                            try
                            {
                                var dir = System.IO.Path.GetDirectoryName(Server.MapPath(c.PortadaUrl));
                                if (Directory.Exists(dir)) Directory.Delete(dir, true);
                            }
                            catch { /* noop */ }
                        }

                        db.Cursos.Remove(c);
                        db.SaveChanges();
                    }
                    BindGrid();
                }
            }
        }
    }
}
