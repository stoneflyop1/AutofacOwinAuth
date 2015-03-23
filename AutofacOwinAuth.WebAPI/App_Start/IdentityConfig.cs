//using System;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using AutofacOwinAuth.Core.Domain;
//using AutofacOwinAuth.Core.Service;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.DataProtection;

//namespace AutofacOwinAuth.WebAPI
//{
//    // 配置此应用程序中使用的应用程序用户管理器。UserManager 在 ASP.NET Identity 中定义，并由此应用程序使用。

//    public class ApplicationUserStore : IUserPasswordStore<User,int>, IUserEmailStore<User,int>,
//        IUserLockoutStore<User,int>, IUserTwoFactorStore<User,int>, IUserStore<User, int>
//    {
//        private readonly IUserService _serv;

//        public ApplicationUserStore(IUserService serv)
//        {
//            _serv = serv;
//        }

//        public Task CreateAsync(User user)
//        {
//            return Task.Run(() => _serv.InsertUser(user));
//        }

//        public Task DeleteAsync(User user)
//        {
//            return Task.Run(() => _serv.DeleteUser(user));
//        }

//        public Task<User> FindByIdAsync(int userId)
//        {
//            return Task.Run(() => _serv.GetUserById(userId));
//        }

//        public Task<User> FindByNameAsync(string userName)
//        {
//            return Task.Run(() => _serv.GetUserByUserName(userName));
//        }

//        public Task UpdateAsync(User user)
//        {
//            return Task.Run(() => _serv.UpdateUser(user));
//        }

//        public void Dispose()
//        {
            
//        }

//        public Task<string> GetPasswordHashAsync(User user)
//        {
//            return Task.FromResult(user.Password);
//        }

//        public Task<bool> HasPasswordAsync(User user)
//        {
//            return Task.FromResult(!String.IsNullOrEmpty(user.Password));
//        }

//        public Task SetPasswordHashAsync(User user, string passwordHash)
//        {
//            user.Password = passwordHash;
//            return Task.Run(() => _serv.UpdateUser(user));
//        }

//        public Task<User> FindByEmailAsync(string email)
//        {
//            return Task.Run(() => _serv.GetUserByEmail(email));
//        }

//        public Task<string> GetEmailAsync(User user)
//        {
//            return Task.FromResult(user.Email);
//        }

//        public Task<bool> GetEmailConfirmedAsync(User user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetEmailAsync(User user, string email)
//        {
//            return Task.Run(() => user.Email = email);
//        }

//        public Task SetEmailConfirmedAsync(User user, bool confirmed)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<int> GetAccessFailedCountAsync(User user)
//        {
//            return Task.FromResult(0);
//        }

//        public Task<bool> GetLockoutEnabledAsync(User user)
//        {
//            return Task.FromResult(false);
//        }

//        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<int> IncrementAccessFailedCountAsync(User user)
//        {
//            throw new NotImplementedException();
//        }

//        public Task ResetAccessFailedCountAsync(User user)
//        {
//            return Task.FromResult<object>(null);
//        }

//        public Task SetLockoutEnabledAsync(User user, bool enabled)
//        {
//            return Task.FromResult<object>(null);
//        }

//        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
//        {
//            return Task.FromResult<object>(null);
//        }

//        public Task<bool> GetTwoFactorEnabledAsync(User user)
//        {
//            return Task.FromResult(false);
//        }

//        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
//        {
//            return Task.FromResult(0);
//        }
//    }

//    public static class UserExtensions
//    {
//        public static Task<ClaimsIdentity> CreateUserIdentityAsync(this User user, UserManager<User, int> userManager)
//        {
//            return userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
//        }
//    }

//    public class ApplicationSignInManager : SignInManager<User, int>
//    {
//        public ApplicationSignInManager(UserManager<User,int> userManager, IAuthenticationManager authenticationManager)
//            : base(userManager, authenticationManager)
//        {
//        }

//        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
//        {
//            return user.CreateUserIdentityAsync(UserManager);
//        }

//        public override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
//        {
//            //var user = UserManager.FindByNameAsync(userName);
//            //user.Wait();
//            //var ok = UserManager.CheckPasswordAsync(user.Result, password);
//            //ok.Wait();
//            //password will be hashed in UserManager using its PasswordHash Property
//            return base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
//        }

//    }

//    public class ApplicationUserManager : UserManager<User, int>
//    {
//        public ApplicationUserManager(IUserStore<User, int> store)
//            : base(store)
//        {
//            UserValidator = new UserValidator<User, int>(this)
//            {
//                AllowOnlyAlphanumericUserNames = false,
//                RequireUniqueEmail = true
//            };

//            // Configure validation logic for passwords
//            PasswordValidator = new PasswordValidator
//            {
//                RequiredLength = 6,
//                RequireNonLetterOrDigit = false,
//                RequireDigit = false,
//                RequireLowercase = false,
//                RequireUppercase = false,
//            };

//            // Configure user lockout defaults
//            UserLockoutEnabledByDefault = true;
//            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
//            MaxFailedAccessAttemptsBeforeLockout = 5;

//            var dataProtectionProvider = Startup.DataProtectionProvider;
//            if (dataProtectionProvider != null)
//            {
//                IDataProtector dataProtector = dataProtectionProvider.Create("ASP.NET Identity");

//                UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtector);
//            }
//        }


//        public override Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
//        {
//            return base.ChangePasswordAsync(userId, currentPassword, newPassword);
//        }

//        public override Task<bool> CheckPasswordAsync(User user, string password)
//        {
//            return base.CheckPasswordAsync(user, password);
//        }

//        public override Task<IdentityResult> CreateAsync(User user, string password)
//        {
//            return base.CreateAsync(user, password);
//        }

//        public override Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword)
//        {
//            return base.ResetPasswordAsync(userId, token, newPassword);
//        }
//    }
//}
