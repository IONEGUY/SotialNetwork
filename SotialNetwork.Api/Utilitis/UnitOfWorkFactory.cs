using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SotialNetwork.Api.Models;

namespace SotialNetwork.Api.Utilitis
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFactory" /> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UnitOfWorkFactory(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates a new instance of <c>IUnitOfWork</c>.
        /// </summary>
        /// <returns>Returns a new instance of <c>IUnitOfWork</c>.</returns>
        public IUnitOfWork Create()
        {
            return new UnitOfWork(context);
        }
    }
}