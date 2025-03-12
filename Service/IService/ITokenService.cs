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
        public void SaveRefreshTokenToDB(RefreshToken refreshToken);
        public bool ValidateRefreshToken(string token, int userId);
        public void RevokeRefreshToken(int userId);
    }
}
