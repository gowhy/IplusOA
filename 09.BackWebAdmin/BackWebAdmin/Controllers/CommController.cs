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
    public class CommController : Controller
    {
        const int pageSize = 20;

     
       
        public ActionResult AppGetDeptChild(int? id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                if (id != null && id.HasValue)
                {
                    var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.PId == id).ToList();
                    return Json(list);
                }
                else
                {
                    var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                    return Json(list);
                }
            }
        }
    }
}