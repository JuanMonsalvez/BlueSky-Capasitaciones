using System;

namespace bluesky.Public
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Aquí va tu lógica de carga de página si la necesitas
        }
        protected void btnExplorarCursos_Click(object sender, EventArgs e)
        {
            // Redirigir a la nueva vista
            Response.Redirect("~/Usuario/Cursos.aspx");
        }
    }
}
