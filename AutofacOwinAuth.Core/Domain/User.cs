using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AutofacOwinAuth.Core.Domain
{
    public class User : Entity, IUser<int>
    {

        public new int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordSalt { get; set; }

        public string Password { get; set; }
    }
}
