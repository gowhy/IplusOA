using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace BackWebAdmin.Controllers
{
    public class VolController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int pageSize = 20;
        //
        // GET: /SocSer/
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            IplusOADBContext db = new IplusOADBContext();
            var list = db.VolunteerEntityTable.AsQueryable().ToList();
            return View(list.ToPagedList(pageNumber-1,pageSize));
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult PostAdd(VolunteerEntity entity)
        {
            IplusOADBContext db = new IplusOADBContext();
            db.Add<VolunteerEntity>(entity);
            db.SaveChanges();
            db.Dispose();
            return Success("添加成功");


        }
        public ActionResult Edit(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            VolunteerEntity entity = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == id);
           return View(entity);
 
        }

        public ActionResult PostEdit(VolunteerEntity entity)
        {
            IplusOADBContext db = new IplusOADBContext();
            VolunteerEntity model = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == entity.Id);
            model.State = entity.State;
            model.Msg = entity.Msg;
            db.Update<VolunteerEntity>(model);
            db.SaveChanges();
            db.Dispose();
            return Success("修改成功");
        }

        public ActionResult View(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            VolunteerEntity entity = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == id);
           return View(entity);
 
        }
        
        public ActionResult Delete(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            VolunteerEntity vol = db.VolunteerEntityTable.Find(id);
            db.Delete<VolunteerEntity>(vol);
            db.SaveChanges();
            db.Dispose();
            return View();
        }
        public ActionResult Audit(VolunteerEntity entity)
        {
            IplusOADBContext db = new IplusOADBContext();
            db.Update<VolunteerEntity>(entity);
            db.SaveChanges();
            db.Dispose();
            return Success("修改成功");
        }

	}
}