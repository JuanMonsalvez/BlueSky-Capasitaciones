using System;
using System.Web;
using bluesky.Services.Security;

namespace bluesky
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Desactiva caché del navegador (para evitar sesión falsa al "volver atrás")
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            var isAuth = AuthHelper.IsAuthenticated();
            phAnon.Visible = !isAuth;
            phAuth.Visible = isAuth;

            if (isAuth)
            {
                var role = AuthHelper.GetCurrentUserRole();
                lblUsuario.Text = HttpUtility.HtmlEncode((string)Session["UsuarioNombre"] ?? "");

                // Solo mostrar el menú admin si es Admin
                bool esAdmin = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
                liAdmin.Attributes["style"] = esAdmin ? "display:block;" : "display:none;";
            }
            else
            {
                liAdmin.Attributes["style"] = "display:none;";
                lblUsuario.Text = string.Empty;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            AuthHelper.SignOut();
            Response.Redirect(ResolveUrl("~/Auth/IniciarSesion.aspx"), endResponse: true);
        }
    }
}
