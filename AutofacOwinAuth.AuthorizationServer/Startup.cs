using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutofacOwinAuth.AuthorizationServer.Startup))]
namespace AutofacOwinAuth.AuthorizationServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
