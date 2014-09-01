using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class SiteMapManager
    {
        private static readonly SiteMapDictionary siteMaps = new SiteMapDictionary();

        public static SiteMapDictionary SiteMaps
        {
            get
            {
                return siteMaps;
            }
        }

        internal static void Clear()
        {
            SiteMaps.Clear();
        }
    }
}
