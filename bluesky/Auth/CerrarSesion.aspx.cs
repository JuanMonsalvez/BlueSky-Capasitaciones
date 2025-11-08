using System;
using System.Web.UI;
using bluesky.Services.Security;

namespace bluesky
{
    public partial class CerrarSesion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthHelper.SignOut();
            Response.Redirect("~/IniciarSesion.aspx", endResponse: true);
        }
    }
}
