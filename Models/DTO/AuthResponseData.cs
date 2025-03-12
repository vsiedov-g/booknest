using System;

namespace booknest.Models.DTO
{
    public class AuthResponseData
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Role { get; set; }
    }
}
