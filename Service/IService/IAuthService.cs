using System;
using booknest.Models;

namespace booknest.Service.IService
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPasswordr);
        public Task<User> AuthenticateAsync(string email, string password);
        public Task<User> CreateUserAsync(string email, string password);

    }
}
