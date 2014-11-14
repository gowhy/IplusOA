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
        //开机广告
        public ActionResult StartAdImgAppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var adimg = db.StartAdImgTable;

                var list = from a in adimg select a;

                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        //寻找最上级辖区开机广告
        public ActionResult StartAdImgAppStateIndex(string depId)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var adimg = db.StartAdImgTable;

                var depList = dep.ToList();
                List<StartAdImgEntity> list = adimg.ToList();
                StartAdImgEntity resultEntity = StartAdImgService.GetAppStateIndex(depId, depList, list);
                return Json(resultEntity, JsonRequestBehavior.AllowGet);
            }

        }

        //社区通知
        public ActionResult NoticeAppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var notice = db.NoticeTable;

                var list = from a in notice
                           join d in dep on a.DepId equals d.Id
                           select new NoticeEntityClone
                           {

                               DepName = d.Name,
                               DepId = a.DepId,
                               ImgUrl = a.ImgUrl,
                               Id = a.Id,
                               Des = a.Des,
                               AddTime = a.AddTime,
                               Title = a.Title
                           };
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult WorkGuideAppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var work = db.WorkGuideTable;

                var list = from w in work select w;
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        /// <summary>
        /// App版本检查接口
        /// </summary>
        /// <returns></returns>
        public ActionResult AppVerAppIndex()
        {
            var pageNumber = 1;
            var size = 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var adimg = db.AppVerTable;

                var list = from a in adimg select a;

                list = list.Where(x => x.State == 0);

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ServiceAdImgAppIndex(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var adimg = db.ServiceAdImgTable;

                var list = from a in adimg select a;

                list = list.Where(x => x.State == 0);

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AppLog(LogEntity model)
        {
            ReturnModel res = new ReturnModel();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                db.Add<LogEntity>(model);
                res.State = 1;
                res.Msg = "成功";
                return Json(res);
            }
        }
    }
}