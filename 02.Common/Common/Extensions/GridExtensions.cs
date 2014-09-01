using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcContrib.UI.Grid;
using MvcContrib.UI.Grid.Syntax;
using System.Web.Mvc;
using System.Web;

namespace Common
{
    public static class GridExtensions
    {
        public static IGridWithOptions<T> Complex<T>(this IGridWithOptions<T> gridWithOptions, ViewDataDictionary viewData) where T : class
        {
            return gridWithOptions.Sort((GridSortOptions)viewData["sort"]).Empty("<p class=\"nodata\">查询数据不存在</p>");
        }

        public static IGridColumn<T> CheckBox<T>(this ColumnBuilder<T> column, Func<T, object> keyFunc) where T : class
        {
            return column.Custom(x =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("<input type=\"checkbox\" name=\"{0}\" value=\"{1}\"/>".FormatWith(
                    "key", keyFunc(x)));
                return new HtmlString(sb.ToString());
            })
                              .Header(x => "<input type=\"checkbox\" id=\"check\" name=\"chk_all\">")
                              .Attributes(@class => "chk")
                              .HeaderAttributes(style => "width:30px;align:center;")
                              .Sortable(false);

        }

        public static IGridColumn<T> Buttons<T>(this ColumnBuilder<T> column, Action<GridButtons<T>> buttons) where T : class
        {
            return column.Custom(x =>
            {
                var button = new GridButtons<T>(x);
                buttons(button);
                return button.ToHtmlString();
            })
                .Sortable(false).Named("操作").Attributes(@class => "grid_btn");
        }
    }

    public class GridButtons<T> : IHtmlString where T : class
    {
        private readonly List<Func<T, IHtmlString>> _columns = new List<Func<T, IHtmlString>>();
        private readonly T _instace;

        public GridButtons(T instance)
        {
           // Check.Argument.IsNotNull(instance, "instance");
            _instace = instance;
        }

        public GridButtons<T> Add(Func<T, IHtmlString> func)
        {
            _columns.Add(func);
            return this;
        }

        public string ToHtmlString()
        {
            var sb = new StringBuilder();
            foreach (var column in _columns)
            {
                sb.Append(column(_instace).ToHtmlString());
            }
            return sb.ToString();
        }
    }
}
