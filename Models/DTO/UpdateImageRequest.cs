using System;

namespace booknest.Models.DTO
{
    public class UpdateImageRequest
    {
        public int productId { get; set; }
        public IFormFile imageFile { get; set; }
    }
}
