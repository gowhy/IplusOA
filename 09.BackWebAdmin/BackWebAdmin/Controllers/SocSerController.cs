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
        public ActionResult Index(int? page, SelectSocSerModel model, GridSortOptions sort)
        {
            var pageNumber = page ?? 1;
            model.SocSerList = SocSerService.TypeList(pageNumber, pageSize, null, model, sort);
            return View(model);
        }

        /// <summary>
        /// 管理员查看自己所属社区活动
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [SecurityNode(Name = "社区管理员-本社区服务内容")]
        public ActionResult ManagerIndex(int? page, SelectSocSerModel model, GridSortOptions sort)
        {
            var pageNumber = page ?? 1;
            string depId = AdminUser.DeptId.ToString();

            model.SocSerList = SocSerService.TypeList(pageNumber, pageSize, depId, model, sort);
            return View(model);
        }
        [SecurityNode(Name = "App获取用户本社区服务内容")]
        public ActionResult AppIndex(SelectSocSerModel model, GridSortOptions sort, int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            string depId = AdminUser.DeptId.ToString();

            return Json(SocSerService.TypeList(pageNumber, size, depId, model, sort), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AppAdImgServiceIndex(SelectSocSerModel model, GridSortOptions sort, int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            string depId = null;

            return Json(SocSerService.TypeList(pageNumber, size, depId, model, sort), JsonRequestBehavior.AllowGet);
        }

        [SecurityNode(Name = "发布社区服务内容")]
        public ActionResult Add()
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);
                return View();
            }
        }

        [ValidateInput(false)]
        [SecurityNode(Name = "保存社区服务内容")]
        /// <summary>
        /// 发布社会服务内容详细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult PostAdd(SocServiceDetailEntity entity, string SocSerImgId)
        {
            entity.AddTime = DateTime.Now;
            entity.AddUser = AdminUser.UserName;


            IList<string> coverList = entity.CoverCommunity.Split(',');

            List<SocSerDetailJoinEntity> Join = new List<SocSerDetailJoinEntity>();
            using (IplusOADBContext db = new IplusOADBContext())
            {

                SocialOrgEntity soc = db.SocialOrgEntityTable.SingleOrDefault(x => x.Id == AdminUser.SocOrgId);
                entity.SocialNo = soc.SocialNO;

                int number = 0;
                try
                {
                    number = db.SocServiceDetailEntityTable.AsQueryable().Max<SocServiceDetailEntity>(x => x.Id);
                }
                catch (Exception)
                {
                    number = 0;
                }
                entity.SerNum = entity.Type + DateTime.Now.ToString("yyyyMMdd") + (number + 1).ToString("D2");
                db.Add<SocServiceDetailEntity>(entity);
                db.SaveChanges();

                var idimgs = SocSerImgId.Split(',');
                List<int> listIdImg = new List<int>();
                foreach (var item in idimgs)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        listIdImg.Add(int.Parse(item));
                    }

                }

                //List<SocSerImgEntity> simg = db.SocSerImgTable.Where(x => SocSerImgId.Split(',').Contains(x.Id.ToString())).ToList<SocSerImgEntity>();
                List<SocSerImgEntity> simg = db.SocSerImgTable.Where(x => listIdImg.Contains(x.Id)).ToList<SocSerImgEntity>();
                foreach (var item in simg)
                {
                    item.SocSerId = entity.Id;
                    db.Update(item);
                }
                db.SaveChanges();
                db.Dispose();
                return Success("添加成功");
            }
        }

        [SecurityNode(Name = "修改社区服务内容")]
        public ActionResult Edit(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
                GridSortOptions sort = new GridSortOptions();
                SelectSocSerModel model = new SelectSocSerModel();
                model.Id = id;
                SocServiceDetailEntityClone entity = SocSerService.TypeList(1, 1, "0", model, sort).FirstOrDefault();

                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();

                IList<SocSerDetailJoinEntity> joinList = db.SocSerDetailJoinEntityTable.AsQueryable().Where(x => x.SSDetailId == id && x.DepId == AdminUser.DeptId && x.State != 1).ToList();
                db.Dispose();
                string CoverCommunityNames = "";
                IList<string> clit = entity.CoverCommunity.Trim().Split(',');
                for (int i = 0; i < list.Count; i++)
                {
                    if (clit.Contains(list[i].Id) && (joinList.FirstOrDefault(x => x.DepId == list[i].Id) == null))
                    {
                        list[i].IsCheck = true;
                        CoverCommunityNames += list[i].Name + ",";
                    }
                }

                entity.CoverCommunityNames = CoverCommunityNames;
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list);

                return View(entity);
            }

        }
        [SecurityNode(Name = "APP-获取服务内容详情")]
        public ActionResult AppView(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
                return Json(entity);
            }
        }
        [ValidateInput(false)]
        [SecurityNode(Name = "保存修改社区服务内容")]
        public ActionResult PostEdit(SocServiceDetailEntity entity, string SocSerImgId)
        {

            var idimgs = SocSerImgId.Split(',');
            List<int> listIdImg = new List<int>();
            foreach (var item in idimgs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    listIdImg.Add(int.Parse(item));
                }

            }




            using (IplusOADBContext db = new IplusOADBContext())
            {

                //删除
                List<SocSerImgEntity> simgDelete = db.SocSerImgTable.Where(x => x.SocSerId == entity.Id).ToList<SocSerImgEntity>();
                foreach (var item in simgDelete)
                {
                    item.SocSerId = 0;
                    db.Update(item);
                }
                db.SaveChanges();



                List<SocSerImgEntity> simg = db.SocSerImgTable.Where(x => listIdImg.Contains(x.Id)).ToList<SocSerImgEntity>();
                foreach (var item in simg)
                {
                    item.SocSerId = entity.Id;
                    db.Update(item);
                }
                db.SaveChanges();

                SocServiceDetailEntity data = db.SocServiceDetailEntityTable.Find(entity.Id);

                data.Contacts = entity.Contacts;
                data.Context = entity.Context;
                data.CoverCommunity = entity.CoverCommunity;
                data.Desc = entity.Desc;
                data.EndTime = entity.EndTime;
                data.PubTime = entity.PubTime;
                data.Score = entity.Score;
                data.Type = entity.Type;
                data.PayType = entity.PayType;
                data.THSScore = entity.THSScore;
                data.Phone = entity.Phone;
                data.VHelpDesc = entity.VHelpDesc;
                db.Update<SocServiceDetailEntity>(data);
                db.SaveChanges();
                db.Dispose();
                return Success("修改成功");
            }
        }

        [SecurityNode(Name = "本社区不参与服务")]
        public ActionResult NoTakeIn(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
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
        }
        [SecurityNode(Name = "本社区不参与服务保存")]
        [HttpPost]
        public ActionResult PostNoTakeIn(int detailId, int state, string msg)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
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
        }
        [SecurityNode(Name = "删除社区服务")]
        public ActionResult Delete(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
                db.Delete<SocServiceDetailEntity>(entity);
                db.SaveChanges();
                return Success("操作成功");
            }
        }

        public ActionResult UeditConfig()
        {
            string action = Request["action"];
            if (action == "config")
            {
                string json = System.IO.File.ReadAllText(HttpContext.Server.MapPath("../config.json"));
                return Content(json);
            }
            var file = Request.Files[0];
            if (action == "uploadimage")
            {
                string uploadPath =
                   HttpContext.Server.MapPath("UploadImages" + "\\");

                if (file != null)
                {
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    //保存文件
                    file.SaveAs(uploadPath + file.FileName);

                }
                else
                {

                }
            }

            return Json(new
        {
            state = "SUCCESS",
            url = "http://localhost:6055/socser/UploadImages/" + file.FileName,
            title = "title-why",
            original = "original-why",
            error = ""
        }, "text/html");

        }
        public ActionResult SaveImg()
        {

            //接收上传后的文件
            System.Web.HttpPostedFileBase file = Request.Files["Filedata"];

            string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
            string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
            string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
            string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

            string Dir = DateTime.Now.ToString("yyyyMMdd");
            FTPHelper ftp = new FTPHelper(FtpServer, "SocImg/" + Dir, FtpUser, FtpPassWord);

            FileInfo file2 = new FileInfo(file.FileName);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.Id.ToString() + file2.Extension;

            ftp.Upload(file, fileName);

            SocSerImgEntity img = new SocSerImgEntity();

            img.FTPUrl = ftp.FtpURI;
            img.HttpUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;
            img.Name = file.FileName;
            img.Module = "保存服务";
            img.AddTime = DateTime.Now;

            using (IplusOADBContext db = new IplusOADBContext())
            {

                db.SocSerImgTable.Add(img);
                db.SaveChanges();
            }

            return Json(img);
        }

        public ActionResult UserApplyService(UserApplyServiceEntity entity)
        {

            try
            {
                entity.AddTime = DateTime.Now;
                using (IplusOADBContext db = new IplusOADBContext())
                {
                    var table = db.UserApplyServiceTable;

                    if (table.Count(x => x.VolId == entity.VolId && x.SDId == entity.SDId) > 0)
                    {
                        return Json(new { state = 0, msg = "失败,已经申请该服务" });
                    }
                    else
                    {
                        entity.AddTime = DateTime.Now;
                        db.Add(entity);
                        db.SaveChanges();
                        return Json(new { state = 1, msg = "成功" });
                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { state = -1, msg = "接口执行异常：" + ex.Message + ex.TargetSite });
                throw;
            }
        }


        public ActionResult PostUserApplyServiceAuditing(int id, int state, int num, string msg)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                UserApplyServiceEntity entity = db.UserApplyServiceTable.Find(id);

                entity.State = state;
                entity.Num = num;
                entity.Msg = msg;

                db.Update<UserApplyServiceEntity>(entity);
                db.SaveChanges();
                db.Dispose();
                return Success("操作成功");
            }
        }

        public ActionResult UserApplyServiceIndex(int? page,SelectUserApplySerModel selectModel)
        {
            int pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var user = db.BackAdminUserEntityDBSet.FirstOrDefault(x => x.UserName == AdminUser.UserName);



                var apply = db.UserApplyServiceTable;
                var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;

                var list = from a in apply
                           join v in vol on a.VolId equals v.Id
                           join d in detail on a.SDId equals d.Id
                           join r in record on a.Id equals r.UASId
                           //select new { applyEntiy = a, userEntiy = v, vol2 = vol.Where(x => x.Id == r.VId), det = d, record = r, org = sorg.Where(x => x.SocialNO == d.SocialNo) };
                           select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = v, Vol = vol.Where(x => x.Id == r.VId), Detail = d, Record = r, Org = sorg.Where(x => x.SocialNO == d.SocialNo) };
              
                list = list.Where(x => x.Org.FirstOrDefault().Id == user.SocOrgId);
                if (selectModel.Id>0)list = list.Where(x => x.ApplyEntiy.Id ==selectModel.Id);

               
                SelectUserApplySerModel model = new SelectUserApplySerModel();
                model.ApplySerList =list.OrderByDescending(x => x.ApplyEntiy.Id).ToPagedList(pageNumber - 1, pageSize);

                return View(model);

            }
        }

        [SecurityNode(Name = "添加页")]
        public PartialViewResult UserApplyServiceAuditing(int id)
        {
            if (id<=0)
            {
                return PartialView();
            }
            using (IplusOADBContext db = new IplusOADBContext())
            {

                IList<RoleEntity> role = db.RoleTable.AsQueryable<RoleEntity>().ToList();

                ViewData["UserRole"] = role;

                //部门组织
                var list2 = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                ViewData["Department_List"] = HelpSerializer.JSONSerialize<List<DepartmentEntity>>(list2);


                IList<SocialOrgEntity> Slist = db.SocialOrgEntityTable.AsQueryable<SocialOrgEntity>().ToList();
                ViewData["SocialOrg_List"] = Slist;

             //   db.Dispose();


                var user = db.BackAdminUserEntityDBSet.FirstOrDefault(x => x.UserName == AdminUser.UserName);
                var apply = db.UserApplyServiceTable;
                var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;

                var list = from a in apply
                           join v in vol on a.VolId equals v.Id
                           join d in detail on a.SDId equals d.Id
                           join r in record on a.Id equals r.UASId
                        
                           select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = v, Vol = vol.Where(x => x.Id == r.VId), Detail = d, Record = r, Org = sorg.Where(x => x.SocialNO == d.SocialNo) };
                list = list.Where(x => x.Org.FirstOrDefault().Id == user.SocOrgId);
                if (id > 0) list = list.Where(x => x.ApplyEntiy.Id == id);
                return PartialView(list.FirstOrDefault());
            }
        }
    }
}