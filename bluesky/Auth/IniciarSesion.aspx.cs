using System;
using System.Linq;
using System.Web.UI;
using bluesky.Models;
using bluesky.Services.Security;

namespace bluesky
{
    public partial class IniciarSesion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.QueryString["created"] == "1")
            {
                // Mensaje tras registrarse exitosamente
                if (lblMensaje != null)
                    lblMensaje.Text = "<div class='alert alert-success'>Cuenta creada con éxito. Ahora inicia sesión.</div>";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var correo = txtCorreo.Text.Trim().ToLowerInvariant();
            var pass = txtPassword.Text;

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var user = db.Usuarios.FirstOrDefault(u => u.Correo == correo);
                    if (user == null || !user.Activo)
                    {
                        ShowError("Usuario o contraseña inválidos.");
                        return;
                    }

                    var ok = PasswordHasher.VerifyPassword(pass, user.PasswordHash);
                    if (!ok)
                    {
                        ShowError("Usuario o contraseña inválidos.");
                        return;
                    }

                    // Autenticar sesión
                    AuthHelper.SignIn(user, user.Rol != null ? user.Rol.Nombre : "Usuario");

                    // Redirigir a Cursos (ajusta si quieres otra página)
                    Response.Redirect(ResolveUrl("~/Usuario/Cursos"), endResponse: true);
                }
            }
            catch (Exception ex)
            {
                ShowError("Error al iniciar sesión: " + ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            if (lblMensaje != null)
                lblMensaje.Text = $"<div class='alert alert-danger'>{msg}</div>";
        }
    }
}
