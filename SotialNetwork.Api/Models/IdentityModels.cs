using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace SotialNetwork.Api.Models
{
    public class ApplicationUser : IdentityUser<Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IEntity, IDeletable
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }

        public string Firstname { get; set; }

        public string LastName { get; set; }

        [MaxLength(50)]
        public override string PhoneNumber { get; set; }

        public bool Deleted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime AccountCreationDate { get; set; } = DateTime.Now;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager, string authenticationType)
        {
            return await manager.CreateIdentityAsync(this, authenticationType);
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,
        Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}