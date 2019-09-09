using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api.Models
{
    public class NotificationToken : Entity
    {
        public string AccessToken { get; set; }

        public string Token { get; set; }

        [Index]
        public Guid UserId { get; set; }
    }
}