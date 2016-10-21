using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

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
            builder.RegisterInstance(typeof(IShopConThoDbContext)).As(new ShopThoCon(_connection).GetType());
            base.Load(builder);
        }
    }
}
