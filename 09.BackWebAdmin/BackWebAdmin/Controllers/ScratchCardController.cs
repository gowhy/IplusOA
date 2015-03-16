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
    [SecurityModule(Name = "刮刮卡")]
    public class ScratchCardControlle : BaseController
    {
        const int pageSize = 100;
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page)
        {
            return View();
       
        }
        public ActionResult IndexData(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var sc = db.ScratchCardTable;
                var list = sc;
                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, pageSize), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                ScratchCard entity = db.ScratchCardTable.Find(id);
                return View(entity);
            }

        }
        [ValidateInput(false)]
        public ActionResult PostEdit(ScratchCard entity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                ScratchCard model = db.ScratchCardTable.Find(entity.Id);
                model.Rate = entity.Score;
            }
            return Success("修改成功");

        }
    }
}