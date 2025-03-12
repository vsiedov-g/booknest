using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace booknest.Models
{
    public class Product
    {
        public int Id { get; set;}
        public string Title { get; set;}
        public string Description { get; set; }
        public ICollection<Category> Categories { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [ValidateNever]
        public Author Author { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? FilePath { get; set; }
        public ICollection<User>? Users { get; set; }

    }
}
