using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.Businiss
{
    public class BaseApplyPaging
    {
        public static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, int pageSize, int pageNo, out int totalItem) where T : class
        {
            totalItem = query.Count();
            if (pageSize <= 0)
            {
                return query;
            }
            else
            {
                return query.Skip((pageNo - 1) * pageSize).Take(pageSize);
            }
        }

    }
}
