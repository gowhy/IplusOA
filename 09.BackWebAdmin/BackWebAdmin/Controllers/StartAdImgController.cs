﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using BackWebAdmin.CommService;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using BackWebAdmin.Models;
using System.IO;
using DataLayer.IplusOADB;
using IplusOAEntity;

namespace BackWebAdmin.Controllers
{
      [SecurityModule(Name = "App开机广告")]
    public class StartAdImgController : BaseController
    {
        private static int PageSize = 20;
        //
        // GET: /StartAdImg/
        [SecurityNode(Name = "首页")]
        public ActionResult Index( int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var adimg = db.StartAdImgTable;

                var list = from a in adimg
                           join d in dep on a.DepId equals d.Id
                           select new StartAdImgEntityClone
                           {

                               DepName = d.Name,
                               DepId = a.DepId,
                               HttpUrl = a.HttpUrl,
                               Id = a.Id,
                               Des = a.Des,
                               AddTime = a.AddTime,
                               State=a.State
                           };
                return View(list.OrderByDescending(x=>x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
              
                var adimg = db.StartAdImgTable;

                var list = from a in adimg select a;
                          
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }
            
                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        //public ActionResult AppStateIndex(string depId)
        //{

        //    using (IplusOADBContext db = new IplusOADBContext())
        //    {
        //        var dep = db.DepartmentTable;
        //        var adimg = db.StartAdImgTable;

        //        var depList = dep.ToList();
        //        //List<StartAdImgEntity> list = adimg.Where(x => x.State == 0).ToList();
        //        List<StartAdImgEntity> list = adimg.ToList();
        //        StartAdImgEntity resultEntity = GetAppStateIndex(depId, depList, list);


        //        return Json(resultEntity,JsonRequestBehavior.AllowGet);
        //    }
        //}


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

        public ActionResult PostAdd(StartAdImgEntity entity)
        {

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
            FTPHelper ftp = new FTPHelper(FtpServer, "StartAdImg/" + Dir, FtpUser, FtpPassWord);

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

        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                StartAdImgEntity img = db.StartAdImgTable.Find(id);
                db.Delete<StartAdImgEntity>(img);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult ChangeStateAdImg(int id,int state)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                StartAdImgEntity img = db.StartAdImgTable.Find(id);

                img.State = state;
                db.Update<StartAdImgEntity>(img);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }
	}
}