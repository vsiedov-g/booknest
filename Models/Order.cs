using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace booknest.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]
        public User User { get; set; }
        public ICollection<Product> Products { get; set; }
        public double TotalPrice { get; set; }
        public DateTime date { get; set; }
        public string State { get; set; }
    }
}
