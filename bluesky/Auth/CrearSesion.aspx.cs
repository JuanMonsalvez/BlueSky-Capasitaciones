using System;
using System.Linq;
using System.Web;
using bluesky.Models;
using bluesky.Services.Security;

namespace bluesky.Auth
{
    public partial class CrearSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No-cache para evitar “volver atrás” con datos posteados
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var nombre = txtNombre.Text.Trim();
            var correo = txtCorreo.Text.Trim().ToLowerInvariant();
            var pass = txtPass.Text;

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    // correo único
                    if (db.Usuarios.Any(u => u.Correo == correo))
                    {
                        lblMensaje.Text = "Ese correo ya está registrado.";
                        return;
                    }

                    // rol por defecto = "Usuario" (si no existe, lo crea)
                    var rolUsuario = db.Roles.FirstOrDefault(r => r.Nombre == "Usuario");
                    if (rolUsuario == null)
                    {
                        rolUsuario = new Rol { Nombre = "Usuario" };
                        db.Roles.Add(rolUsuario);
                        db.SaveChanges();
                    }

                    var hash = PasswordHasher.HashPassword(pass);

                    var nuevo = new Models.Usuario
                    {
                        NombreCompleto = nombre,
                        Correo = correo,
                        PasswordHash = hash,
                        RolId = rolUsuario.Id,
                        Activo = true,
                        FechaRegistro = DateTime.UtcNow
                    };

                    db.Usuarios.Add(nuevo);
                    db.SaveChanges();
                }

                // Redirige a iniciar sesión con mensaje
                var loginUrl = ResolveUrl("~/Auth/IniciarSesion.aspx?msg=cuenta_creada");
                Response.Redirect(loginUrl, endResponse: true);
            }
            catch (Exception ex)
            {
                // log (si tienes logging)
                lblMensaje.Text = "No se pudo crear la cuenta. Intenta nuevamente.";
                System.Diagnostics.Debug.WriteLine("CrearSesion error: " + ex);
            }
        }
    }
}
