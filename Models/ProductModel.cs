using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrightWorld_LED.Models
{
    public class ProductModel
    {
        [Key]
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public Guid BrandId { get; set; }
        public BrandModel Brand { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string SecondaryImage { get; set; }
        public List<string> SecondaryImageList
        {
            get
            {
                if (!string.IsNullOrEmpty(SecondaryImage))
                {
                    return SecondaryImage.Split(',').ToList();
                }
                return new List<string>();
            }
        }
        public int Price { get; set; }

        
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public double Power { get; set; }
        public string LightColor { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}