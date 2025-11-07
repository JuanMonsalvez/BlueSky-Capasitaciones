using System;
using System.Web;

namespace bluesky
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Aquí podrías registrar bundles o rutas si las agregas en el futuro
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Manejo global de errores (opcional)
            // var ex = Server.GetLastError();
            // TODO: loguear si agregas un logger
        }
    }
}
