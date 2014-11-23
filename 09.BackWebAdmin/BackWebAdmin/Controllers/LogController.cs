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
    [SecurityModule(Name = "日志")]
    public class LogController : BaseController
    {
        const int pageSize = 20;
        [SecurityNode(Name = "日志列表")]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var log = db.LogTable;

                var list = from l in log select l;

                list = list.OrderByDescending(x => x.Id);

                return View(list.ToPagedList(pageNumber - 1, pageSize));
            }


        }
    }
}