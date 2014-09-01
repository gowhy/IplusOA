using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Executes the provided action for each item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action.</param>
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> instance, Action<T> action)
        {
           // Check.Argument.IsNotNull(action, "action");

            if (instance == null)
            {
                return;
            }

            foreach (T item in instance)
            {
                action(item);
            }
        }

    }
}
