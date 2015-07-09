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
    [SecurityModule(Name = "社区微网站")]
    public class MicrWebSiteController : BaseController
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

                var sungov = db.MicrWebSiteTable;
                var backUser = db.BackAdminUserEntityDBSet;
                var dept = db.DepartmentTable;

                var list = from s in sungov
                           join b in backUser on s.AddUserId equals b.Id
                           join d in dept on s.DeptId equals d.Id
                           select new MicrWebSiteModel
                           {
                               BackAdminUserEntity = b,
                               MicrWebSiteEntity = s,
                               DepartmentEntity=d
                           };

                if (!string.IsNullOrEmpty(bUser.DeptId))
                {
                    list = list.Where(x => x.MicrWebSiteEntity.DeptId == bUser.DeptId);
                }

                return View(list.OrderByDescending(x => x.MicrWebSiteEntity.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var dept = db.DepartmentTable;
                var sungov = db.MicrWebSiteTable;
                var backUser = db.BackAdminUserEntityDBSet;
                var list = from s in sungov
                           join b in backUser on s.AddUserId equals b.Id
                           join d in dept on s.DeptId equals d.Id
                           select new MicrWebSiteModel
                           {
                               BackAdminUserEntity = b,
                               MicrWebSiteEntity = s,
                               DepartmentEntity = d
                           };


                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.MicrWebSiteEntity.DeptId == depId);
                }

                return Json(list.OrderByDescending(x => x.MicrWebSiteEntity.Id).ToPagedList(pageNumber - 1, size), JsonRequestBehavior.AllowGet);
            }
        }
        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {
         
            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAdd(MicrWebSite entity)
        {
            BackAdminUser bUser = this.GetBackUserInfo();
            entity.DeptId = bUser.DeptId;
            entity.AddUserId = bUser.Id;

            using (IplusOADBContext db = new IplusOADBContext())
            {
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

     

        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                MicrWebSite notice = db.MicrWebSiteTable.Find(id);
                db.Delete<MicrWebSite>(notice);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                MicrWebSite work = db.MicrWebSiteTable.Find(id);
                return View(work);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(MicrWebSite entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                MicrWebSite work = db.MicrWebSiteTable.Find(entity.Id);

                work.Name = entity.Name;
                work.LinkUrl = entity.LinkUrl;
                work.ImgUrl = entity.ImgUrl;
                work.Type = entity.Type;
                work.UploadHtmlFile = entity.UploadHtmlFile;
                db.Update(work);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult AppView(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                MicrWebSite work = db.MicrWebSiteTable.Find(id);
                return Json(work);
            }
        }
     
        public ActionResult SaveUploadHtmlFile()
        {
            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];
            SocSerImgEntity res = UploadFile.SaveFile(file, "MicrWebSite", "");

            return Json(res);
        }
    }
}