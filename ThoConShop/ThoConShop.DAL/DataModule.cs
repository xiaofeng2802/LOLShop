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

            builder.RegisterType<Repositories.Repositories<int, Account>>().As<IRepositories<int, Account>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, Rank>>().As<IRepositories<int, Rank>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, User>>().As<IRepositories<int, User>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, Champion>>().As<IRepositories<int, Champion>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, PageGem>>().As<IRepositories<int, PageGem>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, Skin>>().As<IRepositories<int, Skin>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, UserRechargeHistory>>().As<IRepositories<int, UserRechargeHistory>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, UserTradingHistory>>().As<IRepositories<int, UserTradingHistory>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, LuckyWheelItem>>().As<IRepositories<int, LuckyWheelItem>>().InstancePerRequest();
            builder.RegisterType<Repositories.Repositories<int, LuckyWheelHistory>>().As<IRepositories<int, LuckyWheelHistory>>().InstancePerRequest();

            base.Load(builder);
        }
    }
}
