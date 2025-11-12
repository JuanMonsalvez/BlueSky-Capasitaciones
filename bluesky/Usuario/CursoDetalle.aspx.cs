using System;
using System.IO;
using System.Linq;
using System.Web;
using bluesky.App_Code;          // Para ProtectedPage (requiere sesión)
using bluesky.Models;

namespace bluesky.Usuario
{
    public partial class CursoDetalle : ProtectedPage
    {
        private const string PLACEHOLDER = "~/img/curso-placeholder.jpg";

        protected int? CursoId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) Cargar();
        }

        private void Cargar()
        {
            if (!CursoId.HasValue)
            {
                MostrarError("Curso no especificado.");
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var c = db.Cursos.FirstOrDefault(x => x.Id == CursoId.Value);
                if (c == null)
                {
                    MostrarError("Curso no encontrado.");
                    return;
                }

                // Datos del curso
                litTitulo.Text = HttpUtility.HtmlEncode(c.Titulo);
                litDescripcion.Text = HttpUtility.HtmlEncode(c.Descripcion ?? "");
                litNivel.Text = c.Nivel.ToString();
                litDuracion.Text = c.DuracionHoras.ToString();
                litArea.Text = c.AreaId.HasValue && c.Area != null ? c.Area.Nombre : "(sin área)";

                imgPortada.ImageUrl = UrlOrPlaceholder(c.PortadaUrl);

                // Buscar evaluación activa
                var eval = db.Evaluaciones
                             .Where(e => e.CursoId == c.Id && e.Activa)
                             .OrderByDescending(e => e.FechaCreacion)
                             .FirstOrDefault();

                if (eval != null)
                {
                    lnkEvaluacion.NavigateUrl = ResolveUrl($"~/Usuario/RendirEvaluacion.aspx?evaluacionId={eval.Id}");
                    lnkEvaluacion.Visible = true;
                    lblSinEval.Visible = false;
                }
                else
                {
                    lnkEvaluacion.Visible = false;
                    lblSinEval.Visible = true;
                }

                pnlBody.Visible = true;
                lblMsg.Visible = false;
            }
        }

        private string UrlOrPlaceholder(string portada)
        {
            if (string.IsNullOrWhiteSpace(portada)) return ResolveUrl(PLACEHOLDER);
            var fs = Server.MapPath(portada);
            return File.Exists(fs) ? ResolveUrl(portada) : ResolveUrl(PLACEHOLDER);
        }

        private void MostrarError(string msg)
        {
            pnlBody.Visible = false;
            lblMsg.Text = msg;
            lblMsg.Visible = true;
        }
    }
}
