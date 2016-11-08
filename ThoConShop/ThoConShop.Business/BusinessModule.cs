using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ThoConShop.DAL;
using ThoConShop.Business.Contracts;
using ThoConShop.Business.Services;

namespace ThoConShop.Business
{
    public class BusinessModule : Module
    {
        private readonly string _connectionString;

        public BusinessModule(string con)
        {
            _connectionString = con;
        }

        protected override void Load(ContainerBuilder builder)
        {
            MappingConfig.RegisteMapper();

            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerRequest();
            builder.RegisterType<AccountRelationDataService>().As<IAccountRelationDataService>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<HistoryService>().As<IHistoryService>().InstancePerRequest();

            builder.RegisterModule(new DataModule(_connectionString));
            base.Load(builder);
        }
    }
}
