using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightWorld_LED.Models
{
    public class CartModel
    {
        [Key]
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public ProductModel Product { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}