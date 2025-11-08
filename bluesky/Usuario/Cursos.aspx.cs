using System;
using System.Web.UI; // Necesario para Page
using bluesky.App_Code; // Asumiendo que ProtectedPage está en este namespace

namespace bluesky.Usuario
{
    public partial class Cursos : ProtectedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // tu lógica de carga de cursos
            }
        }
    }
}
