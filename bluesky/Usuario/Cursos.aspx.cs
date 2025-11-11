using System;
using System.Linq;
using System.Web.UI;
using System.Data.Entity;
using bluesky.Models;
using bluesky.App_Code;

namespace bluesky.Usuario
{
    public partial class Cursos : ProtectedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Protección mínima por sesión
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("~/Auth/IniciarSesion.aspx?returnUrl=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            if (!IsPostBack)
                CargarCursos();
        }

        private void CargarCursos()
        {
            int usuarioId = Convert.ToInt32(Session["UsuarioId"]);

            using (var db = new ApplicationDbContext())
            {
                // Trae inscripciones + curso
                var query = db.UsuarioCursos
                              .Include(uc => uc.Curso)
                              .Where(uc => uc.UsuarioId == usuarioId)
                              .Select(uc => new
                              {
                                  uc.Curso.Id,
                                  uc.Curso.Titulo,
                                  DescripcionCorta = (uc.Curso.Descripcion ?? "").Length > 140
                                                      ? (uc.Curso.Descripcion.Substring(0, 140) + "…")
                                                      : (uc.Curso.Descripcion ?? ""),
                                  uc.Curso.Nivel,
                                  uc.Curso.DuracionHoras,
                                  // LinkDetalle = "~/Usuario/CursoDetalle.aspx?id=" + uc.Curso.Id
                              })
                              .ToList();

                if (query.Count == 0)
                {
                    rptCursos.Visible = false;
                    pnlEmpty.Visible = true;
                }
                else
                {
                    rptCursos.DataSource = query;
                    rptCursos.DataBind();
                    rptCursos.Visible = true;
                    pnlEmpty.Visible = false;
                }
            }
        }
    }
}
