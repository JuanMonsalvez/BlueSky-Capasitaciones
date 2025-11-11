using System;
using System.Linq;
using System.Web;
using System.Web.UI;          // ScriptManager / ScriptResourceDefinition
using System.Web.Routing;     // RouteTable / RouteCollection
using bluesky.Models;
using bluesky.Services.Security;

namespace bluesky
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterJqueryMapping();
            RegisterRoutes(RouteTable.Routes);
            EnsureRolesAndAdmin();
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        }


        // 1) Rutas amigables
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            // Home (asegúrate de tener ~/Public/Default.aspx)
            routes.MapPageRoute(
                routeName: "HomeDefault",
                routeUrl: "",
                physicalFile: "~/Public/Default.aspx"
            );

            // Auth
            routes.MapPageRoute("AuthLogin", "Auth/IniciarSesion", "~/Auth/IniciarSesion.aspx");
            routes.MapPageRoute("AuthRegister", "Auth/CrearSesion", "~/Auth/CrearSesion.aspx");
            routes.MapPageRoute("AuthCerrarSesion", "Auth/CerrarSesion", "~/Auth/CerrarSesion.aspx");

            // Usuario
            routes.MapPageRoute("UsuarioCursos", "Usuario/Cursos", "~/Usuario/Cursos.aspx");
            routes.MapPageRoute("UsuarioMiProgreso", "Usuario/MiProgreso", "~/Usuario/MiProgreso.aspx");

            // Admin
            routes.MapPageRoute(routeName: "AdminUsuarios",routeUrl: "Admin/AdminUsuarios",physicalFile: "~/Admin/AdminUsuarios.aspx");
            routes.MapPageRoute(routeName: "AdminCursoEditar", routeUrl: "Admin/Curso/editar/{id}", physicalFile: "~/Admin/CursoEditar.aspx");
            routes.MapPageRoute("AdminReportes", "Admin/Reportes", "~/Admin/AdminReportes.aspx");
            routes.MapPageRoute("AdminDashboard", "Admin/Dashboard", "~/Admin/AdminDashboard.aspx");
            routes.MapPageRoute("CursosEditar", "Admin/Cursos/Editar/{cursoId}", "~/Admin/CursoEditar.aspx");
        }

        // 2) jQuery mapping para WebForms unobtrusive validation
        private static void RegisterJqueryMapping()
        {
            var localPath = "~/js/jquery-3.7.1.min.js"; // ajusta si tu archivo está en otra carpeta
            var localDebug = "~/js/jquery-3.7.1.js";

            var cdn = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.7.1.min.js";
            var cdnDebug = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.7.1.js";

            var def = new ScriptResourceDefinition
            {
                Path = localPath,
                DebugPath = localDebug,
                CdnPath = cdn,
                CdnDebugPath = cdnDebug,
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            };

            // Debe llamarse exactamente "jquery"
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", def);
        }

        // 3) Seed de roles y admin
        private static void EnsureRolesAndAdmin()
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
                    }

                    var rolUsuario = db.Roles.FirstOrDefault(r => r.Nombre == "Usuario");
                    if (rolUsuario == null)
                    {
                        rolUsuario = new Rol { Nombre = "Usuario" };
                        db.Roles.Add(rolUsuario);
                    }

                    db.SaveChanges();

                    // Admin por defecto (cambia la contraseña en producción)
                    const string adminEmail = "admin@bluesky.com";
                    if (!db.Usuarios.Any(u => u.Correo == adminEmail))
                    {
                        var hash = PasswordHasher.HashPassword("Admin123");
                        var admin = new Models.Usuario
                        {
                            NombreCompleto = "Administrador BlueSky",
                            Correo = adminEmail,
                            PasswordHash = hash,
                            RolId = rolAdmin.Id,
                            Activo = true,
                            FechaRegistro = DateTime.UtcNow
                        };
                        db.Usuarios.Add(admin);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Seed admin/roles error: " + ex.Message);
            }
        }
    }
}
