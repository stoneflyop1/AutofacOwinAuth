using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AutofacOwinAuth.WebAPI.Tests
{
    [TestFixture]
    public class TestAccounts
    {
        private static string Host = "http://localhost:13648/";

        private static string routPrefix = "api/Auth";

        private static string UserName = "qq@qq.com";
        private static string Email = "qq@qq.com";
        private static string Password = "123456";//
        private static string NewPassword = "654321"; //


        static HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(Host)
            };
            return client;
        }
        [Test]
        public void TestRegister()
        {
            var client = GetClient();
            var password = Password;
            var model = new RegisterBindingModel { UserName=UserName, Email = Email, 
                Password = password, ConfirmPassword = password };
            var res = client.PostAsJsonAsync(routPrefix + "/Register", model);
            res.Wait();
            Assert.AreEqual(HttpStatusCode.OK, res.Result.StatusCode);
        }

        public static TokenModel GetToken()
        {
            return GetToken(new AccountModel {UserName = UserName, Password = Password});
        }

        internal static TokenModel GetToken(AccountModel model)
        {
            using (var client = GetClient())
            {
                var dict = new Dictionary<string, string>
                {
                    {"UserName", model.UserName},
                    {"Password", model.Password},
                    {"grant_type", "password"}
                };
                var m = new FormUrlEncodedContent(dict);
                var res = client.PostAsync("/Token", m);
                res.Wait();
                var obj = res.Result;
                //var list = new List<string>();
                //foreach (var h in obj.Headers)
                //{
                //    list.Add(h.Key + ": " + h.Value);
                //}
                var resContent = obj.Content.ReadAsStringAsync();
                resContent.Wait();
                if (!obj.IsSuccessStatusCode)
                {
                    throw new Exception(resContent.Result);
                }
                var token = JsonConvert.DeserializeObject<TokenModel>(resContent.Result);
                return token;
            }
        }
        // Test Auth and use auth to request another api (use get method)
        [Test]
        public void TestTokenByUserName()
        {
            var token = GetToken(new AccountModel {UserName = UserName, Password = Password});
            Assert.NotNull(token);
        }

        [Test]
        public void TestTokenByEmail()
        {
            var token = GetToken(new AccountModel { UserName = Email, Password = Password });
            Assert.NotNull(token);
        }

        //Test Auth and use auth to change password (use post method)
        [Test]
        public void TestPasswords()
        {
            var email = Email;
            var password = Password;
            var accountModel = new AccountModel { UserName = email, Password = NewPassword };
            var token = GetToken(accountModel);
            var pass = NewPassword;//"test@123B";//
            var client = GetClient();
            client.DefaultRequestHeaders.Add("Authorization", token.TokenType + " " + token.AccessToken);
            var changePassModel = new ChangePasswordModel
            {
                OldPassword = NewPassword,
                NewPassword = pass,
                ConfirmPassword = pass
            };
            var cRes = client.PostAsJsonAsync(routPrefix+"/ChangePassword", changePassModel).Result;
            var content = cRes.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.OK, cRes.StatusCode);

            // If password has been setted, will throw exception
            // {"Message":"请求无效。","ModelState":{"":["已为用户设置了密码。"]}}
            token = GetToken(new AccountModel {UserName = Email, Password = pass});
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", token.TokenType + " " + token.AccessToken);
            var setPassModel = new SetPasswordModel { NewPassword = password, ConfirmPassword = password };
            var sRes = client.PostAsJsonAsync(routPrefix+"/SetPassword", setPassModel).Result;
            var sContent = sRes.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, sRes.StatusCode);

        }

        [Test]
        public void TestResetPassword()
        {
            var email = Email;
            var newPassword = Password;
            var resetPassModel = new ResetPasswordModel { Email = email, NewPassword = newPassword };
            var client = GetClient();
            var resetRes = client.PostAsJsonAsync(routPrefix+"/ResetPassword", resetPassModel).Result;
            Assert.AreEqual(HttpStatusCode.OK, resetRes.StatusCode);
        }
    }
}
