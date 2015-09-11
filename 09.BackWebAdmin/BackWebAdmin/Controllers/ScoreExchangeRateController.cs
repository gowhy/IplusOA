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

    [SecurityModule(Name = "积分比例")]
    public class ScoreExchangeRateController : BaseController
    {
        private static int PageSize = 20;

        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            BackAdminUser bUser = this.GetBackUserInfo();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var ser = db.ScoreExchangeRateTable;

                var list = from s in ser
                           join d in dep on s.DepId equals d.Id
                           select new ScoreExchangeRateModel
                           {

                               DepartmentEntity = d,
                             ScoreExchangeRateEntity=s
                           };
                list = list.Where(x => x.ScoreExchangeRateEntity.AddUserDeptId == bUser.DeptId);
                return View(list.OrderByDescending(x => x.ScoreExchangeRateEntity.Id).ToPagedList(pageNumber - 1, size));
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
        public ActionResult PostAdd(ScoreExchangeRate entity)
        {
            entity.AddUserDeptId = AdminUser.DeptId;
            entity.AddUserId = AdminUser.Id;
            entity.AddTime = DateTime.Now;
            //entity.RateBase = 1;

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
                ScoreExchangeRate ser = db.ScoreExchangeRateTable.Find(id);
                db.Delete<ScoreExchangeRate>(ser);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                ScoreExchangeRate ser = db.ScoreExchangeRateTable.Find(id);

                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                list.Find(x => x.Id == ser.DepId).IsCheck = true;
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
            
                return View(ser);
            }
        }

        [ValidateInput(false)]
        public ActionResult PostEdit(ScoreExchangeRate model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                ScoreExchangeRate entity = db.ScoreExchangeRateTable.Find(model.Id);
                entity.Rate = model.Rate;
                entity.DepId = model.DepId;
                db.Update(entity);
                db.SaveChanges();

            }
            return Success("添加成功");
        }
    }
}