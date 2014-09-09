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
    [SecurityModule(Name = "自愿者管理")]
    public class VolController : BaseController
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        const int pageSize = 20;
        [SecurityNode(Name = "首页")]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.VolunteerEntityTable.AsQueryable().Where(x => x.State == 1).ToList();
                return View(list.ToPagedList(pageNumber - 1, pageSize));
            }
        }
        [SecurityNode(Name = "新增页")]
        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //部门组织
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);

                return View();
            }
        }

        [SecurityNode(Name = "保存新增")]
        public ActionResult PostAdd(VolunteerEntity entity)
        {

            entity.State = 0;//待审核
            entity.Doing = 1;//默认接受任务
           // entity.PassWord = "000000";//自愿者默认密码
            //model.Score = entity.Score;
            entity.Score = 0;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                SocialOrgEntity so = db.SocialOrgEntityTable.Single(x => x.Id == AdminUser.SocOrgId);
                entity.SocialNO = so.SocialNO;

                db.Add<VolunteerEntity>(entity);
                db.SaveChanges();
                db.Dispose();
                return Success("添加成功");
            }
        }

        [SecurityNode(Name = "编辑")]
        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolunteerEntity entity = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == id);
                return View(entity);

            }
        }
        [SecurityNode(Name = "保存编辑")]
        public ActionResult PostEdit(VolunteerEntity entity)
        {
            IplusOADBContext db = new IplusOADBContext();
            VolunteerEntity model = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == entity.Id);

            model.Address = entity.Address;
            model.CardNum = entity.CardNum;
            model.CardType = entity.CardType;
            model.DepId = entity.DepId;
            model.EMail = entity.EMail;
            model.GroupID = entity.GroupID;
            model.Honor = entity.Honor;

            model.Number = entity.Number;
            model.Phone = entity.Phone;
            model.QQ = entity.QQ;
            model.RealName = entity.RealName;
            model.RealNameState = entity.RealNameState;
            //model.Score = entity.Score;
            model.Score =0;
            model.SocialNO = entity.SocialNO;
            model.ThsScore = entity.ThsScore;
            model.Type = entity.Type;
            model.UerName = entity.UerName;
            model.VID = entity.VID;
            model.WeiXin = entity.WeiXin;

            db.Update<VolunteerEntity>(model);
            db.SaveChanges();
            db.Dispose();
            return Success("修改成功");
        }

        [SecurityNode(Name = "待审核列表")]
        public ActionResult AuditIndex(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.VolunteerEntityTable.AsQueryable().Where(x => x.State == 0).ToList();
                return View(list.ToPagedList(pageNumber - 1, pageSize));
            }
        }
        [SecurityNode(Name = "审核页")]
        /// <summary>
        /// 自愿者审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult Audit(int id)
        {
            VolunteerEntity entity = null;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //部门组织
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);

                entity = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == id);
            }
            return View(entity);

        }

        [SecurityNode(Name = "保存审核")]
        /// <summary>
        /// 自愿者审核提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostAudit(VolunteerEntity entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolunteerEntity model = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == entity.Id);

                model.State = entity.State;
                model.Msg = entity.Msg;

                db.Update<VolunteerEntity>(model);
                db.SaveChanges();
                db.Dispose();

                return Success("操作执行成功");

            }
        }
        [SecurityNode(Name = "查看自愿者")]
        public ActionResult View(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolunteerEntity entity = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == id);
                return View(entity);

            }
        }
        [SecurityNode(Name = "删除自愿者")]
        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolunteerEntity vol = db.VolunteerEntityTable.Find(id);
                db.Delete<VolunteerEntity>(vol);
                db.SaveChanges();
                db.Dispose();
                return View();
            }
        }


    }
}