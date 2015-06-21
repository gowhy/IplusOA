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
using DataLayer.IplusOADB;
using IplusOAEntity;

namespace BackWebAdmin.Controllers
{
      [SecurityModule(Name = "服务类型管理")]
    public class SerTypeController : BaseController
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
                var app = db.SerTypesTable;

                var list = from a in app select a;

                return View(list.OrderBy(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex()
        {
            var pageNumber = 1;
            var size =1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var adimg = db.SerTypesTable;

                var list = from a in adimg select a;

                list = list.Where(x=>x.Id>0);

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size),JsonRequestBehavior.AllowGet);
            }
        }


        [SecurityNode(Name = "新增")]
        public ActionResult Add()
        {
        
            return View();
        }

        public ActionResult PostAdd(SerTypes entity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                db.Add(entity);
                db.SaveChanges();
                
            }
            return Success("添加成功");
        }

    

        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                AppVerEntity img = db.AppVerTable.Find(id);
                db.Delete<AppVerEntity>(img);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

    
	}
}