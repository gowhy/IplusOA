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
using DataLayer.IplusOADB;


namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "兑换商品管理")]
    public class ChangeGoodsController : BaseController
    {
        const int pageSize = 20;
        [SecurityNode(Name = "商品列表")]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var cg = db.ChangeGoodsTable;


                var list = from c in cg select c;

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, pageSize));
            }
        }

        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
            }

            return View();
        }


        public ActionResult PostAdd(ChangeGoods entity)
        {
            BackAdminUser bUser = this.GetBackUserInfo();
           
            entity.AddUserId = bUser.Id;
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


        public ActionResult PostEdit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult View(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var cg = db.ChangeGoodsTable;
               return View(cg.Find(id));
            }

          
        }

        /// <summary>
        /// App获取商品详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AppView(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var cg = db.ChangeGoodsTable;
                return Json(cg.Find(id));
            }
        }

        /// <summary>
        /// App兑换商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AppChangeGoodsAction(ChangeGoodsLog entity )
        {
            entity.AddTime = DateTime.Now;
            if (entity.ChangeCount<1)
            {
                entity.ChangeCount = 1;
            }
            AppReturnModel ret = new AppReturnModel();
     
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var vol = db.VolunteerEntityTable.Find(entity.UserId);
                var cg = db.ChangeGoodsTable.Find(entity.ChangeGoodsId);

                if (cg.Count<=0)
                {
                    ret.State = 0;
                    ret.Msg = "商品库存不足,暂时不能兑换";
                    return Json(ret);
                }

                if (vol.Score < cg.Score)
                {
                    ret.State = 0;
                    ret.Msg = "积分不足,当前可用积分:" + vol.Score + "; 商品所需积分:" + cg.Score;
                    return Json(ret);
                }

                //减掉用户积分
                vol.Score = vol.Score - cg.Score *entity.ChangeCount ;
                db.Update(vol);
                db.SaveChanges();

                //修改库存
                cg.Count = cg.Count - 1 * entity.ChangeCount;
                db.Update(cg);
                db.SaveChanges();

                //添加兑换记录
                db.Add(entity);
                db.SaveChanges();
                ret.State = 1;
                ret.Msg = "兑换成功";
                return Json(ret);
            }
        }

        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];


            SocSerImgEntity res = UploadFile.SaveFile(file, "GhangeGoods", "");

            return Json(res);
        }

        /// <summary>
        /// 兑换记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ChangeGoodsLogIndex(int? page,int? cGoodsId)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var cgl = db.ChangeGoodsLogTable;
                var cg = db.ChangeGoodsTable;
                var vol = db.VolunteerEntityTable;

                var list = from cl in cgl
                           join c in cg on cl.ChangeGoodsId equals c.Id
                           join v in vol on cl.UserId equals v.Id
                           select new ChangeGoodsLogModel
                           {  
                            CGoodsLogs=cl,
                            User=v,
                            CGoods=c
                           };
                if (cGoodsId.HasValue)
                {
                    list = list.Where(x=>x.CGoodsLogs.ChangeGoodsId==cGoodsId);
                }
                return View(list.OrderByDescending(x => x.CGoodsLogs.Id).ToPagedList(pageNumber - 1, pageSize));
            }
        }

        public ActionResult AppGetChangeGoodsByBarCode(string barCode) 
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var cg = db.ChangeGoodsTable;
                return Json(cg.FirstOrDefault(x=>x.Barcode==barCode));
            }
        }

        /// <summary>
        /// 兑换记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult AppChangeGoodsLogIndex(int? userId,int? page, int? cGoodsId)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var cgl = db.ChangeGoodsLogTable;
                var cg = db.ChangeGoodsTable;
                var vol = db.VolunteerEntityTable;

                var list = from cl in cgl
                           join c in cg on cl.ChangeGoodsId equals c.Id
                           into clc
                           from c2  in clc.DefaultIfEmpty()
                           join v in vol on   cl.UserId equals v.Id into cv
                           from cv2 in cv.DefaultIfEmpty()
                           select new ChangeGoodsLogModel
                           {
                               CGoodsLogs = cl,
                               //User = cv2, 
                               CGoods = c2
                           };
                if (cGoodsId.HasValue)
                {
                    list = list.Where(x => x.CGoodsLogs.ChangeGoodsId == cGoodsId);
                }
                if (userId.HasValue)
                {
                    list = list.Where(x => x.CGoodsLogs.UserId == userId);
                }
                return Json(list.OrderByDescending(x => x.CGoodsLogs.Id).ToPagedList(pageNumber - 1, pageSize),JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AppIndex(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var cg = db.ChangeGoodsTable;


                var list = from c in cg where c.Count>0 select c ;

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, pageSize));
            }
        }
    }
}