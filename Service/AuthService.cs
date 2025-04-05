using System;
using System.Security.Cryptography;
using booknest.Models;
using booknest.Models.DTO;
using booknest.Repository.IRepository;
using booknest.Service.IService;
using booknest.Utility;


namespace booknest.Service
{
    public class AuthService : IAuthService
    {

        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var currentUser = await _unitOfWork.User.GetAsync(x => x.Email.ToLower() == email.ToLower());
            if(currentUser == null)
            {
                return null;
            }
            if (!VerifyPassword(password, currentUser.Password))
            {
                return null;
            }
            return currentUser;
        }

        public async Task<User> CreateUserAsync(string email, string password)
        {
            var user = new User {
                Email = email,
                Password = HashPassword(password),
                Role = SD.Customer_Claim
            };
            _unitOfWork.User.Add(user);
            await _unitOfWork.SaveAsync();
            return user;
        }
    }
}
