using System;
using System.Web.UI;

namespace bluesky
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioNombre"] != null)
                lblUsuario.Text = Session["UsuarioNombre"].ToString();
        }

        // Utilidad para evaluar rol admin desde el markup
        public static bool IsAdmin(System.Web.SessionState.HttpSessionState session)
        {
            var role = session?["UsuarioRol"] as string ?? "";
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}
