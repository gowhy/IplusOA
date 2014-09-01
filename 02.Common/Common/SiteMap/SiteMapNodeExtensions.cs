using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common
{
    public static class SiteMapNodeExtensions
    {
        public static bool IsCurrent(this SiteMapNode navigatable, RequestContext requestContext)
        {
            Debug.Assert(requestContext.HttpContext.Request.Url != null, "viewContext.HttpContext.Request.Url != null");

            var currentUrl = requestContext.HttpContext.Request.Url.PathAndQuery;
            var url = Generate(requestContext, navigatable);
            var rgx = new Regex(@"#.*$");
            if (url != null) url = rgx.Replace(url, "");
            var currentRoute = new UrlHelper(requestContext).RouteUrl(requestContext.RouteData.Values);

            return url.IsCaseInsensitiveEqual(currentUrl) || url.IsCaseInsensitiveEqual(currentRoute);
        }

        public static string GenerateUrl(this SiteMapNode navigatable, RequestContext requestContext)
        {
            return Generate(requestContext, navigatable);
        }

        public static string GenerateUrl(this SiteMapNode navigatable, RequestContext requestContext, RouteValueDictionary routeValue)
        {
            return Generate(requestContext, navigatable, routeValue);
        }

        private static string Generate(RequestContext requestContext, SiteMapNode navigationItem)
        {
            var routeValues = new RouteValueDictionary();

            if (navigationItem.RouteValues.Any())
            {
                routeValues.Merge(navigationItem.RouteValues);
            }

            return Generate(requestContext, navigationItem, routeValues);
        }

        private static string Generate(RequestContext requestContext, SiteMapNode navigationItem, RouteValueDictionary routeValues)
        {
            Check.Argument.IsNotNull(requestContext, "requestContext");
            Check.Argument.IsNotNull(navigationItem, "navigationItem");

            var urlHelper = new UrlHelper(requestContext);
            string generatedUrl = null;

            if (!string.IsNullOrEmpty(navigationItem.RouteName))
            {
                generatedUrl = urlHelper.RouteUrl(navigationItem.RouteName, routeValues);
            }
            else if (!string.IsNullOrEmpty(navigationItem.ControllerName) && !string.IsNullOrEmpty(navigationItem.ActionName))
            {
                generatedUrl = urlHelper.Action(navigationItem.ActionName, navigationItem.ControllerName, routeValues, null, null);
            }
            else if (!string.IsNullOrEmpty(navigationItem.Url))
            {
                generatedUrl = navigationItem.Url.StartsWith("~/", StringComparison.Ordinal) ?
                               urlHelper.Content(navigationItem.Url) :
                               navigationItem.Url;
                //var rgx = new Regex(@"#.*$");
                //if(rgx.IsMatch(generatedUrl))
                //{
                //    generatedUrl = rgx.Match(generatedUrl).Value;
                //}
            }
            else if (routeValues.Any())
            {
                generatedUrl = urlHelper.RouteUrl(routeValues);
            }

            return generatedUrl;

        }

        private static void GetActionParams(RouteValueDictionary routeValues, out object actionName, out object controllerName, RouteValueDictionary values)
        {
            routeValues.TryGetValue("action", out actionName);
            routeValues.TryGetValue("controller", out controllerName);

            routeValues.Remove("action");
            routeValues.Remove("controller");

            values.Merge(routeValues);
        }

        public static void Action(this SiteMapNode navigatable, RouteValueDictionary routeValues)
        {
            object actionName;
            object controllerName;
            var values = new RouteValueDictionary();

            GetActionParams(routeValues, out actionName, out controllerName, values);

            SetAction(navigatable, (string)actionName, (string)controllerName, values);

        }

        private static void SetAction(SiteMapNode navigatable, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            navigatable.ActionName = actionName;
            navigatable.ControllerName = controllerName;
            navigatable.SetRouteValues(routeValues);
        }


        private static void SetRouteValues(this SiteMapNode navigatable, IDictionary<string, object> values)
        {
            if (values != null)
            {
                navigatable.RouteValues.Clear();
                navigatable.RouteValues.Merge(values);
            }
        }


    }
}
