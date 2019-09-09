using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SotialNetwork.Api.Models;

namespace SotialNetwork.Api.Utilitis
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Commits all made changes.
        /// </summary>
        public void Commit()
        {
            context.SaveChanges();
        }

        public void SetChangeTracking(bool enable)
        {
            context.Configuration.AutoDetectChangesEnabled = enable;
        }

        public void DetachEntities<TEntity>() where TEntity : class
        {
            foreach (DbEntityEntry<TEntity> dbEntityEntry in context.ChangeTracker.Entries<TEntity>())
            {
                dbEntityEntry.State = EntityState.Detached;
            }
        }

        /// <summary>
        /// The Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles cleaning up the object instance.
        /// </summary>
        /// <param name="disposing">Indicates whether or not the class is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            Commit();
        }
    }
}