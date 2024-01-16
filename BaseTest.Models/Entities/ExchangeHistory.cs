using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.Entities
{
    public class ExchangeHistory : BaseEntity
    {
        public string UserId { get; set; }
        public string RewardId { get; set; }
        public int ExchangeAmount { get; set; }
    }
}
