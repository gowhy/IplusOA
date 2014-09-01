using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLogic.Role
{
    public class RoleManager
    {
        public static bool CreateRole(string name, string[] modules, string[] nodes, ref string msg, bool isEffect = true)
        {
            try
            {
                if (ExistRole(name))
                {
                    msg = "管理员角色已经存在";
                    return false;
                }
                var role = new RoleEntity
                {
                    Name = name.Trim(),
                    ListRolePermission = new List<RolePermissionEntity>(),
                    IsEffect = true,
                };

                AddModulesForRole(role, modules);
                AddNodesForRole(role, nodes);

                IplusOADBContext db = new IplusOADBContext();
                db.RoleTable.Add(role);
                db.SaveChanges();
                db.Dispose();
                return true;

            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }
          

        }

        private static void AddModulesForRole(RoleEntity role, IEnumerable<string> modules)
        {
            foreach (var mid in modules ?? new string[] { })
            {
                role.ListRolePermission.Add(new RolePermissionEntity { RoleId = role.Id, Module = mid, Node = string.Empty });
            }
        }

        private static void AddNodesForRole(RoleEntity role, IEnumerable<string> nodes)
        {
            foreach (var nid in nodes ?? new string[] { })
            {
                role.ListRolePermission.Add(new RolePermissionEntity { RoleId = role.Id, Module = string.Empty, Node = nid });
            }
        }

        private static bool ExistRole(string roleName)
        {
            IplusOADBContext db = new IplusOADBContext();
            return db.RoleTable.Count(x => x.Name == roleName) > 0;

        }

        /// <summary>
        /// 更新角色名称
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="isEffect"></param>
        /// <param name="msg"></param>
        public void UpdateRole(int roleId, string roleName, bool isEffect,ref string msg)
        {
            IplusOADBContext db = new IplusOADBContext();
            var role = db.RoleTable.FirstOrDefault(x => x.Id == roleId);
            if (role == null)
            {
                msg += ("角色不存在"); 
            }
            role.Name = roleName;
            role.IsEffect = isEffect;
            db.Update<RoleEntity>(role);

        }

        /// <summary>
        /// 修改角色权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="modules"></param>
        /// <param name="nodes"></param>
        public void UpdateRoleAccess(int roleId, string[] modules, string[] nodes, ref string msg)
        {
            try
            {
                IplusOADBContext db = new IplusOADBContext();
                var role = db.RoleTable.FirstOrDefault(x => x.Id == roleId);
                if (role == null) return;
                var user = db.RolePermission.AsQueryable().Where(x => x.RoleId == roleId);
                db.RolePermission.RemoveRange(user);
               

                AddModulesForRole(role, modules);
                AddNodesForRole(role, nodes);
                db.RolePermission.AddRange(role.ListRolePermission);
                db.SaveChanges();

                db.Dispose();
            }
            catch (Exception ex)
            {
                msg += ex.Message;
            }
         
        }

    
    }
}
