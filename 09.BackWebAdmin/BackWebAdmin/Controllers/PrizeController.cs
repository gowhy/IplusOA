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
    [SecurityModule(Name = "社区奖品")]
    public class PrizeController : BaseController
    {
        private static int PageSize = 20;
        [SecurityNode(Name = "首页")]
        // GET: Prize
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            BackAdminUser bUser = this.GetBackUserInfo();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var pt = db.PrizeTable;
                var plist = from p in pt where p.DeptId == bUser.DeptId select p;

                return View(plist.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));

            }
        }

        public ActionResult Add()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult PostAdd(Prize entity)
        {
            entity.DeptId = AdminUser.DeptId;
            entity.AddUserId = AdminUser.Id;
            entity.AddTime = DateTime.Now;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");

        }
        public ActionResult Edit()
        {

            return View();
        }
        [ValidateInput(false)]
        public ActionResult PostEdit(Prize entity)
        {
            entity.DeptId = AdminUser.DeptId;
            entity.AddUserId = AdminUser.Id;

            using (IplusOADBContext db = new IplusOADBContext())
            {
                entity.AddTime = DateTime.Now;
                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");

        }
        public ActionResult Delete()
        {
            return View();
        }
        public PartialViewResult PrizeRecord(int userId)
        {
            PrizeRecord r = new PrizeRecord();
            r.UserId = userId;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var tb = db.PrizeTable;
                var list = from p in tb where p.DeptId == AdminUser.DeptId && p.StartTime < DateTime.Now && p.EndTime > DateTime.Now select p;

                ViewData["PrizeTable_list"] = list.ToList();

            }
            return PartialView(r);
        }

        public ActionResult PostPrizeRecord(PrizeRecord entity)
        {

            entity.AddUserId = AdminUser.Id;
            entity.AddTime = DateTime.Now;

            if (entity.AprizeId < 1)
            {
                return Error("奖品不存在");
            }

            using (IplusOADBContext db = new IplusOADBContext())
            {
                var tb = db.PrizeRecordTable;

                if (tb.Count(x => x.AprizeId == entity.AprizeId && x.UserId == entity.UserId) > 0)
                {

                    return Error("该用户已经领取过该奖品,请领取其他奖品");
                }

                db.Add(entity);
                db.SaveChanges();

            }
            return Success("领奖成功");

        }


        public ViewResult PrizeRecordIndex(int userId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

           // ShowPrizeRecordModel
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var prize = db.PrizeTable;
                var vol = db.VolunteerEntityTable;
                var prizeRecord = db.PrizeRecordTable;

                var list = from pr in prizeRecord
                           join p in prize on pr.AprizeId equals p.Id
                           join v in vol on pr.UserId equals v.Id
                           where pr.UserId==userId
                           select new ShowPrizeRecordModel { 
                            Prize=p,
                            PrizeRecord=pr,
                            VolunteerEntity=v

                           };

                //var list = from p in tb where p.UserId == userId select p;

                return View(list.OrderBy(x => x.PrizeRecord.Id).ToPagedList(pageNumber - 1, size));

            }

        }
    }
}