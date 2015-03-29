using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Http;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.WebApi;
using AutofacOwinAuth.WebAPI.Core.Data;
using AutofacOwinAuth.WebAPI.Core.Services;

namespace AutofacOwinAuth.WebAPI.Core.Configurations
{
    public static class AutofacConfig
    {
        public static IContainer Container = null;

        public static void Initialize(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            RegisterTypes(builder);

            Container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }

        // to make sure registered types using autofac can be used in owin context,
        // types' instances should be created per lifetimescope, not per request
        private static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // EF DbContext
            builder.RegisterType<EfContext>().As<DbContext>().InstancePerRequest();

            // Register repositories by using Autofac's OpenGenerics feature
            // More info: http://code.google.com/p/autofac/wiki/OpenGenerics
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            //Services
            builder.RegisterType<ContactService>().As<IContactService>().InstancePerRequest();
           // builder.RegisterType<ApplicationSignInManager>().As<SignInManager<User, int>>().InstancePerLifetimeScope();
            //builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication);
        }
    }
}
