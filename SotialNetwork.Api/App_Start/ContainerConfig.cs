using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SotialNetwork.Api.Models;
using SotialNetwork.Api.Utilitis;

namespace SotialNetwork.Api.App_Start
{
    public static class ContainerConfig
    {
        public static void ConfigureContainer(this IAppBuilder app, HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(GenericRepository<>))
                .As(typeof(IGenericRepository<>))
                .InstancePerDependency();

            builder.RegisterType<ApplicationDbContext>().InstancePerRequest();
            builder.RegisterType<UnitOfWorkFactory>()
                .As<IUnitOfWorkFactory>()
                .InstancePerDependency();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacMiddleware(container);
        }
    }
}