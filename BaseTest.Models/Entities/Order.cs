using BaseTest.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.Entities
{
    public class Order : BaseEntity
    {
        public int PaymentId { get; set; }
        [ForeignKey(nameof(PaymentId))]
        [Required]
        public Payment Payment { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [Required]
        public UserCard userCard { get; set; }
        [Range(0,double.MaxValue,ErrorMessage =Constant.Order.ActualPriceErrorMessage)]  
        
        public double ActualPrice { get; set; }
        [Range(0,double.MaxValue,ErrorMessage =Constant.Order.OriginPriceErrorMessage)]
        public double OriginPrice { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public OrderStatus orderStatus { get; set; }
        public IEnumerable<OrderDetail> Details { get; set; }

    }
}
