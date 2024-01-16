using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.Entities
{
    public class OrderStatus : BaseEntity
    {
        [Required]
        public string? Name { get; set; }
    }
}
