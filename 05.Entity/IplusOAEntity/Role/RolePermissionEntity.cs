using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace IplusOAEntity
{
    /// <summary>
    /// 角色权限控制
    /// </summary>
    public class RolePermissionEntity
    {
        public int Id { get; set; }
         

        public int RoleId { get; set; }

        /// <summary>
        /// node key ,默认为0，如果有值，将拥有此节点的权限
        /// </summary>
        public string Node { get; set; }
        /// <summary>
        /// module id，默认为0，如果有值，将拥有此模块下所有节点权限
        /// </summary>
        public string Module { get; set; }
        
        public RoleEntity Role { get; set; }
    }
}
