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
    [SecurityModule(Name = "中天社区幸福响当当")]
    public class WishController : BaseController
    {
        const int pageSize = 20;
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var wish = db.WishTable;

                var list = from w in wish select w;

                return View(list.ToPagedList(pageNumber - 1, pageSize));
            }
        }


        public ActionResult AppIndex(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var wish = db.WishTable;

                var list = from w in wish select w;

                return Json(list.ToList(),JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AppGet(int  applyUserId,int wishId)
        {
            ReturnModel res = new ReturnModel();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var wish = db.WishTable;

               Wish wishEntity= wish.Find(wishId);

               wishEntity.ApplyUserId = applyUserId;
               wishEntity.Id = wishId;
               wishEntity.ApplyTime = DateTime.Now;

               res.State = 1;
               res.Msg = "领取成功";

               return Json(res);
            }
        }

        public ActionResult Auditing(int wishId, string msg, int state)
        {
            ReturnModel res = new ReturnModel();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var wish = db.WishTable;

                Wish wishEntity = wish.Find(wishId);

                wishEntity.AuditeTime = DateTime.Now;
                wishEntity.State = state;
                wishEntity.Desc = msg;

                db.Update<Wish>(wishEntity);
                db.SaveChanges();
            
                return Success("审核成功");
            }
        }
    }
}