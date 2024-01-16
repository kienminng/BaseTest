using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.ReponseModels.NewFolder
{
    public class CreateEditTransaction
    {
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public int CardAmount { get; set; }
    }
}
