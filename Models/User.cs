using System.ComponentModel.DataAnnotations;

namespace booknest.Models;

public class User
{
    [Required]
    public int Id { get; set; }
    public string? Name { get; set; }  
    [DataType(DataType.PhoneNumber)]
    public string? MobilePhone { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Role { get; set; }
    public ICollection<RefreshToken> RefreshToken { get; set; }
    public ICollection<Product> Products { get; set; }
    public ICollection<Order> Orders { get; set; }
}
