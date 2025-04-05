using System;
using System.Security.Claims;
using booknest.Models;

namespace booknest.Service.IService
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public RefreshToken GenerateRefreshToken(int id);
        public Task SaveRefreshTokenToDBAsync(RefreshToken refreshToken);
        public Task<bool> ValidateRefreshTokenAsync(string token, int userId);
        public Task RevokeRefreshTokenAsync(int userId);
    }
}
