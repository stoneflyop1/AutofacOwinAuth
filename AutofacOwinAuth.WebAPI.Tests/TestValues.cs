using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http;

namespace AutofacOwinAuth.WebAPI.Tests
{
    [TestFixture]
    public class TestValues
    {
        private static string Host = "http://localhost:40772/";
        private string _routePrefix = "api/values";

        internal static HttpClient GetClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(Host)
            };
            return client;
        }

        [Test]
        public void TestGetFromURL()
        {
            var client = GetClient();
            var token = TestAccounts.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", token.TokenType + " " + token.AccessToken);
            var res = client.GetAsync(_routePrefix + "?value1=value1&value2=value2");
            var content = res.Result.Content.ReadAsStringAsync().Result;
            var has = JsonConvert.DeserializeObject<string>(content);
            Assert.NotNull(has);
        }



        [Test]
        public void TestPostFromURL()
        {
            var client = GetClient();
            var token = TestAccounts.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", token.TokenType + " " + token.AccessToken);
            var res = client.PostAsync(_routePrefix + "?value1=value1&save=true", "value2", new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            Assert.IsTrue(res.Result.IsSuccessStatusCode);
        }

        [Test]
        public void TestSetDate()
        {
            var client = GetClient();
            var token = TestAccounts.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", token.TokenType + " " + token.AccessToken);
            var res = client.PostAsJsonAsync(_routePrefix, new {Date = new DateTime()}).Result;
            var resContent = res.Content.ReadAsStringAsync().Result;
            Assert.IsTrue(!String.IsNullOrEmpty(resContent));
            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
            var model = JsonConvert.DeserializeObject<ErrorResponseModel>(resContent);
            Assert.NotNull(model);
        }
    }
}
