using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public int? Number_Of_View { get; set; }    
        public double? PointAvg { get; set; }
        public List<ProductReviews> Reviews { get; set;}
        public List<OrderDetail> Details { get; set;}
    }
}
