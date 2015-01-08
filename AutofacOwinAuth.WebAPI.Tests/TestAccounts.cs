using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Thinktecture.IdentityModel.Client;

namespace AutofacOwinAuth.WebAPI.Tests
{
    [TestFixture]
    public class TestAccounts
    {
        private static string Host = "http://localhost:40772/";

        private static string routPrefix = "api/Account";

        private static string Email = "test1@123.com";
        private static string Password = "test@123B";//"test@123A"; //


        private static HttpClient GetClient()
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
            var model = new RegisterBindingModel {Email = Email, Password = Password, ConfirmPassword = Password};
            var res = client.PostAsJsonAsync(routPrefix + "/Register", model);
            res.Wait();
            Assert.AreEqual(HttpStatusCode.OK, res.Result.StatusCode);
        }


        [Test]
        public void TestToken0()
        {
            var client = new OAuth2Client(new Uri(Host+"Token"));

            var res = client.RequestResourceOwnerPasswordAsync(Email, Password);
            res.Wait();
            var obj = res.Result;
            
            Assert.AreEqual(false, obj.IsError);
        }

        private TokenModel GetToken(AccountModel model)
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
                var token = JsonConvert.DeserializeObject<TokenModel>(resContent.Result);
                return token;
            }
        }
        // Test Auth and use auth to request another api (use get method)
        [Test]
        public void TestToken()
        {
            var token = GetToken(new AccountModel {UserName = Email, Password = Password});
            var client = GetClient();
            client.DefaultRequestHeaders.Add("Authorization", token.token_type + " " + token.access_token);

            var vRes = client.GetAsync("/values/Get/1");
            vRes.Wait();
            var content = JsonConvert.DeserializeObject(vRes.Result.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(HttpStatusCode.OK, vRes.Result.StatusCode);
        }

        //Test Auth and use auth to change password (use post method)
        [Test]
        public void TestPasswords()
        {
            var accountModel = new AccountModel {UserName = Email, Password = Password};
            var token = GetToken(accountModel);
            var pass = "test@123A";//"test@123B";//
            var client = GetClient();
            client.DefaultRequestHeaders.Add("Authorization", token.token_type + " " + token.access_token);
            var changePassModel = new ChangePasswordModel
            {
                OldPassword = Password,
                NewPassword = pass,
                ConfirmPassword = pass
            };
            var cRes = client.PostAsJsonAsync("/api/Account/ChangePassword", changePassModel);
            cRes.Wait();
            var content = cRes.Result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.OK, cRes.Result.StatusCode);

            // If password has been setted, will throw exception
            // {"Message":"请求无效。","ModelState":{"":["已为用户设置了密码。"]}}
            token = GetToken(new AccountModel {UserName = Email, Password = pass});
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", token.token_type + " " + token.access_token);
            var setPassModel = new SetPasswordModel {NewPassword = Password, ConfirmPassword = Password};
            var sRes = client.PostAsJsonAsync("/api/Account/SetPassword", setPassModel);
            sRes.Wait();
            var sContent = sRes.Result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.OK, sRes.Result.StatusCode);

        }
    }
}
