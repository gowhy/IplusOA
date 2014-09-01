using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SiteMapBuilder
    {
        private readonly SiteMapBase siteMap;
        private readonly SiteMapNodeBuilder siteMapNodeBuilder;

        public SiteMapBuilder(SiteMapBase siteMap)
        {
            Check.Argument.IsNotNull(siteMap, "siteMap");

            this.siteMap = siteMap;
            siteMapNodeBuilder = new SiteMapNodeBuilder(this.siteMap.RootNode);
        }

        public SiteMapNodeBuilder RootNode
        {
            get
            {
                return siteMapNodeBuilder;
            }
        }

        public static implicit operator SiteMapBase(SiteMapBuilder builder)
        {
           // Check.Argument.IsNotNull(builder, "builder");

            return builder.ToSiteMap();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public SiteMapBase ToSiteMap()
        {
            return siteMap;
        }

        public virtual SiteMapBuilder CacheDurationInMinutes(float value)
        {
            siteMap.CacheDurationInMinutes = value;

            return this;
        }

        public virtual SiteMapBuilder Compress(bool value)
        {
            siteMap.Compress = value;

            return this;
        }

        public virtual SiteMapBuilder GenerateSearchEngineMap(bool value)
        {
            siteMap.GenerateSearchEngineMap = value;

            return this;
        }
    }
}
