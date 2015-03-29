using Owin;
using Microsoft.Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using AutofacOwinAuth.WebAPI.Core.Configurations;
using System.Web;

[assembly: OwinStartup(typeof(AutofacOwinAuth.WebAPI.WebApplication.Startup))]

namespace AutofacOwinAuth.WebAPI.WebApplication
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration config = new HttpConfiguration(); // GlobalConfiguration.Configuration;

            AutofacConfig.Initialize(config);

            OAuthConfig.ConfigOAuth(app, config);

            RouteConfig.MapRoutes(config);

            app.UseWebApi(config);
        }
    }
}