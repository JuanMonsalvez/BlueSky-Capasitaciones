using System;
using System.Web;
using System.Web.UI;
using bluesky.Services.Security;

namespace bluesky.App_Code
{
    public class AdminPage : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Debe estar autenticado
            if (!AuthHelper.IsAuthenticated())
            {
                var login = "~/Auth/IniciarSesion.aspx";
                var ret = HttpUtility.UrlEncode(Request.RawUrl);
                Response.Redirect(VirtualPathUtility.ToAbsolute(string.Format("{0}?returnUrl={1}", login, ret)), true);
                return;
            }

            // Debe ser Admin
            var role = AuthHelper.GetCurrentUserRole();
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect(VirtualPathUtility.ToAbsolute("~/Default.aspx"), true);
                return;
            }

            // anti-cache
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        }
    }
}