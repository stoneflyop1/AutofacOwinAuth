using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutofacOwinAuth.Core.Domain;

namespace AutofacOwinAuth.Core.Data
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("TestUser");
            HasKey(c => c.Id);
            Property(c => c.UserName).IsRequired().HasMaxLength(255);
            Property(c => c.Email).IsRequired();
            Property(c => c.Password).IsRequired();
        }
    }
}
