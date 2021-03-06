﻿using BackWebAdmin.Models;
using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ServiceAPI;

namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "社会组织管理")]
    /// <summary>
    /// 社会组织控制器
    /// </summary>
    public class SOrgController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int pageSize = 20;


        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page, SelectSorgModel model)
        {
            var pageNumber = page ?? 1;

            var filter = PredicateExtensionses.True<SocialOrgEntity>();

            if (!string.IsNullOrWhiteSpace(model.Type)) filter = filter.And(x => x.Type == model.Type.Trim());
            if (!string.IsNullOrWhiteSpace(model.Name)) filter = filter.And(x => x.Name.Contains(model.Name.Trim()));
            if (!string.IsNullOrWhiteSpace(model.RegNO)) filter = filter.And(x => x.RegNO == model.RegNO.Trim());
            if (!string.IsNullOrWhiteSpace(model.SocialNO)) filter = filter.And(x => x.SocialNO == model.SocialNO.Trim());

            model.SocOrgList = SorgSercice.CList(pageNumber, pageSize, filter);

            return View(model);
        }

        [SecurityNode(Name = "APP获取社会组织")]
        public ActionResult AppGetOrg(int? page, int? pageSize, SelectSorgModel model)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? SOrgController.pageSize;
            //using (IplusOADBContext db = new IplusOADBContext())
            //{
            //    var list = db.SocialOrgEntityTable.AsQueryable<SocialOrgEntity>().ToList();
            //    return Json(list.ToPagedList(pageNumber - 1, size));
            //}

            var filter = PredicateExtensionses.True<SocialOrgEntity>();

            if (!string.IsNullOrWhiteSpace(model.Type)) filter = filter.And(x => x.Type == model.Type);
            if (!string.IsNullOrWhiteSpace(model.Name)) filter = filter.And(x => x.Name.Contains(model.Name));
            if (!string.IsNullOrWhiteSpace(model.RegNO)) filter = filter.And(x => x.RegNO == model.RegNO);
            if (!string.IsNullOrWhiteSpace(model.SocialNO)) filter = filter.And(x => x.SocialNO == model.SocialNO);

            model.SocOrgList = SorgSercice.CList(pageNumber, size, filter);
            return Json(model.SocOrgList);
        }



        [SecurityNode(Name = "添加页")]
        public ActionResult Add()
        {
            return View();
        }
        [SecurityNode(Name = "保存添加")]
        [HttpPost]
        public ActionResult PostAdd(SocialOrgEntity entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                entity.SocialNO = entity.Type + DateTime.Now.ToString("yyyyMMdd") + (db.SocialOrgEntityTable.Max(x => x.Id) + 1).ToString("D2");
                db.Add<SocialOrgEntity>(entity);
                db.SaveChanges();
                db.Dispose();
            }
            return Success("操作成功");
        }

        [SecurityNode(Name = "编辑")]
        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SocialOrgEntity entity = db.SocialOrgEntityTable.Find(id);
                db.Dispose();
                return View(entity);
            }
        }
        [SecurityNode(Name = "保存编辑")]
        public ActionResult PostEdit(SocialOrgEntity entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SocialOrgEntity so = db.SocialOrgEntityTable.Single(x => x.Id == entity.Id);

                so.Name = entity.Name;
                so.OrgNo = entity.OrgNo;
                so.RegNO = entity.RegNO;
                so.RegType = entity.RegType;
                so.BusDesc = entity.BusDesc;
                so.EffectiveTime = entity.EffectiveTime;
                so.RegTime = entity.RegTime;
                so.State = entity.State;

                db.Update<SocialOrgEntity>(so);
                db.SaveChanges();
                db.Dispose();
                return Success("修改成功");
            }
        }

        [SecurityNode(Name = "删除社会组织")]
        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SocialOrgEntity entity = db.SocialOrgEntityTable.Find(id);
                db.Delete<SocialOrgEntity>(entity);
                db.SaveChanges();
                return Success("操作成功");
            }
        }
    }
}