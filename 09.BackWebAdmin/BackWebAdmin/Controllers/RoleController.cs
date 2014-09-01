using BackWebAdmin.CommService;
using BackWebAdmin.Models;
using BusLogic.Role;
using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "角色管理")]
    public class RoleController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int pageSize = 20;
        [SecurityNode(Name = "角色管理列表")]
        // GET: /Role/
        public ActionResult Index(int? page)
        {

          
            var pageNumber = page ?? 1;

            IplusOADBContext db = new IplusOADBContext();
            var role = db.RoleTable.AsQueryable<RoleEntity>().ToList();

            return View(role.ToPagedList(pageNumber - 1, pageSize));

        }

        [SecurityNode(Name = "添加")]
        public ActionResult Add()
        {
            var roleModel = CreateEmptyRoleModel();
            return View(roleModel);
        }

        [HttpPost]
        [SecurityNode(Name = "保存添加")]
        public ActionResult PostAdd(RoleEditRole model)
        {
            RolePermissionEntity entity = new RolePermissionEntity();
            string errMsg = "";

            if (!RoleManager.CreateRole(model.Name, model.RoleModule, model.RoleNode, ref errMsg, model.IsEffect))
            {
                return Error("角色添加失败,失败原因:" + errMsg);
            }
            return Success("角色添加成功");
        }

        [SecurityNode(Name = "修改")]
        public ActionResult Edit(int id)
        {
            IplusOADBContext db = new IplusOADBContext();

            RoleEntity role = db.RoleTable.FirstOrDefault(x => x.Id == id);

            if (role == null) return Error("角色不存在");
            PermissionCollection per = new RoleCommService().GetPermissions(id);
            var roleModel = CreateRoleModel(role, per);
            return View(roleModel);

        }

        #region 编辑角色权限
        [HttpPost]
        [SecurityNode(Name = "保存修改")]
        public ActionResult PostEdit(RoleEditRole model)
        {
            string errMsg = "";
            RoleCommService role = new RoleCommService();
            role.BllPostEdit(model, ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                return Error(errMsg);
            }
            return Success("修改角色成功");
        }

        #endregion

        [SecurityNode(Name = "删除")]
        public ActionResult Delete(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            RoleEntity role = db.RoleTable.FirstOrDefault(x => x.Id == id);
            db.Delete<RoleEntity>(role);
            db.SaveChanges();
            return Success("操作成功");
        }
    }
}