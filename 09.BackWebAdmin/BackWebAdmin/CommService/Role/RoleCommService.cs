using BackWebAdmin.Models;
using BusLogic.Role;
using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin.CommService
{
    public class RoleCommService
    {

        public bool BllPostEdit(RoleEditRole model, ref string errMsg)
        {
            RoleManager manager = new RoleManager();
            manager.UpdateRole(model.ID, model.Name, model.IsEffect, ref errMsg);
            manager.UpdateRoleAccess(model.ID, model.RoleModule, model.RoleNode, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return false;
            }
            return true;
        }


        public PermissionCollection GetPermissions(int roleId)
        {
            var permissions = new PermissionCollection();
            IplusOADBContext db = new IplusOADBContext();
            db.RolePermission.AsQueryable<RolePermissionEntity>().Where(x => x.RoleId == roleId)
                .ToList().ForEach(access => permissions.Add(new Permission { ModuleKey = access.Module, NodeKey = access.Node }));

            return permissions;
        }
    }
}