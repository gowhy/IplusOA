using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.IO;
using ServiceAPI;

namespace BackWebAdmin.Controllers
{
    public class CommController : Controller
    {
        const int pageSize = 20;



        public ActionResult AppGetDeptChild(string id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                if (id != null )
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
        //用户注册
        public ActionResult AppPostAddVol(VolunteerEntity entity)
        {

            if (entity == null || string.IsNullOrEmpty(entity.Type) || string.IsNullOrEmpty(entity.PassWord) && (string.IsNullOrEmpty(entity.VID) && string.IsNullOrEmpty(entity.Phone)))
            {
                Json("登陆账号、密码和用户类型是必填项");
            }
            ReturnModel returnModel = VolService.PostAddVol(entity, Request);
            return Json(returnModel);
        }

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult AccountExist(VolunteerEntity entity)
        {
            ReturnModel returnModel = VolService.AccountExist(entity);
            return Json(returnModel);
        }

        public ActionResult AppVolHeadImg(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                byte[] image = (from c in db.VolunteerEntityTable where c.Id == id select c.VolHeadImg).FirstOrDefault<byte[]>();
                return new FileContentResult(image, "image/jpeg");
            }
        }

    }
}