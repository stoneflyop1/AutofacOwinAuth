using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AutofacOwinAuth.HomeWeb
{
    public partial class Startup
	{
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public static string PublicClientId { get; private set; }

		// 有关配置身份验证的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = System.Configuration.ConfigurationManager.AppSettings["cookieName"] ?? "cadsimula",
                LoginPath = new PathString("/Account/Login"),
                CookieDomain = System.Configuration.ConfigurationManager.AppSettings["domain"] ?? ".dbworld.cn",
                //ReturnUrlParameter = "ReturnUrl",
                CookieHttpOnly = true,
                CookieSecure = CookieSecureOption.SameAsRequest,
				Provider = new CookieAuthenticationProvider { OnApplyRedirect = ApplyRedirect }
                //Provider = new CookieAuthenticationProvider
                //{
                //    // 当用户登录时使应用程序可以验证安全戳。
                //    // 这是一项安全功能，当你更改密码或者向帐户添加外部登录名时，将使用此功能。
                //    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                //        validateInterval: TimeSpan.FromMinutes(30),
                //        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                //}
            });

            // 针对基于 OAuth 的流配置应用程序
            PublicClientId = "self";
        }
        //http://stackoverflow.com/questions/21275399/login-page-on-different-domain
        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {
            Uri absoluteUri;
            if (Uri.TryCreate(context.RedirectUri, UriKind.Absolute, out absoluteUri))
            {
                var path = PathString.FromUriComponent(absoluteUri);
                if (path == context.OwinContext.Request.PathBase + context.Options.LoginPath)
                {
                    var loginUrl = System.Configuration.ConfigurationManager.AppSettings["loginUrl"] ?? "http://sso.zjf.dbworld.cn/Account/Login";
                    context.RedirectUri = loginUrl +
                        new QueryString(
                            context.Options.ReturnUrlParameter,
                            context.Request.Uri.AbsoluteUri);
                }
            }

            context.Response.Redirect(context.RedirectUri);
        }
	}
}