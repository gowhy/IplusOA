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
    [SecurityModule(Name = "社区公告")]
    public class NoticeController : BaseController
    {
        private static int PageSize = 20;
        //
        // GET: /StartAdImg/
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var notice = db.NoticeTable;

                var list = from a in notice
                           join d in dep on a.DepId equals d.Id
                           select new NoticeEntityClone
                           {

                               DepName = d.Name,
                               DepId = a.DepId,
                               ImgUrl=a.ImgUrl,
                               Id = a.Id,
                               Des = a.Des,
                               AddTime = a.AddTime,
                               Title=a.Title
                           };
                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var notice = db.NoticeTable;

                var list = from a in notice
                           join d in dep on a.DepId equals d.Id
                           select new NoticeEntityClone
                           {

                               DepName = d.Name,
                               DepId = a.DepId,
                               ImgUrl = a.ImgUrl,
                               Id = a.Id,
                               Des = a.Des,
                               AddTime = a.AddTime,
                               Title = a.Title
                           };
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
            }

            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAdd(NoticeEntity entity)
        {
            entity.DepId = AdminUser.DeptId;
            entity.AddUser = AdminUser.UserName;

            using (IplusOADBContext db = new IplusOADBContext())
            {
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];

            string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
            string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
            string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

            string Dir = DateTime.Now.ToString("yyyyMMdd");
            FTPHelper ftp = new FTPHelper(FtpServer, "NoticeImg/" + Dir, FtpUser, FtpPassWord);

            FileInfo file2 = new FileInfo(file.FileName);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.Id.ToString() + file2.Extension;

            ftp.Upload(file, fileName);

            SocSerImgEntity img = new SocSerImgEntity();
            img.FTPUrl = ftp.FtpURI;
            img.HttpUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
            img.Name = file.FileName;
            img.Module = "保存服务";
            img.AddTime = DateTime.Now;

            return Json(img);
        }
        public ActionResult ImgFile()
        {
              string action = Request["action"];
            if (action == "config")
            {
                string json = System.IO.File.ReadAllText(HttpContext.Server.MapPath("../config.json"));
                return Content(json);
            }

            if (action == "uploadimage")
            {
                var file = Request.Files["upfile"];

                //接收上传后的文件
                // System.Web.HttpPostedFileBase file = Request.Files["imgFile"];

                string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
                string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
                string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
                string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

                string Dir = DateTime.Now.ToString("yyyyMMdd");
                FTPHelper ftp = new FTPHelper(FtpServer, "imgFile/" + Dir, FtpUser, FtpPassWord);

                FileInfo file2 = new FileInfo(file.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.Id.ToString() + file2.Extension;

                ftp.Upload(file, fileName);

                string imgUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
                //SocSerImgEntity img = new SocSerImgEntity();
                //img.FTPUrl = ftp.FtpURI;
                //img.HttpUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
                //img.Name = file.FileName;
                //img.Module = "保存服务";
                //img.AddTime = DateTime.Now;
                //{"error":0,"message":".....","url":"/img/1111.gif"}


                return Json(new
                {
                    state = "SUCCESS",
                    url = imgUrl,
                    title = fileName,
                    original = file.FileName,
                    error = ""
                }, "text/html");
                //return Json(new { url = img.HttpUrl, error = 0 }, "text/html");
            }
            return Json(new
            {
                state = "Error",           
                error = ""
            }, "text/html");
        }
        
        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                NoticeEntity notice = db.NoticeTable.Find(id);
                db.Delete<NoticeEntity>(notice);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult View(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                NoticeEntity notice = db.NoticeTable.Find(id);
                return View(notice);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(NoticeEntity entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                entity.AddTime = DateTime.Now;
                db.Update(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }
        
    }
}