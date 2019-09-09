using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api.Models
{
    public interface IDeletable
    {
        bool Deleted { get; set; }
    }
}