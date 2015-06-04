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
    [SecurityModule(Name = "志愿者风采")]
    public class VolShowController : BaseController
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

                var volS = db.VolShowTable;

                var list = from vs in volS select vs;

                //list = list.Where(x => x.AddUserId == bUser.Id);
                list = list.Where(x => x.DepId == bUser.DeptId);
                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(string deptId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            using (IplusOADBContext db = new IplusOADBContext())
            {

                var volS = db.VolShowTable;
                var volSHtmlFile = db.VolShowHtmlFileTable;

                var list = from vs in volS
                           select new VolShow_HtmlFile
                           {
                               AddTime = vs.AddTime,
                               AddUserId = vs.AddUserId,
                               DepId = vs.DepId,
                               Des = vs.DepId,
                               Id = vs.Id,
                               ImgUrl = vs.ImgUrl,
                               Name = vs.Name,
                               Phone = vs.Phone,
                               VolType=vs.VolType,
                               UploadHtmlFile = vs.UploadHtmlFile,
                               VolShowHtmlFileList = volSHtmlFile.Where(x=>x.volShowId==vs.Id).ToList()

                           };
                if (string.IsNullOrEmpty(deptId))
                {
                    list = list.Where(x => x.DepId == deptId);
                }
                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size),JsonRequestBehavior.AllowGet);
            }
        }

        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {


            return View();

        }

        [ValidateInput(false)]
        public ActionResult PostAdd(VolShow entity, string volShowIds)
        {
            BackAdminUser bUser = this.GetBackUserInfo();
            entity.DepId = bUser.DeptId;
            entity.AddUserId = bUser.Id;

            using (IplusOADBContext db = new IplusOADBContext())
            {
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(volShowIds))
                {
                    string[] volShowIdArr = volShowIds.Split(',');

                    var Vshf = db.VolShowHtmlFileTable;
                    for (int i = 0; i < volShowIdArr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(volShowIdArr[i]))
                        {
                            var temp = Vshf.Find(int.Parse(volShowIdArr[i]));
                            temp.volShowId = entity.Id;
                            db.Update(temp);
                            db.SaveChanges();
                        }

                    }
                }
              

                return Success("添加成功");
            }

        }
        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolShow v = db.VolShowTable.Find(id);
                db.Delete<VolShow>(v);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolShow v = db.VolShowTable.Find(id);
                return View(v);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(VolShow entity, string volShowIds)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolShow work = db.VolShowTable.Find(entity.Id);

                // work.AddTime = DateTime.Now;
                work.Des = entity.Des;
                work.Name = entity.Name;
                work.ImgUrl = entity.ImgUrl;
                work.Phone = entity.Phone;
                work.UploadHtmlFile = entity.UploadHtmlFile;
                db.Update(work);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult SaveUploadHtmlFile()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];


            SocSerImgEntity res = UploadFile.SaveFile(file, "VolShowHtml", "");

            return Json(res);
        }

        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];

            //积分商城
            SocSerImgEntity res = UploadFile.SaveFile(file, "volShowPic", "");

            return Json(res);
        }

        public ActionResult SaveUploadVolHtmlFile()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];


            SocSerImgEntity res = UploadFile.SaveFile(file, "VolHtmlFile", "");

            //VolShowHtmlFile m = new VolShowHtmlFile();
            //m.AddTime = DateTime.Now;
            //m.HtmlTitle

            return Json(res);
        }

        public ActionResult SaveUploadVolHtmlFileTable(VolShowHtmlFile model)
        {
            model.AddTime = DateTime.Now;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                //接收上传后的文件
                db.Add<VolShowHtmlFile>(model);
                db.SaveChanges();
              
                return Json(model);

            }
        }
    }
}