using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
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
using Common.Dynamic;


namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "商户信息管理")]
    public class SellerController : BaseController
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

                var smt = db.SellerTable;

                var list = from s in smt select s;

                list = list.Where(x => x.AddUserId == bUser.Id);

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(string deptId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

        
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var smt = db.SellerTable;

                var list = from s in smt select s;
                if (!string.IsNullOrEmpty(deptId)) list = list.Where(x => x.DeptId == deptId);
              
                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
                return View();
            }
        }

        [ValidateInput(false)]
        public ActionResult PostAdd(Seller entity)
        {

            entity.AddTime = DateTime.Now;
            entity.AddUserId = base.GetBackUserInfo().Id;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");

        }
        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {      //部门组织
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);

                Seller entity = db.SellerTable.Find(id);
                return View(entity);
            }

        }
        [ValidateInput(false)]
        public ActionResult PostEdit(Seller entity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                Seller sm = db.SellerTable.Find(entity.Id);
                sm.DeptId = entity.DeptId;
                //sm.PassWord = entity.PassWord;
                sm.Phone = entity.Phone;
                sm.SellName = entity.SellName;
                sm.UserName = entity.UserName;
             
         
                db.Update<Seller>(sm);
                db.SaveChanges();

            }
            return Success("修改成功");

        }
        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                Seller entity = db.SellerTable.Find(id);
                db.Delete<Seller>(entity);
                db.SaveChanges();
                return Success("删除成功");
            }

        }
        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];

            //积分商城
            SocSerImgEntity res = UploadFile.SaveFile(file, "ScoreMall", "");

            return Json(res);
        }
        public ActionResult ImgFile()
        {
            string action = Request["action"];
            if (action == "config")
            {
                string json = System.IO.File.ReadAllText(HttpContext.Server.MapPath("../config.json"));
                return Content(json);
            }

            UEditorReturnModel ueditorModel = new UEditorReturnModel();
            if (action == "uploadimage")
            {
                var file = Request.Files["upfile"];
                SocSerImgEntity res = UploadFile.SaveFile(file, "SellerEditor", "");

                ueditorModel.url = res.HttpUrl;
                ueditorModel.title = res.Name;
                ueditorModel.original = res.Name;
                ueditorModel.state = UEditorReturnState.Success;
                return Json(ueditorModel, "text/html");

            }
            ueditorModel.state = UEditorReturnState.Error;
            return Json(ueditorModel, "text/html");

        }
    }
}