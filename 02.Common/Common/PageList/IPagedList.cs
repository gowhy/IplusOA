using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IPagedList<T> : IPagedList, IEnumerable<T>
    {
        /// <summary>
        /// 通过索引获得指定元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T this[int index] { get; }

        /// <summary>
        /// 获得当前页索引下的元素的总数
        /// </summary>
        int Count { get; }
    }

    /// <summary>
    /// 带分页信息的集合
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        /// 集合下的总页数
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// 集合下的总条目数
        /// </summary>
        int TotalItemCount { get; }

        /// <summary>
        /// 当前的页索引--基于0开始的页索引
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 当前的页索引--基于1开始的页索引
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// 每一页的最大页尺寸
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// 当前是否是第一页
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// 当前是否是最后一页
        /// </summary>
        bool IsLastPage { get; }

        /// <summary>
        /// 在当前页索引页下，第一条的索引号，基于0开始
        /// </summary>
        int FirstIndexOnPage { get; }

        /// <summary>
        /// 在当前页索引页下，最后一条的索引号，基于0开始
        /// </summary>
        int LastIndexOnPage { get; }

        /// <summary>
        /// 在当前页索引页下，第一条的索引号，基于1开始
        /// </summary>
        int FirstItemOnPage { get; }

        /// <summary>
        /// 在当前页索引页下，最后一条的索引号，基于1开始
        /// </summary>
        int LastItemOnPage { get; }
    }
}
