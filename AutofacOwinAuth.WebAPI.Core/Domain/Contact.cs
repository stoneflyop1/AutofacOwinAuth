﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacOwinAuth.WebAPI.Core.Domain
{
    public class Contact : Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
