using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutofacOwinAuth.HomeWeb.Startup))]
namespace AutofacOwinAuth.HomeWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
