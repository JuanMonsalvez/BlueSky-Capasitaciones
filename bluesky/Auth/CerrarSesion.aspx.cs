using System;
using System.Web;
using bluesky.Services.Security;

namespace bluesky.Auth
{
    public partial class CerrarSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) cerrar sesión
            AuthHelper.SignOut();

            // 2) evitar back button de inmediato
            AuthHelper.ApplyNoCache(Response);

            // 3) redirigir al login con un mensaje
            var url = "~/Auth/IniciarSesion.aspx?msg=logout";
            Response.Redirect(VirtualPathUtility.ToAbsolute(url), endResponse: true);
        }
    }
}
