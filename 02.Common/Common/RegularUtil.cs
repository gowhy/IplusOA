using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public static class RegularUtil
    {
        public static Regex Email = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex WebUrl = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?|(http|https)://localhost:\d{2,}[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex Password = new Regex(@"^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~]{6,22}$", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// 6~12 char and the letter(_) start
        /// </summary>
        public static Regex UserName = new Regex(@"^[A-Za-z_]\w{3,16}$", RegexOptions.Singleline | RegexOptions.Compiled);
    }
}
