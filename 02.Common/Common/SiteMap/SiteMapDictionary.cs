using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SiteMapDictionary : IDictionary<string, SiteMapBase>
    {
        private readonly IDictionary<string, SiteMapBase> innerDictionary = new Dictionary<string, SiteMapBase>(StringComparer.OrdinalIgnoreCase);

        private static Func<SiteMapBase> defaultSiteMapFactory = CreateDefaultSiteMapFactory();

        private SiteMapBase defaultSiteMap;

        public static Func<SiteMapBase> DefaultSiteMapFactory
        {
            get
            {
                return defaultSiteMapFactory;
            }
            set
            {
                Check.Argument.IsNotNull(value, "value");

                defaultSiteMapFactory = value;
            }
        }

        public SiteMapBase DefaultSiteMap
        {
            get { return defaultSiteMap ?? (defaultSiteMap = DefaultSiteMapFactory()); }

            set
            {
                defaultSiteMap = value;
            }
        }

        public int Count
        {
            get
            {
                return innerDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return innerDictionary.IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return innerDictionary.Keys;
            }
        }

        public ICollection<SiteMapBase> Values
        {
            get
            {
                return innerDictionary.Values;
            }
        }

        public SiteMapBase this[string key]
        {
            get
            {
                return innerDictionary[key];
            }

            set
            {
                innerDictionary[key] = value;
            }
        }

        public SiteMapDictionary Register<TSiteMap>(string name, Action<TSiteMap> configure) where TSiteMap : SiteMapBase, new()
        {
            Check.Argument.IsNotEmpty(name, "name");
            Check.Argument.IsNotNull(configure, "configure");

            var siteMap = new TSiteMap();
            configure(siteMap);

            Add(name, siteMap);

            return this;
        }

        public void Add(KeyValuePair<string, SiteMapBase> item)
        {
            innerDictionary.Add(item);
        }

        public void Add(string key, SiteMapBase value)
        {
            innerDictionary.Add(key, value);
        }


        public void Clear()
        {
            innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, SiteMapBase> item)
        {
            return innerDictionary.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return innerDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, SiteMapBase>[] array, int arrayIndex)
        {
            innerDictionary.CopyTo(array, arrayIndex);
        }


        public IEnumerator<KeyValuePair<string, SiteMapBase>> GetEnumerator()
        {
            return innerDictionary.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, SiteMapBase> item)
        {
            return innerDictionary.Remove(item);
        }

        public bool Remove(string key)
        {
            return innerDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out SiteMapBase value)
        {
            return innerDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static Func<SiteMapBase> CreateDefaultSiteMapFactory()
        {
            Func<SiteMapBase> factory = () =>
            {
                var siteMap = new XmlSiteMap();

                siteMap.Load();

                return siteMap;
            };

            return factory;
        }
    }
}
