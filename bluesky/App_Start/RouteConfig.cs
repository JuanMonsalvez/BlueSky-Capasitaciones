using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace bluesky
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings
            {
                AutoRedirectMode = RedirectMode.Off // no fuerces 301, sirve /x y /x.aspx
            };
            routes.EnableFriendlyUrls(settings);
        }
    }
}
