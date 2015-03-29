using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AutofacOwinAuth.WebAPI.Core.Data
{
    public class EfContext : DbContext
    {
        public EfContext() : this("OwinContacts") { }

        public EfContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContactMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
