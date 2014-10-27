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
    [SecurityModule(Name = "服务评价")]
    public class SuggestionController : BaseController
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
              
                var sugg = db.SuggestionTable;
                var dep = db.DepartmentTable;
                var vol = db.VolunteerEntityTable;

                var list = from s in sugg
                            join d in dep on s.DepId equals d.Id
                            join v in vol on s.AddUser equals v.Id
                            select new SuggestionEntityClone
                            {
                                AddUser = s.AddUser,
                                Content = s.Content,
                                AddTime = s.AddTime,
                                DepId = s.DepId,
                                DepName = d.Name,
                                Id=s.Id,
                                volEntity = v
                            };

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(int userId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var sug = db.SuggestionTable;
                var list = from s in sug select s;

                list = list.Where(x => x.AddUser == userId);
                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        [ValidateInput(false)]
        public ActionResult AppPostAdd(SuggestionEntity entity)
        {
            try
            {
                entity.DepId = AdminUser.DeptId;

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
                return Json(new { state = 0, msg = ex.Message + ex.TargetSite });
                throw;
            }

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
            FTPHelper ftp = new FTPHelper(FtpServer, "workGuide/" + Dir, FtpUser, FtpPassWord);

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
                FTPHelper ftp = new FTPHelper(FtpServer, "workGuide/" + Dir, FtpUser, FtpPassWord);

                FileInfo file2 = new FileInfo(file.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.Id.ToString() + file2.Extension;

                ftp.Upload(file, fileName);

                string imgUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
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
                WorkGuideEntity notice = db.WorkGuideTable.Find(id);
                db.Delete<WorkGuideEntity>(notice);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult View(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SuggestionEntity sugg = db.SuggestionTable.Find(id);
                return View(sugg);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(SuggestionEntity entity)
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