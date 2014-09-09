using BackWebAdmin.Models;
using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BackWebAdmin.Controllers
{
     [SecurityModule(Name = "用户管理")]
    public class UserController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int pageSize = 20;


        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page)
        {

            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var backAdminUser = db.BackAdminUserEntityDBSet.Include(x => x.Role)//
                    .Include(x => x.Department)//
                    .Include(x => x.SocialOrg)//
                    .AsQueryable<BackAdminUser>().ToList();
                return View(backAdminUser.ToPagedList(pageNumber - 1, pageSize));
            }
        }
        [SecurityNode(Name = "列表页")]
        public ActionResult List()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                IList<BackAdminUser> backAdminUser = db.BackAdminUserEntityDBSet.AsQueryable<BackAdminUser>().ToList();
                return View(backAdminUser);
            }
        }
        [SecurityNode(Name = "添加页")]
        public PartialViewResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                IList<RoleEntity> role = db.RoleTable.AsQueryable<RoleEntity>().ToList();

                ViewData["UserRole"] = role;

                //部门组织
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);


                IList<SocialOrgEntity> Slist = db.SocialOrgEntityTable.AsQueryable<SocialOrgEntity>().ToList();
                ViewData["SocialOrg_List"] = Slist;

                db.Dispose();
                return PartialView();
            }
        }
        [SecurityNode(Name = "增加用户")]
        public ActionResult PostAdd(BackAdminUser user)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                if (db.BackAdminUserEntityDBSet.Count(u => u.UserName.Trim() == user.UserName.Trim()) > 0)
                {
                    db.Dispose();
                    return Error("用户已经存在");
                }
                user.Role = db.RoleTable.FirstOrDefault(r => r.Id == user.RoleId);
                db.BackAdminUserEntityDBSet.Add(user);
                db.SaveChanges();
                return Success("操作成功");
            }
        }

        [SecurityNode(Name = "删除用户")]
        public ActionResult Delete(int id)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                BackAdminUser user = db.BackAdminUserEntityDBSet.Find(id);
                db.Delete<BackAdminUser>(user);
                db.SaveChanges();
                return Success("操作成功");
            }
        }

        [SecurityNode(Name = "编辑页面")]
        public PartialViewResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                BackAdminUser user = db.BackAdminUserEntityDBSet.Find(id);

                IList<RoleEntity> role = db.RoleTable.AsQueryable<RoleEntity>().ToList();

                ViewData["UserRole"] = role;


                //部门组织
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);


                IList<SocialOrgEntity> Slist = db.SocialOrgEntityTable.AsQueryable<SocialOrgEntity>().ToList();
                ViewData["SocialOrg_List"] = Slist;

                db.Dispose();

                return PartialView(user);
            }
        }


        [SecurityNode(Name = "编辑保存用户")]
        public ActionResult PostEdit(BackAdminUser model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                BackAdminUser user = db.BackAdminUserEntityDBSet.First(x => x.Id == model.Id);
                if (user == null) return Error("用户不存在");

                user.DeptId = model.DeptId;
                user.RealName = model.RealName;
                user.RoleId = model.RoleId;
                user.SocOrgId = model.SocOrgId;

                db.Update<BackAdminUser>(user);
                db.SaveChanges();

                return Success("操作成功");
            }
        }

   
	}
}