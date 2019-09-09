using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Infrastructure;
using SotialNetwork.Api.Managers;
using SotialNetwork.Api.Models;

namespace SotialNetwork.Api.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenManager = context.OwinContext.Get<RefreshTokenManager>();
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            var userName = context.Ticket.Properties.Dictionary["userName"];
            var user = await userManager.FindByEmailAsync(userName);
            var refreshToken = new RefreshToken
            {
                UserEmail = userName,
                UserId = user.Id,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(Constants.RefreshTokenExpireTimeSpan)
            };
            context.Ticket.Properties.IssuedUtc = refreshToken.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = refreshToken.ExpiresUtc;
            refreshToken.ProtectedTicket = context.SerializeTicket();

            var newToken = await refreshTokenManager.AddRefreshToken(refreshToken);
            if (newToken != null)
            {
                context.SetToken(newToken.Id.ToString());
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var refreshTokenManager = context.OwinContext.Get<RefreshTokenManager>();

            var refreshTokenId = Guid.Parse(context.Token);
            var refreshToken = await refreshTokenManager.FindRefreshToken(refreshTokenId);

            if (refreshToken != null)
            {
                if (refreshToken.ExpiresUtc < DateTime.UtcNow)
                {
                    await refreshTokenManager.RevokeRefreshToken(refreshTokenId);
                    return;
                }

                context.DeserializeTicket(refreshToken.ProtectedTicket);
            }
        }
    }
}