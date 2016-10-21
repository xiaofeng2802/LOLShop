using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ThoConShop.DAL;

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
            builder.RegisterModule(new DataModule(_connectionString));
            base.Load(builder);
        }
    }
}
