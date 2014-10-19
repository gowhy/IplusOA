using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using BackWebAdmin.Models;
using ServiceAPI;
using System.IO;

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
        public ActionResult Index(int? page,SelectVolModel model)
        {
            var pageNumber = page ?? 1;

            string depId = AdminUser.DeptId.ToString();

            var filter = PredicateExtensionses.True<VolunteerEntity>();

            if (!string.IsNullOrWhiteSpace(model.RealName)) filter = filter.And(x => x.RealName.Contains(model.RealName.Trim()));
            if (!string.IsNullOrWhiteSpace(model.Phone)) filter = filter.And(x => x.Phone == model.Phone.Trim());
            if (!string.IsNullOrWhiteSpace(model.CardNum)) filter = filter.And(x => x.CardNum == model.CardNum.Trim());
            if (!string.IsNullOrWhiteSpace(model.Type)) filter = filter.And(x => x.Type == model.Type.Trim());

            if (!string.IsNullOrEmpty(depId)) filter = filter.And(x => x.DepId.StartsWith(depId));
         
            model.VolList = VolService.CList(pageNumber, pageSize, filter);
            return View(model);

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

        [SecurityNode(Name = "自愿者注册")]
        public ActionResult AppPostAdd(VolunteerEntity entity)
        {

            entity.State = 0;//待审核
            entity.Doing = 1;//默认接受任务
            entity.Score = 0;
       
            using (IplusOADBContext db = new IplusOADBContext())
            {

                //SocialOrgEntity so = db.SocialOrgEntityTable.Single(x => x.Id == AdminUser.SocOrgId);
                //entity.SocialNO = so.SocialNO;



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
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
                return View(entity);

            }
        }

        [SecurityNode(Name = "查看自愿者信息")]
        public ActionResult AppView(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                VolunteerEntity entity = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == id);
                return Json(entity);

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


        public ActionResult AppPostEdit(VolunteerEntity entity)
        {
            AppReturnModel ret = new AppReturnModel();
            if (entity==null||entity.Id==0)
            {
                ret.State = 0;
                ret.Msg = "用户Id必填";
           
                   return Json(ret);
            }
            IplusOADBContext db = null;
            try
            {
                db = new IplusOADBContext();
                VolunteerEntity model = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == entity.Id);

                if (entity.Address != null) model.Address = entity.Address;
                if (entity.Address != null) model.CardNum = entity.CardNum;
                if (entity.CardType != null) model.CardType = entity.CardType;
                if (entity.DepId!= null) model.DepId = entity.DepId;
                if (entity.EMail != null) model.EMail = entity.EMail;
                if (entity.GroupID != null) model.GroupID = entity.GroupID;
                if (entity.Honor != null) model.Honor = entity.Honor;
                if (entity.Number != null) model.Number = entity.Number;
                if (entity.QQ != null) model.QQ = entity.QQ;
                if (entity.RealName != null) model.RealName = entity.RealName;
                if (entity.RealNameState != null) model.RealNameState = entity.RealNameState;
               // if (entity.Score != null) model.Score = 0;
                if (entity.SocialNO != null) model.SocialNO = entity.SocialNO;
                if (entity.ThsScore != null) model.ThsScore = entity.ThsScore;
                if (entity.Type != null) model.Type = entity.Type;
                if (entity.UerName != null) model.UerName = entity.UerName;
                if (entity.VID != null) model.VID = entity.VID;
                if (entity.WeiXin != null) model.WeiXin = entity.WeiXin;
                if (entity.State.HasValue) model.State = entity.State;

                db.Update<VolunteerEntity>(model);
                db.SaveChanges();

                ret.State = 1;
                ret.Msg = "修改成功";
              
                db.Dispose();
            }
            catch (Exception e)
            {
                ret.State = 0;
                ret.Msg = "修改失败," + e.Message;
              
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }

            }
            return Json(ret);
        }


        public ActionResult AppPostEditHeader(VolunteerEntity entity)
        {
            AppReturnModel ret = new AppReturnModel();
            if (entity == null || entity.Id == 0)
            {
                ret.State = 0;
                ret.Msg = "用户Id必填";

                return Json(ret);
            }
            IplusOADBContext db = null;
          
            HttpRequestBase request = Request;
            Stream imgStream = null;
            if (request.Files.Count > 0)
            {
                try
                {
                    db = new IplusOADBContext();
                    VolunteerEntity model = db.VolunteerEntityTable.FirstOrDefault(x => x.Id == entity.Id);

                    imgStream = request.Files[0].InputStream;
                    int length = (int)imgStream.Length;
                    byte[] image = new byte[length];
                    imgStream.Read(image, 0, length);
                    imgStream.Close();

                    model.VolHeadImg = image;
                    db.Update<VolunteerEntity>(model);
                    db.SaveChanges();
                    ret.State = 1;
                    ret.Msg = "修改成功";
                }
                catch (Exception e)
                {
                    ret.State = 0;
                    ret.Msg = "修改失败," + e.Message;
                    throw e;
                }
                finally
                {
                    if (imgStream != null)
                    {
                        imgStream.Close();
                    }
                }
            }

            return Json(ret);
        }

        [SecurityNode(Name = "待审核列表")]
        public ActionResult AuditIndex(int? page)
        {
            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.VolunteerEntityTable.Where(x => x.State == 0);
                return View(list.ToPagedList(pageNumber - 1, pageSize));
            }
        }
        [SecurityNode(Name = "审核页")]
        /// <summary>
        /// 自愿者审
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
                if ( entity.State.HasValue&& entity.State.Value==1)
                {
                    if (!string.IsNullOrEmpty(entity.VID)) model.VID = entity.VID;
                    if (!string.IsNullOrEmpty(entity.SocialNO)) model.SocialNO = entity.SocialNO;
                    model.Type = "志愿者";
                }

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
                return Success("操作成功");
            }
        }


        public ActionResult AppVolHeadImg(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                byte[] image = (from c in db.VolunteerEntityTable where c.Id == id select c.VolHeadImg).FirstOrDefault<byte[]>();
                return new FileContentResult(image, "image/jpeg");
            }
        }

        //获用户信息
        public ActionResult AppGetVol(VolunteerEntity volentity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                //VolunteerEntity volentity = null;
                if (volentity.Type == "志愿者")
                {
                    volentity = db.VolunteerEntityTable.FirstOrDefault<VolunteerEntity>(x => x.VID == volentity.VID && x.PassWord == volentity.PassWord);
                }
                if (volentity.Type == "普通用户")
                {
                    volentity = db.VolunteerEntityTable.FirstOrDefault<VolunteerEntity>(x => x.Phone == volentity.Phone && x.PassWord == volentity.PassWord);
                }

                if (volentity == null)
                {
                    return Json("用户不存在");
                }

                return Json(volentity);
            }
        }

        //获用户信息通过Id
        public ActionResult AppGetUserById(VolunteerEntity volentity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
          
                volentity = db.VolunteerEntityTable.FirstOrDefault<VolunteerEntity>(x => x.Id==volentity.Id);
                if (volentity == null)
                {
                    return Json("用户不存在");
                }

                return Json(volentity);
            }
        }
      
    }
}