using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class SecurityNodeAttribute : Attribute
    {
        /// <summary>
        /// 该节点当前是否有效
        /// </summary>
        public bool IsEffect { get; set; }

        /// <summary>
        /// 该节点
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 该节点
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 节点排序
        /// </summary>
        public int Sort { get; set; }
    }
}
