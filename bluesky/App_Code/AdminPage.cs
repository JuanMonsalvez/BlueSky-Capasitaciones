using System;
using System.Web;
using bluesky.Services.Security;

namespace bluesky.App_Code
{
    public class AdminPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            // 1) Requiere sesión
            AuthHelper.EnsureAuthenticatedOrRedirect("~/Auth/IniciarSesion.aspx");

            // 2) Requiere rol Admin
            AuthHelper.EnsureRoleOrRedirect("Admin", "~/Public/Default.aspx");

            // 3) Evita cache de navegador
            AuthHelper.ApplyNoCache(Response);

            base.OnLoad(e);
        }
    }
}