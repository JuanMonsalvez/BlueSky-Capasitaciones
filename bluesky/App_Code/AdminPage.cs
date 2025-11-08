using System;
using System.Web;
using System.Web.UI;
using bluesky.Services.Security;

namespace bluesky.App_Code
{
    /// <summary>
    /// Página base para rutas de administración.
    /// Requiere sesión activa y rol "Admin".
    /// </summary>
    public class AdminPage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            // 1) exige login
            AuthHelper.EnsureAuthenticatedOrRedirect("~/IniciarSesion.aspx");

            // 2) valida rol
            var role = AuthHelper.GetCurrentUserRole();
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Current.Response.Redirect("~/Public/Default.aspx", endResponse: true);
                return;
            }

            base.OnLoad(e);
        }
    }
}
