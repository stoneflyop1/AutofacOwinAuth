using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutofacOwinAuth.WebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace AutofacOwinAuth.WebAPI.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            var user = User.Identity;
            var userName = user.GetUserName();
            var userId = user.GetUserId<int>();
            if (User.Identity.IsAuthenticated)
            {
                
            }
            return "value";
        }
        [AllowAnonymous]
        public bool Get([FromUri] string value1, string value2)
        {
            var values = Get();
            return values.Any(c => c == value1) && values.Any(c=>c==value2);
        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}
        [AllowAnonymous]
        public void Post([FromUri] string value1, bool save, [FromBody]string value2)
        {
            
        }
        [AllowAnonymous]
        public IHttpActionResult Post(DateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok();
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
