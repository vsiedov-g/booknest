using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace booknest.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Status { get; set; }
        public string? InvoiceId { get; set; }
    }
}
