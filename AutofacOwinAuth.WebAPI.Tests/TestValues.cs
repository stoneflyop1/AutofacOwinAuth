﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AutofacOwinAuth.WebAPI.Tests
{
    [TestFixture]
    public class TestValues
    {
        private string _routePrefix = "api/values";

        [Test]
        public void TestGetFromURL()
        {
            var client = TestAccounts.GetClient();
            var res = client.GetAsync(_routePrefix + "?value1=value1&value2=value2");
            var content = res.Result.Content.ReadAsStringAsync().Result;
            var has = JsonConvert.DeserializeObject<bool>(content);
            Assert.IsTrue(has);
        }
    }
}