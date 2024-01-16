using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.Entities
{
    public class Token : BaseEntity
    {
        public string? RefreshToken { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserCard? UserCard { get; set; }
    }
}
