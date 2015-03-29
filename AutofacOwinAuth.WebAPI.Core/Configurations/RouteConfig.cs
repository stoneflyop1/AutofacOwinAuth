using System.Web.Http;

namespace AutofacOwinAuth.WebAPI.Core.Configurations
{
    public class RouteConfig
    {
        public static void MapRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
