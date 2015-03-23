//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.ModelBinding;
//using AutofacOwinAuth.Core.Domain;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OAuth;
//using AutofacOwinAuth.WebAPI.Models;
//using AutofacOwinAuth.WebAPI.Providers;
//using AutofacOwinAuth.WebAPI.Results;

//namespace AutofacOwinAuth.WebAPI.Controllers
//{
//    [Authorize]
//    [RoutePrefix("api/Account")]
//    public class AccountController : ApiController
//    {
//        private const string LocalLoginProvider = "Local";
//        private UserManager<User, int> _userManager;

//        public AccountController(UserManager<User,int> userManager, SignInManager<User, int> signInManager
//            ) //,ISecureDataFormat<AuthenticationTicket> accessTokenFormat
//        {
//            _userManager = userManager;
//            //AccessTokenFormat = accessTokenFormat;
//            _signInManager = signInManager;
//        }

//        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

//        // GET api/Account/UserInfo
//        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
//        [Route("UserInfo")]
//        public UserInfoViewModel GetUserInfo()
//        {
//            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

//            return new UserInfoViewModel
//            {
//                Email = User.Identity.GetUserName(),
//                HasRegistered = externalLogin == null,
//                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
//            };
//        }

//        // POST api/Account/Logout
//        [Route("Logout")]
//        public IHttpActionResult Logout()
//        {
//            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
//            return Ok();
//        }

//        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
//        [Route("ManageInfo")]
//        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
//        {
//            throw new NotImplementedException();
//            IdentityUser user = null; //await _userManager.FindByIdAsync(User.Identity.GetUserId<int>());

//            if (user == null)
//            {
//                return null;
//            }

//            var logins = user.Logins.Select(linkedAccount => new UserLoginInfoViewModel
//            {
//                LoginProvider = linkedAccount.LoginProvider, ProviderKey = linkedAccount.ProviderKey
//            }).ToList();

//            if (user.PasswordHash != null)
//            {
//                logins.Add(new UserLoginInfoViewModel
//                {
//                    LoginProvider = LocalLoginProvider,
//                    ProviderKey = user.UserName,
//                });
//            }

//            return new ManageInfoViewModel
//            {
//                LocalLoginProvider = LocalLoginProvider,
//                Email = user.UserName,
//                Logins = logins,
//                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
//            };
//        }

//        // POST api/Account/ChangePassword
//        [Route("ChangePassword")]
//        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            IdentityResult result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword,
//                model.NewPassword);
            
//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }
//        [AllowAnonymous]
//        [Route("ResetPassword")]
//        public async Task<IHttpActionResult> ResetPassword(ResetPasswordBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            var user = await _userManager.FindByEmailAsync(model.Email);
//            var token = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
//            var result = await _userManager.ResetPasswordAsync(user.Id, token, model.NewPassword);
//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        // POST api/Account/SetPassword
//        [Route("SetPassword")]
//        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            IdentityResult result = await _userManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);

//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            return Ok();
//        }

//        // POST api/Account/AddExternalLogin
//        [Route("AddExternalLogin")]
//        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

//            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

//            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
//                && ticket.Properties.ExpiresUtc.HasValue
//                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
//            {
//                return BadRequest("外部登录失败。");
//            }

//            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

//            if (externalData == null)
//            {
//                return BadRequest("外部登录已与某个帐户关联。");
//            }

//            IdentityResult result = await _userManager.AddLoginAsync(User.Identity.GetUserId<int>(),
//                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

//            return !result.Succeeded ? GetErrorResult(result) : Ok();
//        }

//        // POST api/Account/RemoveLogin
//        [Route("RemoveLogin")]
//        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            IdentityResult result;

//            if (model.LoginProvider == LocalLoginProvider)
//            {
//                result = await _userManager.RemovePasswordAsync(User.Identity.GetUserId<int>());
//            }
//            else
//            {
//                result = await _userManager.RemoveLoginAsync(User.Identity.GetUserId<int>(),
//                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
//            }

//            return !result.Succeeded ? GetErrorResult(result) : Ok();
//        }

//        // GET api/Account/ExternalLogin
//        [OverrideAuthentication]
//        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
//        [AllowAnonymous]
//        [Route("ExternalLogin", Name = "ExternalLogin")]
//        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
//        {
//            if (error != null)
//            {
//                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
//            }

//            if (!User.Identity.IsAuthenticated)
//            {
//                return new ChallengeResult(provider, this);
//            }

//            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

//            if (externalLogin == null)
//            {
//                return InternalServerError();
//            }

//            if (externalLogin.LoginProvider != provider)
//            {
//                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
//                return new ChallengeResult(provider, this);
//            }

//            var user = await _userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
//                externalLogin.ProviderKey));

//            bool hasRegistered = user != null;

//            if (hasRegistered)
//            {
//                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

//                ClaimsIdentity oAuthIdentity = await _userManager.CreateIdentityAsync(user,
//                    OAuthDefaults.AuthenticationType);
//                ClaimsIdentity cookieIdentity = await _userManager.CreateIdentityAsync(user,
//                    CookieAuthenticationDefaults.AuthenticationType);

//                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
//                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
//            }
//            else
//            {
//                IEnumerable<Claim> claims = externalLogin.GetClaims();
//                var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
//                Authentication.SignIn(identity);
//            }

//            return Ok();
//        }

//        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
//        [AllowAnonymous]
//        [Route("ExternalLogins")]
//        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
//        {
//            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
//            var logins = new List<ExternalLoginViewModel>();

//            string state;

//            if (generateState)
//            {
//                const int strengthInBits = 256;
//                state = RandomOAuthStateGenerator.Generate(strengthInBits);
//            }
//            else
//            {
//                state = null;
//            }

//            foreach (AuthenticationDescription description in descriptions)
//            {
//                var login = new ExternalLoginViewModel
//                {
//                    Name = description.Caption,
//                    Url = Url.Route("ExternalLogin", new
//                    {
//                        provider = description.AuthenticationType,
//                        response_type = "token",
//                        client_id = Startup.PublicClientId,
//                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
//                        state = state
//                    }),
//                    State = state
//                };
//                logins.Add(login);
//            }

//            return logins;
//        }

//        // cannot auth in oauth authentication
//        [AllowAnonymous]
//        [Route("Login")]
//        public async Task<IHttpActionResult> Login(UserLoginViewModel model)
//        {
//            if (User != null && User.Identity.IsAuthenticated)
//            {
//                Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
//            }
//            try
//            {
//                var status = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
//                if (status == SignInStatus.Success)
//                {
//                    var user = await _userManager.FindByEmailAsync(model.Email);
//                    ClaimsIdentity oAuthIdentity = await _userManager.CreateIdentityAsync(user,
//                    OAuthDefaults.AuthenticationType);
//                    ClaimsIdentity cookieIdentity = await _userManager.CreateIdentityAsync(user,
//                        CookieAuthenticationDefaults.AuthenticationType);
//                    Authentication.SignIn(oAuthIdentity, cookieIdentity);
//                    var u = Authentication.User;
//                    var k = u.Identity.IsAuthenticated;//false
//                    return Ok(user);
//                }
//                return BadRequest(status.ToString());
//            }
//            catch (AggregateException ex) //catch async exception
//            {
//                var innerEx = ex.InnerException;
//                throw;
//            }
//        }

//        // POST api/Account/Register
//        [AllowAnonymous]
//        [Route("Register")]
//        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            var user = new User
//            {
//                UserName = model.UserName,
//                Email = model.Email
//            };

//            try
//            {
//                var result = await _userManager.CreateAsync(user, model.Password);

//                return !result.Succeeded ? GetErrorResult(result) : Ok();
//            }
//            catch (Exception ex)
//            {
//                var innerEx = ex.InnerException;
//                throw;
//            }
//        }

//        // POST api/Account/RegisterExternal
//        [OverrideAuthentication]
//        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
//        [Route("RegisterExternal")]
//        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var info = await Authentication.GetExternalLoginInfoAsync();
//            if (info == null)
//            {
//                return InternalServerError();
//            }

//            var user = new User
//            {
//                UserName = model.Email,
//                Email = model.Email
//            };
//            //create user withour password...
//            IdentityResult result = await _userManager.CreateAsync(user);
//            if (!result.Succeeded)
//            {
//                return GetErrorResult(result);
//            }

//            result = await _userManager.AddLoginAsync(user.Id, info.Login);

//            return !result.Succeeded ? GetErrorResult(result) : Ok();
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && _userManager != null)
//            {
//                _userManager.Dispose();
//                _userManager = null;
//            }

//            base.Dispose(disposing);
//        }

//        #region 帮助程序

//        private IAuthenticationManager Authentication
//        {
//            get
//            {
//                return Request.GetOwinContext().Authentication;
//            }
//        }

//        private SignInManager<User, int> _signInManager;


//        private IHttpActionResult GetErrorResult(IdentityResult result)
//        {
//            if (result == null)
//            {
//                return InternalServerError();
//            }

//            if (!result.Succeeded)
//            {
//                if (result.Errors != null)
//                {
//                    foreach (string error in result.Errors)
//                    {
//                        ModelState.AddModelError("", error);
//                    }
//                }

//                if (ModelState.IsValid)
//                {
//                    // 没有可发送的 ModelState 错误，因此仅返回空 BadRequest。
//                    return BadRequest();
//                }

//                return BadRequest(ModelState);
//            }

//            return null;
//        }

//        private class ExternalLoginData
//        {
//            public string LoginProvider { get; set; }
//            public string ProviderKey { get; set; }
//            public string UserName { get; set; }

//            public IList<Claim> GetClaims()
//            {
//                IList<Claim> claims = new List<Claim>();
//                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

//                if (UserName != null)
//                {
//                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
//                }

//                return claims;
//            }

//            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
//            {
//                if (identity == null)
//                {
//                    return null;
//                }

//                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

//                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
//                    || String.IsNullOrEmpty(providerKeyClaim.Value))
//                {
//                    return null;
//                }

//                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
//                {
//                    return null;
//                }

//                return new ExternalLoginData
//                {
//                    LoginProvider = providerKeyClaim.Issuer,
//                    ProviderKey = providerKeyClaim.Value,
//                    UserName = identity.FindFirstValue(ClaimTypes.Name)
//                };
//            }
//        }

//        private static class RandomOAuthStateGenerator
//        {
//            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

//            public static string Generate(int strengthInBits)
//            {
//                const int bitsPerByte = 8;

//                if (strengthInBits % bitsPerByte != 0)
//                {
//                    throw new ArgumentException("strengthInBits 必须能被 8 整除。", "strengthInBits");
//                }

//                int strengthInBytes = strengthInBits / bitsPerByte;

//                byte[] data = new byte[strengthInBytes];
//                _random.GetBytes(data);
//                return HttpServerUtility.UrlTokenEncode(data);
//            }
//        }

//        #endregion
//    }
//}
