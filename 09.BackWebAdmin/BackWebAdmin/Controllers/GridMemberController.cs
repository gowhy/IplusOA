using BackWebAdmin.CommService;
using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "网格员")]
    public class GridMemberController : BaseController
    {
        private static int PageSize = 20;
        //
        // GET: /StartAdImg/
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            BackAdminUser bUser = this.GetBackUserInfo();

            using (IplusOADBContext db = new IplusOADBContext())
            {

                var work = db.GridMemberTable;

                var list = from w in work select w;

                list = list.Where(x => x.DeptId == bUser.DeptId);

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(int userId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (IplusOADBContext db = new IplusOADBContext())
            {
                var vol = db.VolunteerEntityTable;
                var grid = db.GridMemberTable;
                var dep = db.DepartmentTable;

                VolunteerEntity AppUser = base.GetAppUserInfoById(userId);

                var list = from g in grid select g;

                list = list.Where(x => x.DeptId == AppUser.DepId);

                List<GridMember> listObj = list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size).ToList();

                var depNameList = from d in dep where d.PId == AppUser.DepId select d;
                foreach (var item in listObj)
                {
                    if (item != null && !string.IsNullOrEmpty(item.VDeptId))
                    {
                        foreach (var item2 in item.VDeptId.Split(','))
                        {
                            string tmp = (depNameList.FirstOrDefault(x => x.Id == item2) ?? new DepartmentEntity()).Name;
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                item.VDeptName += tmp + ",";
                            }
                        }
                    }
                }

                List<GridMember> listObj1 = listObj.FindAll(x => x.DeptId == AppUser.DepId && x.VDeptName.Contains(AppUser.Address));
                List<GridMember> listObj2 = listObj.FindAll(x => !(x.DeptId == AppUser.DepId && x.VDeptName.Contains(AppUser.Address)));

                List<GridMember> listObj3 = new List<GridMember>();


                if (listObj1 != null)
                {
                    listObj3 = listObj3.Union(listObj1).ToList();
                }
                if (listObj2 != null)
                {
                    listObj3 = listObj3.Union(listObj2).ToList();
                }

                return Json(listObj3);
            }
        }


        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {
            BackAdminUser bUser = this.GetBackUserInfo();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //获取当前登录用户下的小区信息
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level == 7&&x.PId==bUser.DeptId).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
            }

            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAdd(GridMember entity)
        {
            if (string.IsNullOrEmpty(entity.GridPhone)||string.IsNullOrEmpty(entity.GridName)||string.IsNullOrEmpty(entity.GridNo))
            {
                return RedirectToAction("Add");
            }
            BackAdminUser bUser = this.GetBackUserInfo();

            entity.AddUserId = bUser.Id;
            entity.AddTime = DateTime.Now;
            entity.DeptId = bUser.DeptId;

            using (IplusOADBContext db = new IplusOADBContext())
            {
             
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult SaveHeaderImg()
        {
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];

            //网格员头像
            SocSerImgEntity res = UploadFile.SaveFile(file, "GridMember", "");

            return Json(res);
           
        }


        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                GridMember notice = db.GridMemberTable.Find(id);
                db.Delete<GridMember>(notice);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult View(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                GridMember work = db.GridMemberTable.Find(id);
                return View(work);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(GridMember entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                GridMember grid = db.GridMemberTable.Find(entity.Id);

                //grid.AddTime = DateTime.Now;
                //grid.DepId = entity.DepId;
                //grid.AddUser = entity.AddUser;
                //grid.ImgUrl = entity.ImgUrl;
                //grid.Title = entity.Title;
           
                db.Update(grid);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult AppView(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                GridMember work = db.GridMemberTable.Find(id);
                return Json(work);
            }
        }
      
    }
}