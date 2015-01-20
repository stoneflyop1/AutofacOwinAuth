using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AutofacOwinAuth.WebAPI.Tests
{
    public class AccountModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class SetPasswordModel
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class TokenModel
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        //public string userName { get; set; }

        //public string Password { get; set; }

        //public string grant_type { get; set; }
    }

    public class RegisterBindingModel
    {

        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }
    }
}
