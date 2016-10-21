using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThoConShop.DAL.Contracts;
using ThoConShop.DAL.Entities;

namespace ThoConShop.DAL.Repositories
{
    public class Repositories<TKey, TEntity> : 
                IRepositories<TKey, TEntity> where TEntity : BaseEntity<TKey> where TKey : struct
    {
        private readonly IShopConThoDbContext _dbContext; 

        public Repositories(IShopConThoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Create(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            var result = _dbContext.Set<TKey, TEntity>().Add(entity);
            return result;
        }

        public IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TKey, TEntity>().Where(expression);
        }

        public TEntity ReadOne(Expression<Func<TEntity, bool>> expression)
        {
            return _dbContext.Set<TKey, TEntity>().SingleOrDefault(expression);
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            _dbContext.Entry<TKey, TEntity>(entity).State = EntityState.Modified;

            return entity;
        }

        public bool Delete(TKey entityId)
        {
            var result = _dbContext.Set<TKey, TEntity>().Remove(ReadOne(a => a.Id.Equals(entityId)));

            return (result != null);
        }

        public int Delete(Expression<Func<TEntity, bool>> expression)
        {
            var result = Read(expression);
            foreach (var item in result)
            {
                _dbContext.Set<TKey, TEntity>().Remove(item);
            }

            return result.Count();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
