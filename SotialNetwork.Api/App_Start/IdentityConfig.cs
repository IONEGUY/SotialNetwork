using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using SotialNetwork.Api.Models;

namespace SotialNetwork.Api
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        private readonly IUserStore<ApplicationUser, Guid> store;

        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store, IDataProtectionProvider provider)
            : base(store)
        {
            this.store = store;
            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, Guid>(provider.Create("Confirmations"));

            UserValidator = new UserValidator<ApplicationUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        /// <summary>
        /// Creates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">context</exception>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<ApplicationDbContext>()),
                options.DataProtectionProvider);

            return manager;
        }
    }
}
