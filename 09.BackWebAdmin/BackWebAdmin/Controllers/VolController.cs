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
using System.Data;
using MvcContrib.UI.Grid;

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
        public ActionResult Index(int? page, SelectVolModel model, GridSortOptions sort)
        {
            var pageNumber = page ?? 1;
            BackAdminUser bauEntity = base.GetBackUserInfo();
            string depId = bauEntity.DeptId;

            using (IplusOADBContext db = new IplusOADBContext())
            {
                var vol = db.VolunteerEntityTable;
                var sorg = db.SocialOrgEntityTable;
                SocialOrgEntity soEntity = sorg.Find(bauEntity.SocOrgId);


                //查询社区用户
                IQueryable<VolunteerEntity> listDept = null;
                if (!string.IsNullOrEmpty(depId))
                {
                    listDept = from v in vol select v;
                    listDept = listDept.Where(x => x.DepId == (depId));
                }

                //查询社会组织用户
                IQueryable<VolunteerEntity> listOrg = null;
                if (soEntity !=null&& !string.IsNullOrEmpty(soEntity.SocialNO))
                {
                    listOrg = from v in vol select v;
                    listOrg = listOrg.Where(x => x.SocialNO == soEntity.SocialNO);
                }


                //合并社区的和社会组织的
                IQueryable<VolunteerEntity> listAll = null;
                if (listDept != null && listOrg!=null)
                {
                    listAll = listDept.Union(listOrg);
                }
                else
                {
                    if (listDept == null && listOrg != null)
                    {
                        listAll = listOrg;
                    }
                    if (listOrg == null && listDept != null)
                    {
                        listAll = listDept;
                    }
                }
            


                if (!string.IsNullOrEmpty(model.RealName)) listAll = listAll.Where(x => x.RealName.Contains(model.RealName.Trim()));
                if (!string.IsNullOrEmpty(model.Phone)) listAll = listAll.Where(x => x.Phone == model.Phone.Trim());
                if (!string.IsNullOrEmpty(model.CardNum)) listAll = listAll.Where(x => x.CardNum == model.CardNum.Trim());
                if (!string.IsNullOrEmpty(model.Type)) listAll = listAll.Where(x => x.Type == model.Type.Trim());


                model.VolList = listAll.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, pageSize);

            }

            return View(model);


        }
        [SecurityNode(Name = "新增页")]
        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //部门组织
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);

                return View();
            }
        }

        //[SecurityNode(Name = "保存新增")]
        //public ActionResult PostAdd(VolunteerEntity entity)
        //{

        //    entity.State = 0;//待审核
        //    entity.Doing = 1;//默认接受任务
        //    // entity.PassWord = "000000";//自愿者默认密码
        //    //model.Score = entity.Score;
        //    entity.Score = 0;
        //    using (IplusOADBContext db = new IplusOADBContext())
        //    {


        //        BackAdminUser ba = base.GetBackUserInfo();
        //        SocialOrgEntity so = db.SocialOrgEntityTable.Single(x => x.Id == ba.SocOrgId);
        //        entity.SocialNO = so.SocialNO;

        //        db.Add<VolunteerEntity>(entity);
        //        db.SaveChanges();
        //        db.Dispose();
        //        return Success("添加成功");
        //    }
        //}
        [SecurityNode(Name = "保存新增")]
        public ActionResult PostAdd(VolunteerEntity entity)
        {
            ReturnModel returnModel = new ReturnModel();
            try
            {


                if (entity == null  || string.IsNullOrEmpty(entity.Type) || string.IsNullOrEmpty(entity.PassWord) && (string.IsNullOrEmpty(entity.VID) && string.IsNullOrEmpty(entity.Phone)))
                {
                    Json("登陆账号、密码、验证码和用户类型是必填项", JsonRequestBehavior.AllowGet);
                }

                using (IplusOADBContext db = new IplusOADBContext())
                {

                    returnModel = VolService.AccountExist(entity);
                    if (returnModel == null || returnModel.State != 0)
                    {
                        return Error(returnModel.Msg); 
                    }
                }

                returnModel = VolService.PostAddVol(entity, Request);
                if (returnModel.State==1)
                {
                   return Success(returnModel.Msg);
                }
                else
                {
                    return Error(returnModel.Msg);
                }
                
            }
            catch (Exception ex)
            {
                returnModel.Msg = ex.Message + ex.Source + ex.StackTrace + ex.TargetSite + ex.InnerException;
                returnModel.State = -4;
                return Error(returnModel.Msg);
                throw;
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
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
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
            model.VillDeptId = entity.VillDeptId;
            
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
                if (entity.CardNum != null) model.CardNum = entity.CardNum;
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

                if (entity.Speciality != null) model.Speciality = entity.Speciality;
                if (entity.SerAreas != null) model.SerAreas = entity.SerAreas;

                if (entity.Age >0) model.Age = entity.Age;
                if (entity.Sex != null) model.Sex = entity.Sex;
                if (entity.VillDeptId != null) model.VillDeptId = entity.VillDeptId;
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
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
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
                    model.Score = model.Score + 20;
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
                //VolunteerEntity vol = db.VolunteerEntityTable.Find(id);
                //db.Delete<VolunteerEntity>(vol);
                //db.SaveChanges();
                //db.Dispose();
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
        public ActionResult AppGetUserById(VolunteerEntity volEntity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
          
               // volentity = db.VolunteerEntityTable.FirstOrDefault<VolunteerEntity>(x => x.Id==volentity.Id);

                var vol = from v in db.VolunteerEntityTable
                          where v.Id == volEntity.Id
                          select new VolunteerEntityClone
                          {
                              Address = v.Address,
                              Age = v.Age,
                              Id = v.Id,
                              CardNum = v.CardNum,
                              CardType = v.CardType,
                              DepId = v.DepId,
                              Doing = v.Doing,
                              EMail = v.EMail,
                              GroupID = v.GroupID,
                              Honor = v.Honor,
                              //  LoginState = v.LoginState,
                              Msg = v.Msg,
                              Number = v.Number,
                              Phone = v.Phone,
                              QQ = v.QQ,
                              RealName = v.RealName,
                              Score = v.Score,
                              SerAreas = v.SerAreas,
                              Sex = v.Sex,
                              SocialNO = v.SocialNO,
                              Speciality = v.Speciality,
                              State = v.State,
                              ThsScore = v.ThsScore,
                              Type = v.Type,
                              UerName = v.UerName,
                              VID = v.VID,
                              WeiXin = v.WeiXin,
                              VillDeptId = v.VillDeptId,
                              LoveBankScore = v.LoveBankScore

                          };
                if (vol == null)
                {
                    return Json("用户不存在");
                }
                VolunteerEntityClone tmp = vol.FirstOrDefault();
                return Json(tmp);
            }
        }
        //获用户信息通过Id
        public ActionResult AppGetUserByPhone(VolunteerEntity volentity)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                volentity = db.VolunteerEntityTable.FirstOrDefault<VolunteerEntity>(x => x.Phone == volentity.Phone);
                if (volentity == null)
                {
                    return Json("用户不存在");
                }

                return Json(volentity);
            }
        }
        public PartialViewResult AddVolExcel()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                IList<RoleEntity> role = db.RoleTable.AsQueryable<RoleEntity>().ToList();

                ViewData["UserRole"] = role;

                //部门组织
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);


                IList<SocialOrgEntity> Slist = db.SocialOrgEntityTable.AsQueryable<SocialOrgEntity>().ToList();
                ViewData["SocialOrg_List"] = Slist;

                db.Dispose();
                return PartialView();
            }
        }

        public ActionResult PostAddVolExcel(string socialNO, string deptId)
        {
            HttpPostedFileBase Volfile = Request.Files["volInfoExcel"];

            FileInfo file2 = new FileInfo(Volfile.FileName);
            string FileName = Server.MapPath("..//Tmp") + "//" + AdminUser.Id + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + file2.Extension;
            FileHelper.Upload(Volfile, FileName);
          
            ExcelHelper eh = new ExcelHelper(FileName, "");
            DataTable dt = eh.InputFromExcel();

            List<VolunteerEntity> volList = new List<VolunteerEntity>();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    VolunteerEntity tmp = new VolunteerEntity();
                    tmp.Phone = dt.Rows[i]["手机号"].ToString();


                    if (db.VolunteerEntityTable.Count(x => x.Phone == tmp.Phone) > 0)
                    {
                        return Error("批量导入失败，手机号是：" + tmp.Phone + "、姓名是" + tmp.RealName + "的自愿者已经存在");
                    }
                    tmp.RealName = dt.Rows[i]["姓名"].ToString();

                    tmp.State = 1;
                    tmp.Score = 50;
                    tmp.ThsScore = 20;
                    tmp.Type = "志愿者";
                    tmp.PassWord = "000000";

                    tmp.SocialNO = socialNO;
                    tmp.DepId = deptId;
                    tmp.VID = tmp.Phone;
                    if (volList.Count(x => x.Phone == tmp.Phone) > 0)
                    {
                          return Error("批量导入失败，Excel表格中手机号重复：" + tmp.Phone + "、姓名是" + tmp.RealName + "");

                    }
                    volList.Add(tmp);
                }



                db.VolunteerEntityTable.AddRange(volList);
                db.SaveChanges();
            }

            System.IO.File.Delete(FileName);
            return Success("批量导入成功");
        }
      
    }
}