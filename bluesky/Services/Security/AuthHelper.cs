using System;
using System.Web;
using bluesky.Models;

namespace bluesky.Services.Security
{
    public static class AuthHelper
    {
        private const string SessionUserId = "UsuarioId";
        private const string SessionUserName = "UsuarioNombre";
        private const string SessionUserRole = "UsuarioRol";

        public static bool IsAuthenticated()
        {
            return HttpContext.Current?.Session?[SessionUserId] != null;
        }

        public static int? GetCurrentUserId()
        {
            if (!IsAuthenticated()) return null;
            return (int?)HttpContext.Current.Session[SessionUserId];
        }

        public static string GetCurrentUserRole()
        {
            if (!IsAuthenticated()) return null;
            return (string)HttpContext.Current.Session[SessionUserRole];
        }

        public static void EnsureAuthenticatedOrRedirect(string loginUrl)
        {
            if (IsAuthenticated()) return;

            var ctx = HttpContext.Current;
            var returnUrl = ctx != null ? HttpUtility.UrlEncode(ctx.Request.RawUrl) : "";
            ctx.Response.Redirect($"{VirtualPathUtility.ToAbsolute(loginUrl)}?returnUrl={returnUrl}", true);
        }

        // 👇 Aquí se usa el namespace completo para evitar conflicto
        public static void SignIn(bluesky.Models.Usuario user, string rolNombre)
        {
            var sess = HttpContext.Current.Session;
            sess[SessionUserId] = user.Id;
            sess[SessionUserName] = user.NombreCompleto;
            sess[SessionUserRole] = rolNombre ?? "Usuario";
        }

        public static void SignOut()
        {
            var ctx = HttpContext.Current;
            ctx.Session.Clear();
            ctx.Session.Abandon();
        }
    }
}
