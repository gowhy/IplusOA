using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
namespace Common
{
    public static class ScriptManagementExtension
    {
        private const string ScriptFormat = "<script type=\"text/javascript\" src=\"{0}\"></script>";
        private const string CSSFormat = "<link href=\"{0}\" rel=\"stylesheet\"/>";

        private static MvcHtmlString IncludeHeader(HtmlHelper helper, string key, string path, string format)
        {
            var context = helper.ViewContext.HttpContext;
            var exists = context.Items.Contains(key);
            if (!exists)
            {
                var url = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
                context.Items[key] = true;
                return new MvcHtmlString(string.Format(format, url.Content(path)));
            }
            return null;
        }
        public static MvcHtmlString IncludeScript(this　HtmlHelper helper, string path)
        {
            return IncludeScript(helper, path.ToLower(), path);
        }
        public static MvcHtmlString IncludeScript(this　HtmlHelper helper, string key, string path)
        {
            return IncludeHeader(helper, key, path, ScriptFormat);
        }
        public static MvcHtmlString IncludeCSS(this　HtmlHelper helper, string path)
        {
            return IncludeCSS(helper, path.ToLower(), path);
        }
        public static MvcHtmlString IncludeCSS(this　HtmlHelper helper, string key, string path)
        {
            return IncludeHeader(helper, key, path, CSSFormat);
        }
    }
}
