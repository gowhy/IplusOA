using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class SiteMapBase
    {
        private static float defaultCacheDurationInMinutes;
        private static bool defaultCompress = true;
        private static bool defaultGenerateSearchEngineMap = true;

        private float cacheDurationInMinutes;

        protected SiteMapBase()
        {
            CacheDurationInMinutes = DefaultCacheDurationInMinutes;
            Compress = DefaultCompress;
            GenerateSearchEngineMap = DefaultGenerateSearchEngineMap;

            RootNode = new SiteMapNode();
        }

        public static float DefaultCacheDurationInMinutes
        {
            get
            {
                return defaultCacheDurationInMinutes;
            }

            set
            {
                Check.Argument.IsNotNegative(value, "value");

                defaultCacheDurationInMinutes = value;
            }
        }

        public static bool DefaultCompress
        {
            get
            {
                return defaultCompress;
            }

            set
            {
                defaultCompress = value;
            }
        }

        public static bool DefaultGenerateSearchEngineMap
        {
            get
            {
                return defaultGenerateSearchEngineMap;
            }

            set
            {
                defaultGenerateSearchEngineMap = value;
            }
        }

        public SiteMapNode RootNode
        {
            get;
            set;
        }

        public float CacheDurationInMinutes
        {
            get
            {
                return cacheDurationInMinutes;
            }

            set
            {
                Check.Argument.IsNotNegative(value, "value");

                cacheDurationInMinutes = value;
            }
        }

        public bool Compress
        {
            get;
            set;
        }

        public bool GenerateSearchEngineMap
        {
            get;
            set;
        }

        public static implicit operator SiteMapBuilder(SiteMapBase siteMap)
        {
         //   Check.Argument.IsNotNull(siteMap, "siteMap");

            return siteMap.ToBuilder();
        }

        public SiteMapBuilder ToBuilder()
        {
            return new SiteMapBuilder(this);
        }

        public virtual void Reset()
        {
            CacheDurationInMinutes = DefaultCacheDurationInMinutes;
            Compress = DefaultCompress;
            GenerateSearchEngineMap = DefaultGenerateSearchEngineMap;

            RootNode = new SiteMapNode();
        }
    }
}
