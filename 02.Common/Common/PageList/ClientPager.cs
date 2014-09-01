using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common
{
    public class ClientPager : IHtmlString
    {
        private readonly IPagedList _pagination;
        private readonly ViewContext _viewContext;
        private string _pageQueryName = "page";

        private string _paginationFirst = "首页";
        private string _paginationLast = "末页";
        private string _paginationNext = "下一页";
        private string _paginationPrev = "上一页";
        private Func<int, string> _urlBuilder;

        /// <summary>
        /// Creates a new instance of the Pager class.
        /// </summary>
        /// <param name="pagination">The IPagination datasource</param>
        /// <param name="context">The view context</param>
        public ClientPager(IPagedList pagination, ViewContext context)
        {
            _pagination = pagination;
            _viewContext = context;

            _urlBuilder = CreateDefaultUrl;
        }

        protected ViewContext ViewContext { get { return _viewContext; } }

        #region IHtmlString Members

        public string ToHtmlString()
        {
            if (_pagination.TotalItemCount == 0)
            {
                return null;
            }

            var builder = new StringBuilder();

            builder.Append("<div class=\"ui-paging\">");
            RenderPager(builder);
            builder.Append(@"</div>");

            return builder.ToString();
        }

        #endregion

        /// <summary>
        /// Specifies the query string parameter to use when generating pager links. The default is 'page'
        /// </summary>
        public ClientPager QueryParam(string queryStringParam)
        {
            _pageQueryName = queryStringParam;
            return this;
        }

        /// <summary>
        /// Specifies the format to use when rendering a pagination containing a single page. 
        /// The default is 'Showing {0} of {1}' (eg 'Showing 1 of 3')
        /// </summary>
        public ClientPager SingleFormat(string format)
        {
            return this;
        }

        /// <summary>
        /// Specifies the format to use when rendering a pagination containing multiple pages. 
        /// The default is 'Showing {0} - {1} of {2}' (eg 'Showing 1 to 3 of 6')
        /// </summary>
        public ClientPager Format(string format)
        {
            return this;
        }

        /// <summary>
        /// Text for the 'first' link.
        /// </summary>
        public ClientPager First(string first)
        {
            _paginationFirst = first;
            return this;
        }

        /// <summary>
        /// Text for the 'prev' link
        /// </summary>
        public ClientPager Previous(string previous)
        {
            _paginationPrev = previous;
            return this;
        }

        /// <summary>
        /// Text for the 'next' link
        /// </summary>
        public ClientPager Next(string next)
        {
            _paginationNext = next;
            return this;
        }

        /// <summary>
        /// Text for the 'last' link
        /// </summary>
        public ClientPager Last(string last)
        {
            _paginationLast = last;
            return this;
        }

        /// <summary>
        /// Uses a lambda expression to generate the URL for the page links.
        /// </summary>
        /// <param name="urlBuilder">Lambda expression for generating the URL used in the page links</param>
        public ClientPager Link(Func<int, string> urlBuilder)
        {
            _urlBuilder = urlBuilder;
            return this;
        }

        // For backwards compatibility with WebFormViewEngine
        public override string ToString()
        {
            return ToHtmlString();
        }



        protected virtual void RenderPager(StringBuilder builder)
        {
            if (_pagination.PageNumber > 1)
            {
                builder.Append(CreatePageLink(1, _paginationFirst, "&#xf0292;&#xf0292;", string.Empty, "ui-paging-prev"));
                builder.Append(CreatePageLink(_pagination.PageNumber - 1, _paginationPrev, "&#xf0292;", string.Empty, "ui-paging-prev"));
            }
            else
            {
                builder.Append(CreatePageLink(1, _paginationPrev, "&#xf0292;", "#", "ui-paging-prev"));
            }
            const int splitSize = 5;
            var currentSpit = (int)Math.Ceiling((double)_pagination.PageNumber / splitSize);
            var splitBegin = (currentSpit - 1) * 5 + 1;
            var spitEnd = currentSpit * 5;
            for (var i = splitBegin; i <= spitEnd; i++)
            {
                if (i >= _pagination.PageCount + 1)
                {
                    break;
                }
                if (i == _pagination.PageNumber)
                {
                    builder.Append(CreatePageLink(i, i.ToString(), string.Empty, "#", "ui-paging-item ui-paging-current"));
                    continue;
                }
                builder.Append(CreatePageLink(i, i.ToString(), string.Empty, string.Empty, "ui-paging-item"));
            }
            int lastPage = _pagination.PageCount;
            if (_pagination.PageNumber < lastPage)
            {
                builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext, "&#xf02af;", string.Empty, "ui-paging-next"));
                builder.Append(CreatePageLink(lastPage, _paginationLast, "&#xf02af;&#xf02af;", string.Empty, "ui-paging-next"));
            }
            else
            {
                builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext, "&#xf02af;", "#", "ui-paging-next"));
            }
        }



        private string CreatePageLink(int pageNumber, string text, string split, string url, string @class)
        {
            var builder = new TagBuilder("a");
            if (!string.IsNullOrEmpty(split))
            {
                if (split.IndexOf("&#xf02af;", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    builder.InnerHtml = text + "<i class=\"iconfont\" title=\"下一页\">" + split + "</i>";
                }
                else
                {
                    builder.InnerHtml = "<i class=\"iconfont\" title=\"下一页\">" + split + "</i>" + text;
                }

            }
            else
            {
                builder.SetInnerText(text);
            }
            builder.MergeAttribute("class", @class);
            builder.MergeAttribute("href", string.IsNullOrEmpty(url) ? _urlBuilder(pageNumber) : url);
            return builder.ToString(TagRenderMode.Normal);
        }
        private string CreateDefaultUrl(int pageNumber)
        {
            var routeValues = new RouteValueDictionary();

            foreach (
                string key in
                    _viewContext.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
            {
                routeValues[key] = _viewContext.RequestContext.HttpContext.Request.QueryString[key];
            }
            foreach (object key in _viewContext.RequestContext.HttpContext.Request.Form.Keys)
            {
                routeValues[key.ToString()] = _viewContext.RequestContext.HttpContext.Request.Form[key.ToString()];
            }
            routeValues[_pageQueryName] = pageNumber;

            string url = UrlHelper.GenerateUrl(null, null, null, routeValues, RouteTable.Routes,
                                               _viewContext.RequestContext, true);
            return url;
        }
    }
}
