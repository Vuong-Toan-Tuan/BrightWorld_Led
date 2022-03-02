using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightWorld_LED.Models
{
    public class OrderModel
    {
        [Key]
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public double TotalPayment { get; set; }
        public string Transaction { get; set; }
        public orderstatus OrderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<OrderDetailModel> OrderDetail { get; set; }
    }
    
    public enum orderstatus
    {
        Submitted = 1, Confirmed = 2, Delivered = 3, Cancelled = 4
    }
}