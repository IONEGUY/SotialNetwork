using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api.Models
{
    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>, IEntity
    {
        public ApplicationRole() { }

        public ApplicationRole(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
    }
}