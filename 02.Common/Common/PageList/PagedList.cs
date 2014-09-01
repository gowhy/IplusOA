using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PagedList<T> : BasePagedList<T>
    {
        public PagedList(IEnumerable<T> source, int index, int pageSize)
            : this(source == null ? new List<T>().AsQueryable() : source.AsQueryable(), index, pageSize)
        {
        }

        private PagedList(IQueryable<T> source, int index, int pageSize)
            : base(index, pageSize, source.Count())
        {
            // add items to internal list
            if (TotalItemCount > 0)
                Subset.AddRange(index == 0
                    ? source.Take(pageSize).ToList()
                    : source.Skip((index) * pageSize).Take(pageSize).ToList()
                );
        }
    }
}
