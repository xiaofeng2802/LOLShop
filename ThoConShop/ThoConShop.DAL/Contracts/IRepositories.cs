using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ThoConShop.DAL.Entities;

namespace ThoConShop.DAL.Contracts
{
    public interface IRepositories<TKey, TEntity> 
    {
        TEntity Create(TEntity entity);

        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression, bool tracking = false);

        TEntity ReadOne(Expression<Func<TEntity, bool>> expression);

        TEntity Update(TEntity entity);

        int Delete(Expression<Func<TEntity, bool>> expression);

        int SaveChanges();

    }
}
