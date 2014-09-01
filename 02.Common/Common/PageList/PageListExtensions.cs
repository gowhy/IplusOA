using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class PageListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize) where T : class
        {
            var pageList = new PagedList<T>(source, index, pageSize);
            return pageList;
        }
    }
}
