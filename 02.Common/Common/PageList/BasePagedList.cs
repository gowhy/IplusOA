using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class BasePagedList<T> : IPagedList<T>
    {
        protected readonly List<T> Subset = new List<T>();

        protected internal BasePagedList(int index, int pageSize, int totalItemCount)
        {

            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            PageIndex = index;
            if (TotalItemCount > 0)
            {
                PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            }
            else
            {
                PageCount = 0;
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, "PageIndex cannot be below 0.");
            }
        }

        #region Implementation IPageList<T>

        public T this[int index]
        {
            get { return Subset[index]; }
        }

        public int Count
        {
            get { return Subset.Count; }
        }

        public int PageCount
        {
            get;
            protected set;
        }

        public int TotalItemCount
        {
            get;
            protected set;
        }

        public int PageIndex
        {
            get;
            protected set;
        }

        public int PageNumber
        {
            get
            {
                return PageIndex + 1;
            }
        }

        public int PageSize
        {
            get;
            protected set;
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageIndex > 0;
            }
        }

        public bool HasNextPage
        {
            get { return PageIndex < (PageCount - 1); }
        }

        public bool IsFirstPage
        {
            get { return PageIndex <= 0; }
        }

        public bool IsLastPage
        {
            get { return PageIndex >= (PageCount - 1); }
        }


        public int FirstIndexOnPage
        {
            get { return (PageIndex * PageSize); }
        }

        public int LastIndexOnPage
        {
            get
            {
                var numberOfLastItemOnPage = (PageIndex * PageSize) + (PageSize - 1);
                if (numberOfLastItemOnPage > TotalItemCount)
                    numberOfLastItemOnPage = TotalItemCount - 1;
                return numberOfLastItemOnPage;
            }
        }

        public int FirstItemOnPage
        {
            get { return FirstIndexOnPage + 1; }
        }

        public int LastItemOnPage
        {
            get
            {
                return LastIndexOnPage + 1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Subset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
