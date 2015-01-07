using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutofacOwinAuth.Core.Data;
using AutofacOwinAuth.Core.Domain;
using AutofacOwinAuth.Core.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace AutofacOwinAuth.WebAPI
{
    public class AutofacConfig
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
            builder.RegisterType<EntityContext>().As<IDbContext>().InstancePerLifetimeScope();

            // Register repositories by using Autofac's OpenGenerics feature
            // More info: http://code.google.com/p/autofac/wiki/OpenGenerics
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            
            //Services
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<User, int>>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().As<UserManager<User, int>>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationSignInManager>().As<SignInManager<User, int>>().InstancePerLifetimeScope();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication);
        }
    }
}