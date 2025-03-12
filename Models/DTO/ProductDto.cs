using System;

namespace booknest.Models.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string[] Categories { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
