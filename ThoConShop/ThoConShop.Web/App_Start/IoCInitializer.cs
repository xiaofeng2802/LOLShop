using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThoConShop.Business;

namespace ThoConShop.Web.App_Start
{
    public class IoCInitializer
    {
        public static void Init()
        {
            ContainerBuilder builder = new ContainerBuilder();

            // Get your HttpConfiguration.

            string connectionName = "ShopThoConDb";
            builder.RegisterModule(new BusinessModule(connectionName));

            // Register your Web API controllers.
            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //// Set the dependency resolver for MVC.
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
        }
    }
}