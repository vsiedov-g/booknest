using System;

namespace booknest.Models.DTO
{
    public class ProductDtoWithImage
    {
        public string productDto { get; set; }
        public IFormFile? imageFile { get; set; }
    }
}
