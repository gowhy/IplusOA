using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;


namespace Common
{
    public class SiteMapNodeBuilder
    {
        private readonly SiteMapNode siteMapNode;

        public SiteMapNodeBuilder(SiteMapNode siteMapNode)
        {
           // Check.Argument.IsNotNull(siteMapNode, "siteMapNode");

            this.siteMapNode = siteMapNode;
        }


        public static implicit operator SiteMapNode(SiteMapNodeBuilder builder)
        {
           // Check.Argument.IsNotNull(builder, "builder");

            return builder.ToNode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public SiteMapNode ToNode()
        {
            return siteMapNode;
        }

        public virtual SiteMapNodeBuilder Title(string value)
        {
            siteMapNode.Title = value;

            return this;
        }

        public virtual SiteMapNodeBuilder Visible(bool value)
        {
            siteMapNode.Visible = value;

            return this;
        }

        public virtual SiteMapNodeBuilder Route(string routeName, RouteValueDictionary routeValues)
        {
            siteMapNode.RouteName = routeName;

            SetRouteValues(routeValues);
            SetTitleIfEmpty(routeName);

            return this;
        }

        public virtual SiteMapNodeBuilder Route(string routeName, object routeValues)
        {
            Route(routeName, null);

            SetRouteValues(routeValues);

            return this;
        }

        public virtual SiteMapNodeBuilder Route(string routeName)
        {
            return Route(routeName, (object)null);
        }

        public SiteMapNodeBuilder Action(RouteValueDictionary routeValues)
        {
            siteMapNode.Action(routeValues);

            return this;
        }

        public virtual SiteMapNodeBuilder Action(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            siteMapNode.ControllerName = controllerName;
            siteMapNode.ActionName = actionName;

            SetRouteValues(routeValues);
            SetTitleIfEmpty(actionName);

            return this;
        }

        public virtual SiteMapNodeBuilder Action(string actionName, string controllerName, object routeValues)
        {
            Action(actionName, controllerName, null);

            SetRouteValues(routeValues);

            return this;
        }

        public virtual SiteMapNodeBuilder Action(string actionName, string controllerName)
        {
            return Action(actionName, controllerName, (object)null);
        }

        public virtual SiteMapNodeBuilder Action<TController>(Expression<Action<TController>> controllerAction) where TController : Controller
        {
            var call = (MethodCallExpression)controllerAction.Body;

            string controllerName = typeof(TController).Name;

            if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Controller name must end with 'Controller'.", "controllerAction");
            }

            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);

            if (controllerName.Length == 0)
            {
                throw new ArgumentException("Cannot route to class named 'Controller'.", "controllerAction");
            }

            if (call.Method.IsDefined(typeof(NonActionAttribute), false))
            {
                throw new ArgumentException("The specified method is not an action method.", "controllerAction");
            }

            string actionName = call.Method.GetCustomAttributes(typeof(ActionNameAttribute), false)
                                           .OfType<ActionNameAttribute>()
                                           .Select(attribute => attribute.Name)
                                           .FirstOrDefault() ?? call.Method.Name;

            siteMapNode.ControllerName = controllerName;
            siteMapNode.ActionName = actionName;

            ParameterInfo[] parameters = call.Method.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                var arg = call.Arguments[i];
                object value;
                var ce = arg as ConstantExpression;

                if (ce != null)
                {
                    value = ce.Value;
                }
                else
                {
                    var lambdaExpression = Expression.Lambda<Func<object>>(Expression.Convert(arg, typeof(object)));
                    var func = lambdaExpression.Compile();
                    value = func();
                }

                siteMapNode.RouteValues.Add(parameters[i].Name, value);
            }

            return this;
        }

        public virtual SiteMapNodeBuilder Url(string value)
        {
            siteMapNode.Url = value;

            return this;
        }

        public virtual SiteMapNodeBuilder IncludeInSearchEngineIndex(bool value)
        {
            siteMapNode.IncludeInSearchEngineIndex = value;

            return this;
        }

        public virtual SiteMapNodeBuilder Attributes(IDictionary<string, object> value)
        {
          //  Check.Argument.IsNotNull(value, "value");

            siteMapNode.Attributes.Clear();
            siteMapNode.Attributes.Merge(value);

            return this;
        }

        public virtual SiteMapNodeBuilder Attributes(object value)
        {
           // Check.Argument.IsNotNull(value, "value");

            return Attributes(new RouteValueDictionary(value));
        }

        public virtual SiteMapNodeBuilder ChildNodes(Action<SiteMapNodeFactory> addActions)
        {
          //  Check.Argument.IsNotNull(addActions, "addActions");

            var factory = new SiteMapNodeFactory(siteMapNode);

            addActions(factory);

            return this;
        }

        private void SetRouteValues(ICollection<KeyValuePair<string, object>> values)
        {
            if (values == null || !values.Any()) return;
            siteMapNode.RouteValues.Clear();
            siteMapNode.RouteValues.AddRange(values);
        }

        private void SetRouteValues(object values)
        {
            if (values == null) return;
            siteMapNode.RouteValues.Clear();
            siteMapNode.RouteValues.Merge(values);
        }

        private void SetTitleIfEmpty(string value)
        {
            if (string.IsNullOrEmpty(siteMapNode.Title))
            {
                siteMapNode.Title = value;
            }
        }
    }
}
