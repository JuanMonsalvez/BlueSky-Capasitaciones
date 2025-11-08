using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using bluesky.Models;
using bluesky.Services.Security;

namespace bluesky
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "jquery",
                new ScriptResourceDefinition
                {
                    Path = "~/js/jquery.min.js",
                    DebugPath = "~/js/jquery.js",
                    CdnPath = "https://code.jquery.com/jquery-3.7.1.min.js",
                    CdnDebugPath = "https://code.jquery.com/jquery-3.7.1.js"
                });

            TryEnsureAdminUser();
        }

        private void TryEnsureAdminUser()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var rolAdmin = db.Roles.FirstOrDefault(r => r.Nombre == "Admin");
                    if (rolAdmin == null)
                    {
                        rolAdmin = new Rol { Nombre = "Admin" };
                        db.Roles.Add(rolAdmin);
                        db.SaveChanges();
                    }

                    const string correo = "admin@bluesky.com";
                    var admin = db.Usuarios.FirstOrDefault(u => u.Correo == correo);
                    if (admin == null)
                    {
                        var hash = PasswordHasher.HashPassword("123456");
                        db.Usuarios.Add(new bluesky.Models.Usuario
                        {
                            NombreCompleto = "Administrador BlueSky",
                            Correo = correo,
                            PasswordHash = hash,
                            RolId = rolAdmin.Id,
                            Activo = true,
                            FechaRegistro = DateTime.UtcNow
                        });
                        db.SaveChanges();
                    }
                }
            }
            catch { /* log si quieres */ }
        }
    }
}
