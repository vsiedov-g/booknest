using System;
using booknest.Models;

namespace booknest.Service.IService
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPasswordr);
        public User Authenticate(string email, string password);
        public User CreateUser(string email, string password);

    }
}
