using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.Entities
{
    public class ProductReviews : BaseEntity
    {
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [Required]
        public Product Products { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [Required]
        public UserCard userCard { get; set; }

        public string ContentRate { get; set; }
        public string Content { get; set; }
        public int? PonitEvaluation { get; set; }
    }
}
