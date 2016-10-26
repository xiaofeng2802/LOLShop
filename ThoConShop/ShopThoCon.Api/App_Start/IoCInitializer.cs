using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using ThoConShop.Business;

namespace ThoConShop.Api.App_Start
{
    public class IoCInitializer
    {
        public static void Init()
        {
            ContainerBuilder builder = new ContainerBuilder();

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

            //// Set the dependency resolver for MVC.
            //var mvcResolver = new AutofacDependencyResolver(container);
            //DependencyResolver.SetResolver(mvcResolver);
        }
    }
}