using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;
using ShopThoCon = ThoConShop.DAL.Entities;

namespace ThoConShop.DAL
{
    public class DataModule : Module
    {
        private readonly string _connection;
        public DataModule(string connectionString)
        {
            _connection = connectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ShopThoCon.ShopThoCon(_connection)).As<IShopConThoDbContext>().SingleInstance();

            builder.RegisterType<Repositories.Repositories<int, Account>>().As<IRepositories<int, Account>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, Rank>>().As<IRepositories<int, Rank>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, User>>().As<IRepositories<int, User>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, Champion>>().As<IRepositories<int, Champion>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, PageGem>>().As<IRepositories<int, PageGem>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, Skin>>().As<IRepositories<int, Skin>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, UserRechargeHistory>>().As<IRepositories<int, UserRechargeHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, UserTradingHistory>>().As<IRepositories<int, UserTradingHistory>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, LuckyWheelItem>>().As<IRepositories<int, LuckyWheelItem>>().InstancePerLifetimeScope();
            builder.RegisterType<Repositories.Repositories<int, LuckyWheelHistory>>().As<IRepositories<int, LuckyWheelHistory>>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
