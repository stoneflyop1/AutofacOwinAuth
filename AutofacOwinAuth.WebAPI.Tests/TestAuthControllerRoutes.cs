using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AecCloud.WebAPI.Tests;
using AutofacOwinAuth.AuthorizationServer.Controllers;
using NUnit.Framework;

namespace AutofacOwinAuth.WebAPI.Tests
{
    public class TestAuthControllerRoutes
    {
        [Test]
        public void TestResetPassword()
        {
            var dummyUrl = "http://test.com/api/Auth/ResetPassword";
            var ok = dummyUrl.ShouldMapTo<AuthController>("ResetPassword", HttpMethod.Post);
            Assert.AreEqual(true, ok);
        }

        [Test]
        public void TestSetPassword()
        {
            var dummyUrl = "http://test.com/api/Auth/SetPassword";
            var ok = dummyUrl.ShouldMapTo<AuthController>("SetPassword", HttpMethod.Post);
            Assert.AreEqual(true, ok);
        }

        [Test]
        public void TestChangePassword()
        {
            var dummyUrl = "http://test.com/api/Auth/ChangePassword";
            var ok = dummyUrl.ShouldMapTo<AuthController>("ChangePassword", HttpMethod.Post);
            Assert.AreEqual(true, ok);
        }
    }
}
