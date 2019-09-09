using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SotialNetwork.Api.Models
{
    public class RefreshToken : Entity
    {
        public Guid UserId { get; set; }

        public string UserEmail { get; set; }

        public DateTime IssuedUtc { get; set; }

        public DateTime ExpiresUtc { get; set; }

        [Required]
        public string ProtectedTicket { get; set; }
    }
}