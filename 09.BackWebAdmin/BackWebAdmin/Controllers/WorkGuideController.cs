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
    [SecurityModule(Name = "工作指南")]
    public class WorkGuideController : BaseController
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

                var work = db.WorkGuideTable;

                var list = from w in work select w;

                list = list.Where(x => x.DepId == bUser.DeptId);

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var work = db.WorkGuideTable;

                var list = from w in work select w;
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size),JsonRequestBehavior.AllowGet);
            }
        }
        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
            }

            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAdd(WorkGuideEntity entity)
        {
            BackAdminUser bUser = this.GetBackUserInfo();
            entity.DepId = bUser.DeptId;
            entity.AddUser = bUser.UserName;

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
                WorkGuideEntity work = db.WorkGuideTable.Find(id);
                return View(work);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(WorkGuideEntity entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                WorkGuideEntity work = db.WorkGuideTable.Find(entity.Id);

                work.AddTime = DateTime.Now;
                work.Des = entity.Des;
                work.AddUser = entity.AddUser;
                work.ImgUrl = entity.ImgUrl;
                work.Title = entity.Title;
                work.LinkSocSerUrl = entity.LinkSocSerUrl;
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
                WorkGuideEntity work = db.WorkGuideTable.Find(id);
                return Json(work);
            }
        }
        /// <summary>
        /// App推送消息标识
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PushMsg(int id)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                WorkGuideEntity notice = db.WorkGuideTable.Find(id);
                if (notice.State>=1)
                {
                      return Json("已经推送,不能再推送");
                }
                notice.State = 1;

                db.Update<WorkGuideEntity>(notice);
                db.SaveChanges();
                db.Dispose();
                return Json("操作成功");
            }
        }

        public ActionResult SaveUploadHtmlFile()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];


            SocSerImgEntity res = UploadFile.SaveFile(file, "WorkGuidHtmlFile", "");

            return Json(res);
        }

        #region 测试能适配多屏（基于Bootstrap）的文本编辑器Summernote
        public ActionResult AddBootstrap()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
            }

            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAddBootstrap(WorkGuideEntity entity)
        {
            BackAdminUser bUser = this.GetBackUserInfo();
            entity.DepId = bUser.DeptId;
            entity.AddUser = bUser.UserName;

            using (IplusOADBContext db = new IplusOADBContext())
            {
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }
        #endregion
    }
}