using Owin;
using Microsoft.Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using AutofacOwinAuth.WebAPI.Core.Configurations;


//[assembly: OwinStartup(typeof(AutofacOwinAuth.WebAPI.Core.Startup))]

namespace AutofacOwinAuth.WebAPI.OwinHost
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            AutofacConfig.Initialize(config);

            OAuthConfig.ConfigOAuth(app, config);

            RouteConfig.MapRoutes(config);            

            app.UseWebApi(config);
        }

        
        
    }
}
