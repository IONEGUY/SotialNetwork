using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using SotialNetwork.Api.Models;
using SotialNetwork.Api.Utilitis;

namespace SotialNetwork.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager userManager;
        private readonly IGenericRepository<NotificationToken> tokenRepository;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            IUnitOfWorkFactory unitOfWorkFactory,
            IGenericRepository<NotificationToken> tokenRepository)
        {
            UserManager = userManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.tokenRepository = tokenRepository;
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register([FromBody]RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ErrorCode", ((int)ErrorCode.ValidationError).ToString());
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Firstname = model.FirstName,
                LastName = model.LastName
            };

            var existingUser = await UserManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("ErrorCode", ((int)ErrorCode.DuplicateUser).ToString());
                return BadRequest(ModelState);
            }

            IdentityResult result = UserManager.Create(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdateAccessToken")]
        public IHttpActionResult UpdateAccessToken(string oldAccessToken)
        {
            using (unitOfWorkFactory.Create())
            {
                var token = tokenRepository
                .Get(t => t.UserId.ToString() == User.Identity.GetUserId()
                    && t.AccessToken == oldAccessToken)
                .SingleOrDefault();

                token.AccessToken = GetAccessToken();

                if (token != null)
                {
                    tokenRepository.Update(token);
                }
            }

            return Ok();
        }

        [HttpPost]
        [Route("UpdatePushNotificationToken")]
        public IHttpActionResult UpdatePushNotificationToken(string fcmToken)
        {
            if (!Guid.TryParse(User.Identity.GetUserId(), out var userId))
            {
                return Unauthorized();
            }

            string accessToken = GetAccessToken();

            using (unitOfWorkFactory.Create())
            {
                var token = tokenRepository
                .Get(t => t.UserId == userId && t.AccessToken == accessToken)
                .SingleOrDefault()
                    ?? new NotificationToken
                    {
                        AccessToken = accessToken,
                        Token = fcmToken,
                        UserId = userId
                    };

                tokenRepository.AddOrUpdate(token, t => t.Id);
            }

            return Ok();
        }

        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            if (!Guid.TryParse(User.Identity.GetUserId(), out var userId))
            {
                return Unauthorized();
            }

            string accessToken = GetAccessToken();

            using (unitOfWorkFactory.Create())
            {
                var token = tokenRepository
                    .Get(t => t.UserId == userId && t.AccessToken == accessToken)
                    .SingleOrDefault();

                if (token != null)
                {
                    tokenRepository.Remove(token);
                }
            }

            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }

            base.Dispose(disposing);
        }

        private string GetAccessToken()
        {
            const string bearer = "Bearer";
            string value = Request.Headers.Authorization.Parameter.Trim();
            if (value.ToLowerInvariant().StartsWith(bearer))
            {
                value = value.Substring(bearer.Length);
            }
            return value.Trim();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
