using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SotialNetwork.Api.Models;

namespace SotialNetwork.Api.Managers
{
    public interface IRefreshTokenManager : IDisposable
    {
        Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);

        Task RevokeRefreshToken(Guid refreshTokenId);

        Task RevokeRefreshTokenForUser(string clientEmail);

        Task<RefreshToken> FindRefreshToken(Guid refreshTokenId);
    }
}