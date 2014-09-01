using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Routing;
using System.Xml;

namespace Common
{
    public class XmlSiteMap : SiteMapBase
    {
        private const string SiteMapNodeName = "siteMapNode";
        private const string RouteValuesNodeName = "routeValues";
        private const string TitleAttributeName = "title";
        private const string VisibleAttributeName = "visible";
        private const string RouteAttributeName = "route";
        private const string ControllerAttributeName = "controller";
        private const string ActionAttributeName = "action";
        private const string UrlAttributeName = "url";
        private const string LastModifiedAttributeName = "lastModifiedAt";
        private const string ChangeFrequencyAttributeName = "changeFrequency";
        private const string UpdatePriorityAttributeName = "updatePriority";
        private const string IncludeInSearchEngineIndexAttributeName = "includeInSearchEngineIndex";
        private const string AreaAttributeName = "area";

        private static readonly string[] knownAttributes = CreateKnownAttributes();

        private readonly ICacheProvider cacheProvider;
        private readonly IPathResolver pathResolver;
        private readonly IVirtualPathProvider provider;

        private static string defaultPath = "~/Web.sitemap";

        public XmlSiteMap(IPathResolver pathResolver, IVirtualPathProvider provider, ICacheProvider cacheProvider)
        {
            Check.Argument.IsNotNull(pathResolver, "pathResolver");
            Check.Argument.IsNotNull(provider, "fileSystem");
            Check.Argument.IsNotNull(cacheProvider, "cacheProvider");

            //this.pathResolver = pathResolver;
          
           // this.cacheProvider = cacheProvider;
        }

        //public XmlSiteMap()
        //    : this(IoC.Current.Resolve<IPathResolver>(), IoC.Current.Resolve<IVirtualPathProvider>(), IoC.Current.Resolve<ICacheProvider>())
        //{
        //}
        public XmlSiteMap()
        {
            this.provider = new VirtualPathProviderWrapper();
            this.cacheProvider = new WebCacheProvider();
            this.pathResolver = new PathResolver();
        }
        public static string DefaultPath
        {
            get
            {
                return defaultPath;
            }

            set
            {
                Check.Argument.IsNotEmpty(value, "value");

                defaultPath = value;
            }
        }

        public void Load()
        {
            LoadFrom(DefaultPath);
        }

        public virtual void LoadFrom(string relativeVirtualPath)
        {
            Check.Argument.IsNotEmpty(relativeVirtualPath, "relativeVirtualPath");

            if (!string.IsNullOrEmpty(relativeVirtualPath))
            {
                InternalLoad(relativeVirtualPath);
            }
        }

        internal void InsertInCache(string filePath)
        {
            string cacheKey = GetType().AssemblyQualifiedName + ":" + filePath;

            if (cacheProvider.Get(cacheKey) == null)
            {
                cacheProvider.Insert(cacheKey, filePath, OnCacheItemRemoved, pathResolver.Resolve(filePath));
            }
        }

        internal virtual void InternalLoad(string physicalPath)
        {
           //physicalPath= HostingEnvironment.MapPath(physicalPath);
            string content = provider.ReadAllText(physicalPath);

            if (string.IsNullOrEmpty(content)) return;

            using (var sr = new StringReader(content))
            {
                using (XmlReader xr = XmlReader.Create(sr, new XmlReaderSettings { CloseInput = true, IgnoreWhitespace = true, IgnoreComments = true, IgnoreProcessingInstructions = true }))
                {
                    var doc = new XmlDocument();
                    doc.Load(xr);

                    Reset();

                    if ((doc.DocumentElement != null) && doc.HasChildNodes)
                    {
                        CacheDurationInMinutes = GetFloatValueFromAttribute(doc.DocumentElement, "cacheDurationInMinutes", DefaultCacheDurationInMinutes);
                        Compress = GetBooleanValueFromAttribute(doc.DocumentElement, "compress", true);
                        GenerateSearchEngineMap = GetBooleanValueFromAttribute(doc.DocumentElement, "generateSearchEngineMap", true);

                        XmlNode xmlRootNode = doc.DocumentElement.FirstChild;
                        Iterate(RootNode, xmlRootNode);

                        // Cache it for file change notification
                        InsertInCache(physicalPath);
                    }
                }
            }
        }

        internal void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason == CacheItemRemovedReason.DependencyChanged)
            {
                InternalLoad((string)value);
            }
        }

        private static void Iterate(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            PopulateNode(siteMapNode, xmlNode);

            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.LocalName.IsCaseSensitiveEqual(SiteMapNodeName))
                {
                    var siteMapChildNode = new SiteMapNode();
                    siteMapNode.ChildNodes.Add(siteMapChildNode);

                    Iterate(siteMapChildNode, xmlChildNode);
                }
            }
        }

        private static void PopulateNode(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            var routeValues = new RouteValueDictionary();

            XmlNode xmlRouteValuesNode = xmlNode.FirstChild;

            if ((xmlRouteValuesNode != null) && xmlRouteValuesNode.LocalName.IsCaseSensitiveEqual(RouteValuesNodeName))
            {
                foreach (XmlNode routeValue in xmlRouteValuesNode.ChildNodes)
                {
                    routeValues[routeValue.LocalName] = routeValue.InnerText;
                }
            }

            siteMapNode.Title = GetStringValueFromAttribute(xmlNode, TitleAttributeName);
            siteMapNode.Visible = GetBooleanValueFromAttribute(xmlNode, VisibleAttributeName, true);

            string routeName = GetStringValueFromAttribute(xmlNode, RouteAttributeName);
            string controllerName = GetStringValueFromAttribute(xmlNode, ControllerAttributeName);
            string actionName = GetStringValueFromAttribute(xmlNode, ActionAttributeName);
            string url = GetStringValueFromAttribute(xmlNode, UrlAttributeName);

            string areaName = GetStringValueFromAttribute(xmlNode, AreaAttributeName);

            if (areaName != null)
            {
                routeValues["area"] = areaName;
            }

            if (!string.IsNullOrEmpty(routeName))
            {
                siteMapNode.RouteName = routeName;
                siteMapNode.RouteValues.Clear();
                siteMapNode.RouteValues.Merge(routeValues);
            }
            else if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                siteMapNode.ControllerName = controllerName;
                siteMapNode.ActionName = actionName;
                siteMapNode.RouteValues.Clear();
                siteMapNode.RouteValues.Merge(routeValues);
            }
            else if (!string.IsNullOrEmpty(url))
            {
                siteMapNode.Url = url;
            }

            DateTime? lastModified = GetDateValueFromAttribute(xmlNode, LastModifiedAttributeName);

            if (lastModified.HasValue)
            {
                siteMapNode.LastModifiedAt = lastModified.Value;
            }

            siteMapNode.IncludeInSearchEngineIndex = GetBooleanValueFromAttribute(xmlNode, IncludeInSearchEngineIndexAttributeName, true);

            if (xmlNode.Attributes != null)
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    if (!string.IsNullOrEmpty(attribute.LocalName))
                    {
                        // Only add unknown attibutes
                        if (Array.BinarySearch(knownAttributes, attribute.LocalName, StringComparer.OrdinalIgnoreCase) < 0)
                        {
                            siteMapNode.Attributes.Add(attribute.LocalName, attribute.Value);
                        }
                    }
                }
        }

        private static string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            string value = null;

            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                var attribute = node.Attributes[attributeName];

                if (attribute != null)
                {
                    value = attribute.Value;
                }
            }

            return value;
        }

        private static bool GetBooleanValueFromAttribute(XmlNode node, string attributeName, bool defaultValue)
        {
            var value = defaultValue;

            var stringValue = GetStringValueFromAttribute(node, attributeName);

            if (!string.IsNullOrEmpty(stringValue))
            {
                bool tempValue;

                if (bool.TryParse(stringValue, out tempValue))
                {
                    value = tempValue;
                }
            }

            return value;
        }

        private static float GetFloatValueFromAttribute(XmlNode node, string attributeName, float defaultValue)
        {
            var value = defaultValue;

            var stringValue = GetStringValueFromAttribute(node, attributeName);

            if (!string.IsNullOrEmpty(stringValue))
            {
                float tempValue;

                if (float.TryParse(stringValue, out tempValue))
                {
                    value = tempValue;
                }
            }

            return value;
        }

        private static DateTime? GetDateValueFromAttribute(XmlNode node, string attributeName)
        {
            string stringValue = GetStringValueFromAttribute(node, attributeName);
            DateTime? value = null;

            if (!string.IsNullOrEmpty(stringValue))
            {
                DateTime tempValue;

                if (DateTime.TryParse(stringValue, out tempValue))
                {
                    value = tempValue;
                }
            }

            return value;
        }

        private static string[] CreateKnownAttributes()
        {
            var attributes = new List<string>(new[] { TitleAttributeName, VisibleAttributeName, RouteAttributeName, ControllerAttributeName, ActionAttributeName, UrlAttributeName, LastModifiedAttributeName, ChangeFrequencyAttributeName, UpdatePriorityAttributeName, IncludeInSearchEngineIndexAttributeName });

            attributes.Sort(StringComparer.OrdinalIgnoreCase);

            return attributes.ToArray();
        }

        /*
                private static T ToEnum<T>(string value, T defaultValue) where T : IComparable, IFormattable
                {
                    var enumType = typeof(T);

                    return Enum.IsDefined(enumType, value) ? (T)Enum.Parse(enumType, value, true) : defaultValue;
                }
        */
    }
}
