using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api.Utilitis
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all made changes.
        /// </summary>
        void Commit();

        /// <summary>
        /// Sets the change tracking state.
        /// </summary>
        /// <param name="enable">State of change tracking detecting.</param>
        void SetChangeTracking(bool enable);

        void DetachEntities<TEntity>() where TEntity : class;
    }
}