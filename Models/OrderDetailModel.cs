using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightWorld_LED.Models
{
    public class OrderDetailModel
    {
        [Key]
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; }
        public Guid ProductId { get; set; }
        public ProductModel Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}