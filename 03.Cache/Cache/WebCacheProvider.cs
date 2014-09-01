using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Common
{
    public class WebCacheProvider : ICacheProvider
    {
        public object Get(string key)
        {
            return HttpRuntime.Cache[key];
        }

        public void Insert(string key, object value)
        {
            HttpRuntime.Cache.Insert(key, value);
        }

        public void Insert(string key, object value, CacheItemRemovedCallback cacheItemRemovedCallback, params string[] fileDependencies)
        {
            HttpRuntime.Cache.Insert(key, value, fileDependencies.Any() ? new CacheDependency(fileDependencies) : null,
                System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, cacheItemRemovedCallback);
        }

        public void Insert(string key, object value, CacheDependency dependency)
        {
            HttpRuntime.Cache.Insert(key, value, dependency,
                System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, null);
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}

