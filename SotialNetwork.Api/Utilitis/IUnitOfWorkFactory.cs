using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api.Utilitis
{
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Creates a new instance of <c>IUnitOfWork</c>.
        /// </summary>
        /// <returns>Returns a new instance of <c>IUnitOfWork</c>.</returns>
        IUnitOfWork Create();
    }
}