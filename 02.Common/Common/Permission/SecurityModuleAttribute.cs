using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SecurityModuleAttribute : Attribute
    {
        /// <summary>
        /// 该模块的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 该模块的描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 模块排序
        /// </summary>
        public int Sort { get; set; }
    }
}
