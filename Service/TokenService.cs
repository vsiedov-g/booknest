using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using booknest.Models;
using booknest.Repository.IRepository;
using booknest.Service.IService;
using Microsoft.IdentityModel.Tokens;

namespace booknest.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IConfiguration config, IUnitOfWork unitOfWork)
        {
            _config = config;
            _unitOfWork = unitOfWork;
        }
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new [] { 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
                };

            var token = new JwtSecurityToken(_config["JwtSettings:Issuer"], 
            _config["JwtSettings:Audience"], claims,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters parameters = new TokenValidationParameters
            {
                ValidIssuer= _config["JwtSettings:Issuer"],
                ValidAudience= _config["JwtSettings:Audience"],
                IssuerSigningKey= new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ValidateToken(token, parameters, out _);
            return claims;
        }

        public RefreshToken GenerateRefreshToken(int id)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(7),
                UserId = id,
                Revoked = false
            };

            return refreshToken;
        }
        public async Task SaveRefreshTokenToDBAsync(RefreshToken refreshToken)
        {
            _unitOfWork.RefreshToken.Add(refreshToken);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ValidateRefreshTokenAsync(string token, int userId)
        {
            RefreshToken refreshToken = await _unitOfWork.RefreshToken.GetAsync(r => r.UserId == userId && !r.Revoked);
            if (refreshToken == null)
            {
                return false;
            }
            if (token != refreshToken.Token || DateTime.Now > refreshToken.Expires)
            {
                return false;
            }
            return true;
        }

        public async Task RevokeRefreshTokenAsync(int userId)
        {
            RefreshToken refreshToken = await _unitOfWork.RefreshToken.GetAsync(r => r.UserId == userId && !r.Revoked);
            if (refreshToken != null)
            {
                refreshToken.Revoked = true;
                _unitOfWork.RefreshToken.Update(refreshToken);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
