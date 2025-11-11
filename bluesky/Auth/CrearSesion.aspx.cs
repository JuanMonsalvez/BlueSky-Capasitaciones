using bluesky.Models;
using bluesky.Services.Security;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace bluesky.Auth
{
    public partial class CrearSesion : Page
    {
        // Política server-side por si deshabilitan JS
        private const string PasswordPolicyRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$";

        protected void Page_Load(object sender, EventArgs e)
        {
            // nada especial por ahora
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var nombre = txtNombre.Text.Trim();
            var correo = txtCorreo.Text.Trim().ToLowerInvariant();
            var pass = txtPassword.Text;

            // Validación de contraseña del lado servidor
            if (!System.Text.RegularExpressions.Regex.IsMatch(pass, PasswordPolicyRegex))
            {
                valSummary.HeaderText = "Corrige los errores:";
                valSummary.ShowSummary = true;
                Page.Validators.Add(new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "La contraseña debe tener mínimo 8 caracteres, con mayúscula, minúscula y número."
                });
                return;
            }

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    // ¿Correo ya existe?
                    var existe = db.Usuarios.Any(u => u.Correo == correo);
                    if (existe)
                    {
                        valSummary.HeaderText = "Corrige los errores:";
                        valSummary.ShowSummary = true;
                        Page.Validators.Add(new CustomValidator
                        {
                            IsValid = false,
                            ErrorMessage = "El correo ya está registrado."
                        });
                        return;
                    }

                    // Rol por defecto: "Usuario"
                    var rolUsuario = db.Roles.FirstOrDefault(r => r.Nombre == "Usuario");
                    if (rolUsuario == null)
                    {
                        // fallback: lo creamos si no existe (no debería pasar por el seed)
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

                // Redirección a iniciar sesión con mensajito
                Response.Redirect(ResolveUrl("~/Auth/IniciarSesion?created=1"), endResponse: true);
            }
            catch (Exception ex)
            {
                valSummary.HeaderText = "Ocurrió un error:";
                valSummary.ShowSummary = true;
                Page.Validators.Add(new CustomValidator
                {
                    IsValid = false,
                    ErrorMessage = "No se pudo crear la cuenta. Intenta nuevamente. Detalle: " + ex.Message
                });
            }
        }
    }
}
