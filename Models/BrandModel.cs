using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightWorld_LED.Models
{
    public class BrandModel
    {
        [Key]
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImage { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProductModel> Products { get; set; }
    }
}