using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.Entities
{
    public class UserCard : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }   
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Roles Roles { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; }
        public Token? Token { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpiresDate { get; set; }

        public IEnumerable<ProductReviews>? Reviews { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
