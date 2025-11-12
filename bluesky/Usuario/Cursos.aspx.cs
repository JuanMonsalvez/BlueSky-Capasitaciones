using bluesky.Models;
using System;
using System.IO;
using System.Linq;
using bluesky.App_Code;

namespace bluesky.Publico
{
    public partial class Cursos : ProtectedPage
    {
        private const string PLACEHOLDER = "~/img/curso-placeholder.jpg";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Cargar();
        }

        private void Cargar()
        {
            using (var db = new ApplicationDbContext())
            {
                var data = db.Cursos
                    .Where(c => c.Activo)
                    .OrderByDescending(c => c.FechaCreacion)
                    .ToList()
                    .Select(c => new
                    {
                        c.Id,
                        c.Titulo,
                        Resumen = (c.Descripcion ?? "").Length > 220
                                  ? (c.Descripcion.Substring(0, 220) + "…")
                                  : (c.Descripcion ?? ""),
                        Nivel = c.Nivel.ToString(),
                        Imagen = UrlOrPlaceholder(c.PortadaUrl),
                        LinkDetalle = ResolveUrl($"~/Usuario/CursoDetalle.aspx?id={c.Id}")
                    })
                    .ToList();

                rpCursos.DataSource = data;
                rpCursos.DataBind();

                lblVacio.Visible = data.Count == 0;
            }
        }


        private string UrlOrPlaceholder(string portada)
        {
            if (string.IsNullOrWhiteSpace(portada)) return ResolveUrl(PLACEHOLDER);
            var fs = Server.MapPath(portada);
            return File.Exists(fs) ? ResolveUrl(portada) : ResolveUrl(PLACEHOLDER);
        }
    }
}
