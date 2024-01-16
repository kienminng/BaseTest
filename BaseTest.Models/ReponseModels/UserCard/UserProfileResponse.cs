using BaseTest.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.ReponseModels.UserCard
{
    public class UserProfileResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Roles Roles { get; set; }
        public int Amount { get; set; }
        public DateTime createDate { get; set; }
    }
}
