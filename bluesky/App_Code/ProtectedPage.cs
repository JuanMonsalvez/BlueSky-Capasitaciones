using System;
using System.Web.UI;
using bluesky.Services.Security;

namespace bluesky.App_Code
{
    /// <summary>
    /// Página base para usuarios autenticados.
    /// Redirige automáticamente al login si no hay sesión activa.
    /// </summary>
    public class ProtectedPage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            AuthHelper.EnsureAuthenticatedOrRedirect("~/IniciarSesion.aspx");
            base.OnLoad(e);
        }
    }
}
