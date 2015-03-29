using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AutofacOwinAuth.WebAPI.Core.Domain;

namespace AutofacOwinAuth.WebAPI.Core.Data
{
    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            ToTable("Contacts");
            HasKey(c => c.Id);
            Property(c => c.Name).IsRequired();
            Property(c => c.Email).IsRequired();
        }
    }
}
