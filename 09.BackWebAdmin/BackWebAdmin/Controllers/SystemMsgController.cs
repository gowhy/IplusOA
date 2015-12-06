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
    [SecurityModule(Name = "系统消息")]
    public class SystemMsgController : BaseController
    {
        const int pageSize = 20;
        [SecurityNode(Name = "列表")]
        // GET: SystemMsg
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var msg = db.SystemMsgTable;

                var list = from m in msg where m.State == 0 && m.BeginTime < DateTime.Now && m.EndTime > DateTime.Now select m;

                return View(list.ToPagedList(pageNumber - 1, pageSize));
            }

        }

        public PartialViewResult Add()
        {
            return PartialView();
        }

        [ValidateInput(false)]
        public ActionResult PostAdd(SystemMsgEntity entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                entity.AddTime = DateTime.Now;
                db.Add<SystemMsgEntity>(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }

        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SystemMsgEntity msg = db.SystemMsgTable.Find(id);
                db.Delete<SystemMsgEntity>(msg);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }
    }
}