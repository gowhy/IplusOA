using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class CommonAction
    {
        private static Action<SecurityManager> _securityRegister;

        public static void Sercurity(Assembly assembly, Action<SecurityManager> action)
        {
            Action<SecurityManager> proxy = x =>
            {
                x.LoadAssembly(assembly);
                action(x);
            };
            _securityRegister = proxy;
        }
    }
}
