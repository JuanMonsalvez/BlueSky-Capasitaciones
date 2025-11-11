using System;
using System.Web;
using bluesky.Services.Security;

namespace bluesky.App_Code
{
    public class ProtectedPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            // 1) Requiere sesión
            AuthHelper.EnsureAuthenticatedOrRedirect("~/Auth/IniciarSesion.aspx");

            // 2) Evita cache de navegador (back button)
            AuthHelper.ApplyNoCache(Response);

            base.OnLoad(e);
        }
    }
}
