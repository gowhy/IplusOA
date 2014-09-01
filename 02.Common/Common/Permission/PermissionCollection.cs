using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PermissionCollection : List<Permission>
    {
        public bool IsPermission(Node node)
        {
            foreach (var permission in this)
            {
                //检查是否拥有包含节点的模块权限
                if (!string.IsNullOrWhiteSpace(permission.ModuleKey) && node.ModuleKey == permission.ModuleKey.ToLower())
                {
                    return true;
                }

                //检查是否拥有节点的权限
                if (node.NodeKey == permission.NodeKey)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasModule(Node node)
        {
            foreach (var permission in this)
            {
                //检查是否拥有包含节点的模块权限
                if (string.IsNullOrWhiteSpace(permission.ModuleKey) || node.ModuleKey != permission.ModuleKey.ToLower())
                    continue;

                return true;
            }

            return false;
        }

        public bool HasNode(Node node)
        {
            foreach (var permission in this)
            {
                //检查是否拥有节点的权限
                if (node.NodeKey != permission.NodeKey) continue;
                return true;
            }

            return false;
        }
    }
}
