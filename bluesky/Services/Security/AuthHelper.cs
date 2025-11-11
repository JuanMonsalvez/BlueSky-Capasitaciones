using System;
using System.Web;
using bluesky.Models;

namespace bluesky.Services.Security
{
    public static class AuthHelper
    {
        // Claves de sesión
        private const string SessionUserId = "UsuarioId";
        private const string SessionUserName = "UsuarioNombre";
        private const string SessionUserRole = "UsuarioRol";

        // --- Estado de sesión ---
        public static bool IsAuthenticated()
            => HttpContext.Current?.Session?[SessionUserId] != null;

        public static int? GetCurrentUserId()
        {
            if (!IsAuthenticated()) return null;
            var raw = HttpContext.Current.Session[SessionUserId];
            if (raw == null) return null;
            try { return (int)raw; }
            catch { return Convert.ToInt32(raw); }
        }

        public static string GetCurrentUserRole()
        {
            if (!IsAuthenticated()) return string.Empty;
            return (HttpContext.Current.Session[SessionUserRole] as string) ?? string.Empty;
        }

        // --- Redirecciones seguras ---
        public static void EnsureAuthenticatedOrRedirect(string loginUrl)
        {
            if (IsAuthenticated()) return;

            var ctx = HttpContext.Current;
            var rawReturn = ctx?.Request?.RawUrl ?? "/";
            var safeReturn = SafeLocalReturnUrl(rawReturn);

            var loginAbs = VirtualPathUtility.ToAbsolute(loginUrl);
            var url = string.IsNullOrEmpty(safeReturn)
                ? loginAbs
                : $"{loginAbs}?returnUrl={HttpUtility.UrlEncode(safeReturn)}";

            ctx.Response.Redirect(url, endResponse: true);
        }

        public static void EnsureRoleOrRedirect(string requiredRole, string fallbackUrl)
        {
            var role = GetCurrentUserRole();
            if (!role.Equals(requiredRole ?? "", StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Current.Response.Redirect(
                    VirtualPathUtility.ToAbsolute(fallbackUrl),
                    endResponse: true
                );
            }
        }

        // --- Login / Logout ---
        public static void SignIn(Models.Usuario user, string rolNombre)
        {
            var sess = HttpContext.Current.Session;
            sess[SessionUserId] = user.Id;
            sess[SessionUserName] = user.NombreCompleto;
            sess[SessionUserRole] = string.IsNullOrWhiteSpace(rolNombre) ? "Usuario" : rolNombre.Trim();
        }

        public static void SignOut()
        {
            // Limpia sesión
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            // (Opcional) Invalida cookie propia si la usas
            var cookie = new HttpCookie("BSKY_AUTH", "")
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true
            };
            HttpContext.Current.Response.Cookies.Add(cookie);

            // Defensa contra "back button": deshabilita caché del navegador en la respuesta actual
            ApplyNoCache(HttpContext.Current.Response);
        }

        // --- Utilidades ---
        /// <summary>
        /// Permite sólo returns locales del estilo "/ruta" y bloquea //host o rutas raras.
        /// </summary>
        private static string SafeLocalReturnUrl(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl)) return "";
            // Debe comenzar con "/" y NO con "//" ni "/\"
            if (!returnUrl.StartsWith("/")) return "";
            if (returnUrl.StartsWith("//") || returnUrl.StartsWith(@"/\")) return "";
            // Puedes filtrar más si quieres (ej: evitar volver a /Auth/*)
            return returnUrl;
        }

        /// <summary>
        /// Encabezados para evitar que el navegador muestre páginas protegidas con el botón "Atrás".
        /// Llamar en páginas protegidas (OnLoad) o después de SignOut.
        /// </summary>
        public static void ApplyNoCache(HttpResponse response)
        {
            if (response == null) return;
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        }
    }
}
