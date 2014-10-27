using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using BackWebAdmin.Models;
using ServiceAPI;
using System.IO;

namespace BackWebAdmin.Controllers
{
    public class TSController : BaseController
    {
        private static int PageSize = 20;
        // GET: TS
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var sup = db.SuperviseTable;
                var dep = db.DepartmentTable;
                var vol = db.VolunteerEntityTable;

                var list = from s in sup
                           join d in dep on s.DepId equals d.Id
                           join v in vol on s.AddUser equals v.Id
                           select new SuperviseEntityClone
                           {
                               AddUser = s.AddUser,
                               Content = s.Content,
                               AddTime = s.AddTime,
                               DepId = s.DepId,
                               DepName = d.Name,
                               Id = s.Id,
                               volEntity = v
                           };
                list = list.Where(x => x.DepId == AdminUser.DeptId);
                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }

        }





        [ValidateInput(false)]
        public ActionResult AppPostAdd(SuperviseEntity entity)
        {
            try
            {
                // entity.DepId = AdminUser.DeptId;
                using (IplusOADBContext db = new IplusOADBContext())
                {
                    entity.AddTime = DateTime.Now;
                    db.Add(entity);
                    db.SaveChanges();

                }
                return Json(new { state = 1, msg = "新增成功" });
            }
            catch (Exception ex)
            {

                return Json(new { state = 0, msg = ex.Message+ex.TargetSite });
            }
      
        }


        public ActionResult AppIndex(int userId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var sup = db.SuperviseTable;
                var list = from s in sup select s;
                list = list.Where(x => x.AddUser == userId);
                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult View(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //SuperviseEntity sugg = db.SuperviseTable.Find(id);
                //return View(sugg);

                var sup = db.SuperviseTable;
                var dep = db.DepartmentTable;
                var vol = db.VolunteerEntityTable;

                var list = from s in sup
                           join d in dep on s.DepId equals d.Id
                           join v in vol on s.AddUser equals v.Id
                           select new SuperviseEntityClone
                           {
                               AddUser = s.AddUser,
                               Content = s.Content,
                               AddTime = s.AddTime,
                               DepId = s.DepId,
                               DepName = d.Name,
                               Id = s.Id,
                               ImgUrl=s.ImgUrl,
                               volEntity = v
                           };
                var model = list.SingleOrDefault(x => x.Id == id);
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SuperviseEntity sup = db.SuperviseTable.Find(id);
                db.Delete<SuperviseEntity>(sup);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }


        public ActionResult SaveImg()
        {
            try
            {

                var file = Request.Files[0];


                string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
                string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
                string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
                string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

                string Dir = DateTime.Now.ToString("yyyyMMdd");
                FTPHelper ftp = new FTPHelper(FtpServer, "tousu/" + Dir, FtpUser, FtpPassWord);

                FileInfo file2 = new FileInfo(file.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.Id.ToString() + file2.Extension;

                ftp.Upload(file, fileName);

                string imgUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;

                return Json(new
                {
                    state = 1,
                    url = imgUrl,
                    error = ""
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    state = 0,
                    url = "",
                    error = ex.Message + ex.TargetSite
                });
                throw ex;
            }
        }
    }
}