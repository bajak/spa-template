using System.Web.Mvc;
using System.Web.Routing;

namespace spa.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{*url}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}