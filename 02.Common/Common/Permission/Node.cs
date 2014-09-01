using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Node
    {
        /// <summary>
        /// 节点的模块名称
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 节点的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点的描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 节点的Controller
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 节点的Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 当前节点是否有效
        /// </summary>
        public bool IsEffect { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 模块Key
        /// </summary>
        public string ModuleKey
        {
            get
            {
                return "{0}".FormatWith(Controller.ToLower());
            }
        }

        /// <summary>
        /// 节点Key
        /// </summary>
        public string NodeKey
        {
            get
            {
                return "{0}_{1}".FormatWith(Controller.ToLower(), Action.ToLower());
            }
        }

    }
}
