using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Common
{
    public class SiteMapNode
    {
        private string title;
        private string routeName;
        private string controllerName;
        private string actionName;
        private string url;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNode"/> class.
        /// </summary>
        public SiteMapNode()
        {
            Visible = true;
            RouteValues = new RouteValueDictionary();
            IncludeInSearchEngineIndex = true;
            Attributes = new RouteValueDictionary();
            ChildNodes = new SiteMapNodeCollection(this);
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
              //  Check.Argument.IsNotEmpty(value, "value");

                title = value;
            }
        }

        /// <summary>
        /// Gets or sets the last modified at.
        /// </summary>
        /// <value>The last modified at.</value>
        public DateTime? LastModifiedAt
        {
            get;
            set;
        }

        public string RouteName
        {
            get
            {
                return routeName;
            }

            set
            {
               // Check.Argument.IsNotEmpty(value, "value");

                routeName = value;
                controllerName = actionName = url = null;
            }
        }

        public string ControllerName
        {
            get
            {
                return controllerName;
            }

            set
            {
                Check.Argument.IsNotEmpty(value, "value");

                controllerName = value;

                routeName = url = null;
            }
        }

        public string ActionName
        {
            //get
            //{
            //    return actionName;
            //}

            //set
            //{
            //    Check.Argument.IsNotEmpty(value, "value");

            //    actionName = value;

            //    routeName = url = null;
            //}
            get;
            set;
        }

        public IList<SiteMapNode> ChildNodes
        {
            get;
            set;
        }

        public IDictionary<string, object> Attributes
        {
            get;
            private set;
        }

        public bool IncludeInSearchEngineIndex
        {
            get;
            set;
        }

        public RouteValueDictionary RouteValues
        {
            get;
            private set;
        }

        public bool Selected { get; set; }

        public string Url
        {
            get
            {
                return url;
            }

            set
            {
                Check.Argument.IsNotEmpty(value, "value");

                url = value;

                routeName = controllerName = actionName = null;
                RouteValues.Clear();
            }
        }

        public bool Visible
        {
            get;
            set;
        }

        public SiteMapNode Parent
        {
            get;
            protected internal set;
        }

        public SiteMapNode PreviousSibling
        {
            get;
            protected internal set;
        }

        public SiteMapNode NextSibling
        {
            get;
            protected internal set;
        }

        public static implicit operator SiteMapNodeBuilder(SiteMapNode node)
        {
            Check.Argument.IsNotNull(node, "node");

            return node.ToBuilder();
        }

        public SiteMapNodeBuilder ToBuilder()
        {
            return new SiteMapNodeBuilder(this);
        }
    }
}
