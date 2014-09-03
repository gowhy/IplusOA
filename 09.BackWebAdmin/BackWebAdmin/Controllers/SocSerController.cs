﻿using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "社会服务管理")]
    public class SocSerController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int pageSize = 20;
        //
        [SecurityNode(Name = "已发布社会服务")]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            IplusOADBContext db = new IplusOADBContext();
            var list = db.SocServiceDetailEntityTable.AsQueryable().ToList();
            return View(list.ToPagedList(pageNumber - 1, pageSize));
        }

        /// <summary>
        /// 管理员查看自己所属社区活动
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [SecurityNode(Name = "社区管理员-本社区服务内容")]
        public ActionResult ManagerIndex(int? page)
        {
            var pageNumber = page ?? 1;
            string depId = AdminUser.DeptId.ToString();
            IplusOADBContext db = new IplusOADBContext();
            var list = db.SocServiceDetailEntityTable.AsQueryable().Where(x => x.CoverCommunity.IndexOf(depId) != -1).ToList();
            return View(list.ToPagedList(pageNumber - 1, pageSize));
        }

        [SecurityNode(Name = "发布社区服务内容")]
        public ActionResult Add()
        {


            IplusOADBContext db = new IplusOADBContext();
            var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();

            ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list); ;

            return View();
        }

        [SecurityNode(Name = "保存社区服务内容")]
        /// <summary>
        /// 发布社会服务内容详细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult PostAdd(SocServiceDetailEntity entity)
        {
            entity.AddTime = DateTime.Now;
            entity.AddUser = AdminUser.UserName;

            IplusOADBContext db = new IplusOADBContext();
            SocialOrgEntity soc = db.SocialOrgEntityTable.SingleOrDefault(x => x.Id == AdminUser.SocOrgId);
            entity.SocialNo = soc.SocialNO;

            entity.SerNum = entity.Type + DateTime.Now.ToString("yyyyMMdd") + (db.SocServiceDetailEntityTable.Max(x => x.Id) + 1).ToString("D2");
            db.Add<SocServiceDetailEntity>(entity);
            db.SaveChanges();
            db.Dispose();

            return Success("添加成功");
        }
        [SecurityNode(Name = "修改社区服务内容")]
        public ActionResult Edit(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
            var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();

            IList<SocSerDetailJoinEntity> joinList = db.SocSerDetailJoinEntityTable.AsQueryable().Where(x => x.SSDetailId == id && x.DepId == AdminUser.DeptId && x.State != 1).ToList();
            //  joinList.FirstOrDefault()
            db.Dispose();

            string CoverCommunityNames = "";
            IList<string> clit = entity.CoverCommunity.Trim().Split(',');
            for (int i = 0; i < list.Count; i++)
            {
                if (clit.Contains(list[i].Id.ToString()) && (joinList.FirstOrDefault(x => x.Id == list[i].Id) == null))
                {
                    list[i].IsCheck = true;
                    CoverCommunityNames += list[i].Name + ",";
                }
            }

            entity.CoverCommunityNames = CoverCommunityNames;
            ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);

            return View(entity);
        }

        [SecurityNode(Name = "保存修改社区服务内容")]
        public ActionResult PostEdit(SocServiceDetailEntity entity)
        {
            IplusOADBContext db = new IplusOADBContext();

            db.Update<SocServiceDetailEntity>(entity);

            db.SaveChanges();
            db.Dispose();
            return Success("修改成功");

        }
        [SecurityNode(Name = "本社区不参与服务")]
        public ActionResult NoTakeIn(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);

            SocSerDetailJoinEntity en = db.SocSerDetailJoinEntityTable.SingleOrDefault(x => x.SSDetailId == id && x.DepId == AdminUser.DeptId);
            if (en != null && en.State == -1)
            {
                ViewBag.IsJoin = "不参与";
            }
            else if (en != null && en.State == 0)
            {
                ViewBag.IsJoin = "暂停";
            }
            else
            {
                ViewBag.IsJoin = "参与";
            }
            if (en != null)
            {
                ViewBag.Msg = en.Msg;
            }
            return View(entity);
        }
        [SecurityNode(Name = "本社区不参与服务保存")]
        [HttpPost]
        public ActionResult PostNoTakeIn(int detailId, int state, string msg)
        {
            IplusOADBContext db = new IplusOADBContext();
            //SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
            SocSerDetailJoinEntity en = db.SocSerDetailJoinEntityTable.SingleOrDefault(x => x.SSDetailId == detailId && x.DepId == AdminUser.DeptId);
            if (en == null)//新增
            {
                SocSerDetailJoinEntity Se = new SocSerDetailJoinEntity();
                Se.DepId = AdminUser.DeptId;
                Se.SSDetailId = detailId;
                Se.State = state;
                Se.Msg = msg;
                db.Add(Se);//增加不参与的活动
            }
            else
            {
                en.State = state;
                en.Msg = msg;
                db.Update(en);//存在更新状态
            }

            db.Dispose();

            return Success("操作成功");
        }

        [SecurityNode(Name = "删除社区服务")]
        public ActionResult Delete(int id)
        {
            IplusOADBContext db = new IplusOADBContext();
            SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
            db.Delete<SocServiceDetailEntity>(entity);
            db.SaveChanges();
            return Success("操作成功");

        }
    }
}