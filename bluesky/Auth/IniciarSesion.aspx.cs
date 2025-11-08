using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using bluesky.Models;
using bluesky.Services.Security;

namespace bluesky
{
    public partial class IniciarSesion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si ya está logueado, redirige a su destino
            if (!IsPostBack && AuthHelper.IsAuthenticated())
            {
                Response.Redirect(GetDefaultLandingByRole(AuthHelper.GetCurrentUserRole()), endResponse: true);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            var correo = (txtCorreo.Text ?? "").Trim();
            var pass = (txtPassword.Text ?? "");

            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(pass))
            {
                lblMensaje.Text = "Debe ingresar correo y contraseña.";
                return;
            }

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    // Busca usuario activo por correo
                    var user = db.Usuarios.FirstOrDefault(u => u.Correo == correo && u.Activo);
                    if (user == null)
                    {
                        lblMensaje.Text = "Usuario o contraseña inválidos.";
                        return;
                    }

                    // Verifica password (tu hasher PBKDF2)
                    if (!PasswordHasher.VerifyPassword(pass, user.PasswordHash))
                    {
                        lblMensaje.Text = "Usuario o contraseña inválidos.";
                        return;
                    }

                    // Carga rol (si no está incluido)
                    var rolNombre = user.Rol != null ? user.Rol.Nombre
                                  : db.Roles.Where(r => r.Id == user.RolId).Select(r => r.Nombre).FirstOrDefault() ?? "Usuario";

                    // Crea sesión
                    AuthHelper.SignIn(user, rolNombre);

                    // Redirección segura
                    var target = GetSafeReturnUrl(Request["returnUrl"]);
                    if (string.IsNullOrEmpty(target))
                        target = GetDefaultLandingByRole(rolNombre);

                    Response.Redirect(target, endResponse: true);
                }
            }
            catch (Exception ex)
            {
                // En DEV puedes loguear ex.ToString()
                lblMensaje.Text = "Ocurrió un error al iniciar sesión.";
            }
        }

        private string GetDefaultLandingByRole(string role)
        {
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                return ResolveUrl("~/Admin/AdminDashboard.aspx"); // ajusta si usas otra página de admin
            return ResolveUrl("~/Usuario/Cursos.aspx");
        }

        private string GetSafeReturnUrl(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            // Solo permitimos URLs locales para evitar open redirects
            if (IsLocalUrl(raw)) return raw;

            try
            {
                var decoded = HttpUtility.UrlDecode(raw);
                if (IsLocalUrl(decoded)) return decoded;
            }
            catch { /* ignore */ }

            return null;
        }

        private bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            if (url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) return true;
            if (url.Length > 1 && url[0] == '~' && url[1] == '/') return true;
            return false;
        }
    }
}
