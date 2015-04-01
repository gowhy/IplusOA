using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace  Common.Cache
{
    public interface ICacheProvider
    {
        object Get(string key);
        void Remove(string key);
        void Insert(string key, object value);
        void Insert(string key, object value, int timeOut);
        void Insert(string key, object value, CacheDependency dependency);
        void Insert(string key, object value, CacheItemRemovedCallback cacheItemRemovedCallback, params string[] fileDependencies);
    }
}
