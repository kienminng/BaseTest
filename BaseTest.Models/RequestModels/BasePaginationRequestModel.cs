using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Models.ReponseModels
{
    public class BasePaginationRequestModel
    {
        public int PageNo { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
