using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace SotialNetwork.Api.Models
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, IEntity, IDeletable
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;
        private readonly bool isDeletable;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            dbSet = context.Set<TEntity>();
            isDeletable = typeof(IDeletable).IsAssignableFrom(typeof(TEntity));
        }

        public bool ItemExists(Guid id)
        {
            return dbSet.Any(x => x.Id == id);
        }

        public Task<TEntity> FindByIdAsync(Guid id)
        {
            return dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<TEntity> Get(bool includeDeleted)
        {
            if (isDeletable && !includeDeleted)
            {
                return dbSet.Where(r => !r.Deleted);
            }
            return dbSet;
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool includeDeleted)
        {
            return Get(includeDeleted).Where(predicate);
        }

        public Task<TEntity> FindByIdAsync(long id)
        {
            return dbSet.FindAsync(id);
        }

        public virtual TEntity Create(TEntity item)
        {
            return dbSet.Add(item);
        }

        public void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Patch(TEntity item, IEnumerable<string> modifiedProperties)
        {
            dbSet.Attach(item);
            var entry = context.Entry(item);
            entry.State = EntityState.Unchanged;
            foreach (string property in modifiedProperties)
            {
                entry.Property(property).IsModified = true;
            }

            context.SaveChanges();
        }

        public void AddOrUpdate(TEntity item, Expression<Func<TEntity, object>> idExpression,
            Expression<Func<TEntity, object>>[] modifyPropsExpression)
        {
            if (modifyPropsExpression != null && modifyPropsExpression.Any())
            {
                dbSet.Attach(item);
                modifyPropsExpression.ToList().ForEach(mpe => context.Entry(item).Property(mpe).IsModified = true);
                context.Entry(item).Property(e => e.Id).IsModified = false;
            }
            dbSet.AddOrUpdate(idExpression, item);
        }

        public void Remove(TEntity item)
        {
            if (item is IDeletable deletable)
            {
                deletable.Deleted = true;
                return;
            }

            dbSet.Remove(item);
        }

        public void Remove(Func<TEntity, bool> predicate)
        {
            context.Set<TEntity>().RemoveRange(context.Set<TEntity>().Where(predicate));
        }

        public IQueryable<TEntity> GetWithInclude(bool includeDeleted, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeDeleted, includeProperties);
        }

        public TEntity FindByIdWithInclude(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(true, includeProperties).SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<TEntity> GetWithInclude(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDeleted,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeDeleted, includeProperties);
            return query.Where(predicate);
        }

        private IQueryable<TEntity> Include(bool includeDeleted, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = Get(includeDeleted).AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}