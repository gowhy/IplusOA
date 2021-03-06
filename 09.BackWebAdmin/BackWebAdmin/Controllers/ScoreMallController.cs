﻿using DataLayer.IplusOADB;
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
    [SecurityModule(Name = "积分商城")]
    public class ScoreMallController : BaseController
    {
        private static int PageSize = 20;
        //
        // GET: /StartAdImg/
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

            BackAdminUser bUser = this.GetBackUserInfo();

            using (IplusOADBContext db = new IplusOADBContext())
            {

                var smt = db.ScoreMallTable;

                var list = from s in smt select s;

                list = list.Where(x => x.AddUserId == bUser.Id);

                return View(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult AppIndex(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? PageSize;

        
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var smt = db.ScoreMallTable;

                var list = from s in smt select s;

                list = list.Where(x => x.StartTime<DateTime.Now&&x.EndTime>DateTime.Now);

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        /// <summary>
        /// 兑换商品
        /// 参数scoremallId UserId
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult AppPostScoreMallChange(ScoreMallChangeLog entity)
        {
            entity.AddTime = DateTime.Now;

            AppReturnModel returnModel = new AppReturnModel();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //ScoreMall tmpScoreMallEntity = from s in db.ScoreMallTable where s.Id == entity.ScoreMallId && DateTime.Now > s.StartTime && s.EndTime > DateTime.Now select s;

                ScoreMall tmpScoreMallEntity = db.ScoreMallTable.FirstOrDefault(s => s.Id == entity.ScoreMallId && DateTime.Now > s.StartTime && s.EndTime > DateTime.Now);
                if (tmpScoreMallEntity == null )//
                {
                    returnModel.State = 0;//失败
                    returnModel.Msg = "商品已经下架";
                    return Json(returnModel);
                }

                if (tmpScoreMallEntity != null && tmpScoreMallEntity.Count > 0)//没有库存了
                {
                    returnModel.State = 0;//失败
                    returnModel.Msg = "无库存";
                    return Json(returnModel);
                }
               
                //添加兑换记录
                entity.UseScore = tmpScoreMallEntity.UseScore;
                db.Add<ScoreMallChangeLog>(entity);
                db.SaveChanges();

                //降低库存
                tmpScoreMallEntity.Count = tmpScoreMallEntity.Count - 1;
                db.Update(tmpScoreMallEntity);
                db.SaveChanges();

                ///扣除用户的积分
                VolunteerEntity volEntity = db.VolunteerEntityTable.Find(entity.UserId);
                volEntity.Score = volEntity.Score - entity.UseScore;
                db.Update(volEntity);
                db.SaveChanges();

                returnModel.State = 1;//失败
                returnModel.Msg = "兑换成功";
                return Json(returnModel);

            } 
        }
        public ActionResult Add()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult PostAdd(ScoreMall entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                return Success("名称是必填项");
            }
    
            entity.AddUserId = AdminUser.Id;
            entity.AddTime = DateTime.Now;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                db.Add(entity);
                db.SaveChanges();

            }
            return Success("添加成功");

        }
        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                ScoreMall entity = db.ScoreMallTable.Find(id);
                return View(entity);
            }

        }
        [ValidateInput(false)]
        public ActionResult PostEdit(ScoreMall entity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                ScoreMall sm = db.ScoreMallTable.Find(entity.Id);
                sm.Name = entity.Name;
                sm.Count = entity.Count;
                sm.Desc = entity.Desc;
                sm.ImgUrl = entity.ImgUrl;
                sm.Price = entity.Price;
                sm.UseScore = entity.UseScore;
                sm.AddTime = DateTime.Now;
                sm.StartTime = entity.StartTime;
                sm.EndTime = entity.EndTime;
                db.Update<ScoreMall>(sm);
                db.SaveChanges();

            }
            return Success("修改成功");

        }
        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                ScoreMall entity = db.ScoreMallTable.Find(id);
                db.Delete<ScoreMall>(entity);
                db.SaveChanges();
                return Success("删除成功");
            }

        }
        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];

            //积分商城
            SocSerImgEntity res = UploadFile.SaveFile(file, "ScoreMall", "");

            return Json(res);
        }
        public ActionResult ImgFile()
        {
            string action = Request["action"];
            if (action == "config")
            {
                string json = System.IO.File.ReadAllText(HttpContext.Server.MapPath("../config.json"));
                return Content(json);
            }

            UEditorReturnModel ueditorModel = new UEditorReturnModel();
            if (action == "uploadimage")
            {
                var file = Request.Files["upfile"];
                SocSerImgEntity res = UploadFile.SaveFile(file, "ScoreMallEditor", "");

                ueditorModel.url = res.HttpUrl;
                ueditorModel.title = res.Name;
                ueditorModel.original = res.Name;
                ueditorModel.state = UEditorReturnState.Success;
                return Json(ueditorModel, "text/html");

            }
            ueditorModel.state = UEditorReturnState.Error;
            return Json(ueditorModel, "text/html");

        }
    }
}