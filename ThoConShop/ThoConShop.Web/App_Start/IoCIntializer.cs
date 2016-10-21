using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using ThoConShop.Business;

namespace ThoConShop.Web.App_Start
{
    public class IoCIntializer
    {
        public static void RegisteIoC()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            string connectionString = ConfigurationManager.ConnectionStrings["ShopThoConDb"].ToString();
            builder.RegisterModule(new BusinessModule(connectionString));

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}