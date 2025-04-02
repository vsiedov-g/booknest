using System;
using System.ComponentModel.DataAnnotations;

namespace booknest.Models.DTO
{
    public class CallbackRequest
    {
        [Required]
        public string InvoiceId { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int Ccy { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
