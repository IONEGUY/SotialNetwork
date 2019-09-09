using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SotialNetwork.Api.Models;

namespace SotialNetwork.Api.Managers
{
    public class RefreshTokenManager : IRefreshTokenManager
    {
        private readonly ApplicationDbContext dbContext;

        public RefreshTokenManager(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public static RefreshTokenManager Create(
            IdentityFactoryOptions<RefreshTokenManager> options,
            IOwinContext context)
        {
            return new RefreshTokenManager(context.Get<ApplicationDbContext>());
        }

        public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
        {
            var existingToken = dbContext.RefreshTokens
                .SingleOrDefault(r => r.UserId == refreshToken.UserId);
            if (existingToken != null)
            {
                await UpdateExistingToken(refreshToken, existingToken).ConfigureAwait(false);
                return existingToken;
            }
            dbContext.RefreshTokens.Add(refreshToken);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
            return dbContext.RefreshTokens
                .SingleOrDefault(r => r.UserId == refreshToken.UserId);
        }

        private Task UpdateExistingToken(RefreshToken refreshToken, RefreshToken existingToken)
        {
            existingToken.IssuedUtc = refreshToken.IssuedUtc;
            existingToken.ExpiresUtc = refreshToken.ExpiresUtc;
            existingToken.ProtectedTicket = refreshToken.ProtectedTicket;
            dbContext.Entry(existingToken).State = EntityState.Modified;
            return dbContext.SaveChangesAsync();
        }

        public Task RevokeRefreshToken(Guid refreshTokenId)
        {
            var existingToken = dbContext.RefreshTokens
                .SingleOrDefault(r => r.Id == refreshTokenId);
            if (existingToken != null)
            {
                dbContext.RefreshTokens.Remove(existingToken);
                return dbContext.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }

        public Task RevokeRefreshTokenForUser(string clientEmail)
        {
            var existingToken = dbContext.RefreshTokens
                .SingleOrDefault(r => r.UserEmail == clientEmail);
            if (existingToken != null)
            {
                dbContext.RefreshTokens.Remove(existingToken);
                dbContext.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }

        public Task<RefreshToken> FindRefreshToken(Guid refreshTokenId)
        {
            return dbContext.RefreshTokens.FindAsync(refreshTokenId);
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }
    }
}