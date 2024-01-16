using BaseTest.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.ReponseModels.UserCard
{
    public class GetPageUserInput : BasePaginationRequestModel
    {
        public string  role { get; set; }
        public string username { get; set; } = string.Empty;
        public string phonenumber { get; set; } = string.Empty;
        public string email { get; set; } =string.Empty;
    }
}
