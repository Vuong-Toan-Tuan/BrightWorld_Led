using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightWorld_LED.Models
{
    public class CategoryModel
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<ProductModel> Products { get; set; }
    }
}