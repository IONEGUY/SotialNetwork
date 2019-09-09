using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace SotialNetwork.Api.Models
{
    public interface IGenericRepository<TEntity> : IDisposable
        where TEntity : class, IEntity, IDeletable
    {
        TEntity Create(TEntity item);

        bool ItemExists(Guid id);

        Task<TEntity> FindByIdAsync(Guid id);

        TEntity FindByIdWithInclude(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> Get(bool includeDeleted = false);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false);

        void Remove(TEntity item);

        void Remove(Func<TEntity, bool> predicate);

        void Update(TEntity item);

        void Patch(TEntity item, IEnumerable<string> modifiedProperties);

        void AddOrUpdate(TEntity item, Expression<Func<TEntity, object>> idExpression, Expression<Func<TEntity, object>>[] modifyPropsExpression = null);

        IQueryable<TEntity> GetWithInclude(bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, bool includeDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}