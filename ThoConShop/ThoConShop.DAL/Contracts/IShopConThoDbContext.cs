using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoConShop.DAL.Entities;

namespace ThoConShop.DAL.Contracts
{
    public interface IShopConThoDbContext
    {
        int SaveChanges();

        DbSet<TEntity> Set<TKey, TEntity>() where TEntity : BaseEntity<TKey> where TKey : struct ;

        DbEntityEntry<TEntity> Entry<TKey, TEntity>(TEntity entity) where TEntity : BaseEntity<TKey> where TKey : struct;
    }
}
