using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QuickKartServices.Models
{
    public class Products
    {
        public string ProductId { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(100)]
        public string ProductName { get; set; }
        [Required]
        [Range(1,24)]
        public byte? CategoryId { get; set; }
        [Required]
        [Range(minimum: 1, maximum: Double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range(minimum: 0, maximum: int.MaxValue)]
        public int QuantityAvailable { get; set; }
    }
}
