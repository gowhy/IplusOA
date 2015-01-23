using DataLayer.IplusOADB;
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

            string depId = "";
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var vol = db.VolunteerEntityTable;
                depId = (from v in vol where v.Id == AdminUser.Id select v.DepId).FirstOrDefault();
            }
            model.SocSerList = SocSerService.TypeList(pageNumber, pageSize, depId, model, sort);
            return View(model);
        }
        [SecurityNode(Name = "App获取用户本社区服务内容")]
        public ActionResult AppIndex(SelectSocSerModel model, GridSortOptions sort, int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            string depId ="";
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var vol = db.VolunteerEntityTable;
                depId =( from v in vol where v.Id == AdminUser.Id select v.DepId).FirstOrDefault();

                //LogEntity log = new LogEntity();
                //log.Content = "depId:" + depId + "  AdminUser.Id ：" + AdminUser.Id;
                //log.UserId = AdminUser.Id.ToString();
                //log.AddTime = DateTime.Now;
                //db.Add<LogEntity>(log);
                //db.SaveChanges();
            }


            model.State = 1;//表示待审核的
           
           return Json(SocSerService.TypeListState(pageNumber, size, depId, model, sort), JsonRequestBehavior.AllowGet);
            //return Json(SocSerService.TypeList(pageNumber, size, depId, model, sort), JsonRequestBehavior.AllowGet);
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
                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x=>x.Level<=6).ToList();
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

            if (string.IsNullOrEmpty(entity.CoverCommunity))
            {
                return Error("覆盖社区必须填写");
            }
            IList<string> coverList = entity.CoverCommunity.Split(',');

            List<SocSerDetailJoinEntity> Join = new List<SocSerDetailJoinEntity>();
            BackAdminUser bau = base.GetBackUserInfo();
            using (IplusOADBContext db = new IplusOADBContext())
            {

                SocialOrgEntity soc = db.SocialOrgEntityTable.SingleOrDefault(x => x.Id == bau.SocOrgId);
                if (soc!=null)
                {
                    entity.SocialNo = soc.SocialNO;
                }
               

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
                        if (int.Parse(item)>0)
                        {
                            listIdImg.Add(int.Parse(item));
                        }
                       
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

                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();

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
                if (entity.VolId <= 0 || entity.SDId<=0)
                {
                    return Json(new { state = -1, msg = "VolId或者SDId是必填参数，切必须大于0" });
                }

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
        [SecurityNode(Name = "服务申请审核")]
        public ActionResult UserApplyServiceIndex(int? page, SelectUserApplySerModel selectModel)
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
                           join v in vol on a.VolId equals v.Id into g
                           join d in detail on a.SDId equals d.Id into g2
                           join r in record on a.Id equals r.UASId into g3
                           from gv in g.DefaultIfEmpty()
                           from gd in g2.DefaultIfEmpty()
                           from gr in g3.DefaultIfEmpty()
                           //select new { applyEntiy = a, userEntiy = v, vol2 = vol.Where(x => x.Id == r.VId), det = d, record = r, org = sorg.Where(x => x.SocialNO == d.SocialNo) };
                           //select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = v, Detail = d, Org = sorg.Where(x => x.SocialNO == d.SocialNo) };
                           select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = gv, Vol = vol.Where(x => x.Id == gr.VId), Detail = gd, Record = gr, Org = sorg.Where(x => x.SocialNO == gd.SocialNo) };
                list = list.Where(x => x.Org.FirstOrDefault().Id == user.SocOrgId);
                if (selectModel.Id > 0) list = list.Where(x => x.ApplyEntiy.Id == selectModel.Id);


                SelectUserApplySerModel model = new SelectUserApplySerModel();
                model.ApplySerList = list.OrderByDescending(x => x.ApplyEntiy.Id).ToPagedList(pageNumber - 1, pageSize);

                return View(model);

            }
        }

        [SecurityNode(Name = "添加页")]
        public PartialViewResult UserApplyServiceAuditing(int id)
        {
            if (id <= 0)
            {
                return PartialView();
            }
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var user = db.BackAdminUserEntityDBSet.FirstOrDefault(x => x.UserName == AdminUser.UserName);
                var apply = db.UserApplyServiceTable;
                var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;

                //var list = from a in apply
                //           join v in vol on a.VolId equals v.Id
                //           join d in detail on a.SDId equals d.Id
                //           join r in record on a.Id equals r.UASId
                //           select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = v, Vol = vol.Where(x => x.Id == r.VId), Detail = d, Record = r, Org = sorg.Where(x => x.SocialNO == d.SocialNo) };
                var list = from a in apply
                           join v in vol on a.VolId equals v.Id into g
                           join d in detail on a.SDId equals d.Id into g2
                           join r in record on a.Id equals r.UASId into g3
                           from gv in g.DefaultIfEmpty()
                           from gd in g2.DefaultIfEmpty()
                           from gr in g3.DefaultIfEmpty()
                           //select new { applyEntiy = a, userEntiy = v, vol2 = vol.Where(x => x.Id == r.VId), det = d, record = r, org = sorg.Where(x => x.SocialNO == d.SocialNo) };
                           //select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = v, Detail = d, Org = sorg.Where(x => x.SocialNO == d.SocialNo) };
                           select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = gv, Vol = vol.Where(x => x.Id == gr.VId), Detail = gd, Record = gr, Org = sorg.Where(x => x.SocialNO == gd.SocialNo) };
                list = list.Where(x => x.Org.FirstOrDefault().Id == user.SocOrgId);
                if (id > 0) list = list.Where(x => x.ApplyEntiy.Id == id);
                return PartialView(list.FirstOrDefault());
            }
        }
        public ActionResult AppVolServiceIndex(SelectSocSerModel model, GridSortOptions sort, int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {


                var apply = db.UserApplyServiceTable;
                // var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;

                var list = from s in detail
                           join a in apply on s.Id equals a.SDId into g
                           join o in sorg on s.SocialNo equals o.SocialNO
                           from stuDesc in g.DefaultIfEmpty()
                           where
                           (stuDesc.Num > record.Count(x => x.UASId == stuDesc.Id)
                            || record.Count(x => x.UASId == stuDesc.Id) == 0) && apply.Count(x => x.State == 1 && x.SDId == s.Id) > 0 && stuDesc.VolId != model.VId
                           select new SocServiceDetailEntityClone
                             {
                                 AddTime = s.AddTime,
                                 SocialName = o.Name,
                                 AddUser = s.AddUser,
                                 Contacts = s.Contacts,
                                 SocialNo = s.SocialNo,
                                 Context = s.Context,
                                 CoverCommunity = s.CoverCommunity,
                                 Desc = s.Desc,
                                 EndTime = s.EndTime,
                                 Id = s.Id,
                                 PayType = s.PayType,
                                 Phone = s.Phone,
                                 PubTime = s.PubTime,
                                 Score = s.Score,
                                 SerNum = s.SerNum,
                                 THSScore = s.THSScore,
                                 Type = s.Type,
                                 VHelpDesc = s.VHelpDesc,
                               
                                 // UserApplyEntity=a,
                                 SocSerImgs = img.Where(x => x.SocSerId == s.Id).ToList()

                             };

                //                string sql = @"  SELECT * FROM socservicedetail d
                //                                    LEFT JOIN  userapplyservice u ON d.id=u.sdid 
                //                                    WHERE u.num > (SELECT COUNT(* ) FROM serrecord r WHERE u.`Id`=r.`SDId`) OR 0=(SELECT COUNT(* ) FROM serrecord r WHERE u.`Id`=r.`SDId`)";
                //                var list = db.Database.SqlQuery<SocServiceDetailEntity>(sql);
                //list = list.Where(x => DateTime.Now > x.PubTime && DateTime.Now < x.EndTime);


                //(from r in record where stuDesc.Id == r.SDId select r).Count()
                if (!string.IsNullOrEmpty(model.Type)) list = list.Where(x => x.Type.Trim().ToUpper() == model.Type.Trim().ToUpper());
                list = list.Where(x => DateTime.Now > x.PubTime && DateTime.Now < x.EndTime);
                var res = list.OrderBy(x => x.Id).ToPagedList(pageNumber - 1, size);

                return Json(res.DistinctBy(x => x.Id).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 普通用户查询其申请的服务
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AppUserApplyServiceIndex(SelectSocSerModel model, GridSortOptions sort, int? page, int? pageSize = 20)
        {
            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {


                var apply = db.UserApplyServiceTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;

                var list=from a in apply
                          join d in detail on a.SDId   equals d.Id  into g
                         from s in g.DefaultIfEmpty()
                          join o in sorg on s.SocialNo equals o.SocialNO into g3
                         from go in g3.DefaultIfEmpty()
                          join r in record on  s.Id equals r.SDId into g2
                          where
                          a.VolId == model.VId
                          select new
                          {
                              AddTime = s.AddTime,
                              SocialName = go.Name,
                              AddUser = s.AddUser,
                              Contacts = s.Contacts,
                              SocialNo = s.SocialNo,
                              Context = s.Context,
                              CoverCommunity = s.CoverCommunity,
                              Desc = s.Desc,
                              EndTime = s.EndTime,
                              Id = s.Id,
                              PayType = s.PayType,
                              Phone = s.Phone,
                              PubTime = s.PubTime,
                              Score = s.Score,
                              SerNum = s.SerNum,
                              THSScore = s.THSScore,
                              Type = s.Type,
                              VHelpDesc = s.VHelpDesc,
                              UserApplyEntity = a,
                              SerRecord = g2,
                              State=s.State,
                           
                              SocSerImgs = img.Where(x => x.SocSerId == s.Id).ToList()

                          };
                #region 注释
                //var list = from s in detail
                //           join a in apply on s.Id equals a.SDId into g
                //           // join o in sorg on s.SocialNo equals o.SocialNO
                //           join r in record on s.Id equals r.SDId
                //           from stuDesc in g.DefaultIfEmpty()
                //           where
                //            stuDesc.VolId == model.VId
                //           select new
                //           {
                //               AddTime = s.AddTime,
                //               //  SocialName = o.Name,
                //               AddUser = s.AddUser,
                //               Contacts = s.Contacts,
                //               SocialNo = s.SocialNo,
                //               Context = s.Context,
                //               CoverCommunity = s.CoverCommunity,
                //               Desc = s.Desc,
                //               EndTime = s.EndTime,
                //               Id = s.Id,
                //               PayType = s.PayType,
                //               Phone = s.Phone,
                //               PubTime = s.PubTime,
                //               Score = s.Score,
                //               SerNum = s.SerNum,
                //               THSScore = s.THSScore,
                //               Type = s.Type,
                //               VHelpDesc = s.VHelpDesc,
                //               UserApplyEntity = g,
                //               SerRecord = r,
                //               SocSerImgs = img.Where(x => x.SocSerId == s.Id).ToList()

                //           };
                #endregion
                var res = list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 废弃
        /// </summary>
        /// <param name="selectModel"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AppUserServiceIndex_bak(SelectSocSerModel selectModel, GridSortOptions sort, int? page, int? pageSize = 20)
        {
            int pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var user = db.BackAdminUserEntityDBSet.FirstOrDefault(x => x.UserName == AdminUser.UserName);



                var apply = db.UserApplyServiceTable;
                var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;


                var list = from a in apply
                           join v in vol on a.VolId equals v.Id into g
                           join d in detail on a.SDId equals d.Id into g2
                           //join r in record on a.Id equals r.UASId into g3
                           from gv in g.DefaultIfEmpty()
                           from gd in g2.DefaultIfEmpty()
                           //from gr in g3.DefaultIfEmpty()
                           select new ShowApplyEntity { ApplyEntiy = a, UserEntiy = gv, Detail = gd, Org = sorg.Where(x => x.SocialNO == gd.SocialNo) };
                list = list.Where(x => x.Org.FirstOrDefault().Id == user.SocOrgId);
                if (selectModel.Id > 0) list = list.Where(x => x.ApplyEntiy.Id == selectModel.Id);

                SelectUserApplySerModel model = new SelectUserApplySerModel();

                model.ApplySerList = list.OrderByDescending(x => x.ApplyEntiy.Id).ToPagedList(pageNumber - 1, size);

                return Json(model);

            }

        }


        /// <summary>
        /// 志愿者申请处理服务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult AppVolApplyDo(SerRecordEntity model)
        {
            if (model.VId == 0 || model.SDId == 0)
            {
                return Json(new { state = -3, msg = "自愿者ID（VId）或者服务Id（SDId）,是必填参数" });
            }
            model.AddTime = DateTime.Now;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var apply = db.UserApplyServiceTable;
                // var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;



                if (record.Count(r => r.SDId == model.SDId && r.VId == model.VId) > 0)
                {
                    return Json(new { state = 0, msg = "该服务你已经成功申请成为志愿者,不能再申请." });
                }
                UserApplyServiceEntity userApply = (from a in apply
                                                    where a.SDId == model.SDId
                                                    && (a.Num > (record.Count(x => x.UASId == a.Id))
                                                    || record.Count(x => x.UASId == a.Id) == 0) && a.State == 1
                                                    select a).OrderByDescending(x => x.Id).FirstOrDefault();
                if (userApply == null)
                {
                    return Json(new { state = -1, msg = "所申请参与的志愿者服务,已经被其他志愿者先申请,没有位置了." });
                }

                if (userApply.VolId == model.VId)
                {
                    return Json(new { state = -2, msg = "该服务你已作受众,不能再申请成为志愿者为项目提供服务." });

                }

                model.UASId = userApply.Id;
                model.SDId = userApply.SDId;


                db.Add(model);
                db.SaveChanges();
                return Json(new { state = 1, msg = "成功" });
            }
        }

        /// <summary>
        /// 志愿者执行服务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AppVolBeginDoing(SerRecordEntity model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var apply = db.UserApplyServiceTable;
                // var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;


                SerRecordEntity dbEntity = record.Find(model.Id);

                if (dbEntity.BeginTime.HasValue)
                {
                    return Json(new { state = -1, msg = "已经开始执行" });
                }
                if (dbEntity.EndTime.HasValue)
                {
                    return Json(new { state = -2, msg = "已经执行完成" });
                }
                dbEntity.Img += model.Img;
                dbEntity.BeginTime = DateTime.Now;

                db.Update<SerRecordEntity>(dbEntity);
                db.SaveChanges();

                return Json(new { state = 1, msg = "成功" });
            }



        }

        public ActionResult AppVolEndDoing(SerRecordEntity model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var apply = db.UserApplyServiceTable;
                var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;


                SerRecordEntity dbEntity = record.Find(model.Id);
                if (dbEntity==null)
                {
                      return Json(new { state = -3, msg = "服务记录不存在" });
                }
                if (!dbEntity.BeginTime.HasValue)
                {
                    return Json(new { state = -1, msg = "还没有开始执行" });
                }

                if (dbEntity.EndTime.HasValue)
                {
                    return Json(new { state = -2, msg = "已经执行完成" });
                }



                SocServiceDetailEntity dtEntity = detail.Find(dbEntity.SDId);

                if (dtEntity==null)
                {
                    return Json(new { state = -5, msg = "服务详情不存在" });
                }

                VolunteerEntity volEntity = vol.Find(dbEntity.VId);
                volEntity.Score = volEntity.Score + dtEntity.Score;
                db.Update<VolunteerEntity>(volEntity);
                db.SaveChanges();

                dbEntity.Description = model.Description;
                dbEntity.Img += model.Img;
                dbEntity.EndTime = DateTime.Now;
                db.Update<SerRecordEntity>(dbEntity);
                db.SaveChanges();

                return Json(new { state = 1, msg = "成功" });
            }



        }

        public ActionResult AppSaveVolDoImg()
        {
            try
            {

                var file = Request.Files[0];
                string FtpServerHttpUrl = System.Configuration.ConfigurationManager.AppSettings["FtpServerHttpUrl"];
                string FtpServer = System.Configuration.ConfigurationManager.AppSettings["FtpServer"];
                string FtpUser = System.Configuration.ConfigurationManager.AppSettings["FtpUser"];
                string FtpPassWord = System.Configuration.ConfigurationManager.AppSettings["FtpPassWord"];

                string Dir = DateTime.Now.ToString("yyyyMMdd");
                FTPHelper ftp = new FTPHelper(FtpServer, "VolDo/" + Dir, FtpUser, FtpPassWord);

                FileInfo file2 = new FileInfo(file.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + AdminUser.Id.ToString() + file2.Extension;

                ftp.Upload(file, fileName);

                string imgUrl = FtpServerHttpUrl + ftp.FtpRemotePath + "/" + fileName;

                return Json(new
                {
                    state = 1,
                    url = imgUrl,
                    error = ""
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    state = 0,
                    url = "",
                    error = ex.Message + ex.TargetSite
                });
                throw ex;
            }
        }

        public ActionResult AppVolDoingIndex(SerRecordEntity model, GridSortOptions sort, int? page, int? pageSize = 20)
        {

            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {


                var apply = db.UserApplyServiceTable;
                // var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;

                var list = from s in detail
                           //join a in apply on s.Id equals a.SDId into g
                           join a in apply on s.Id equals a.SDId
                           join o in sorg on s.SocialNo equals o.SocialNO
                           join r in record on s.Id equals r.SDId
                           where r.VId == model.VId && a != null && r.UASId == a.Id
                           select new SocServiceDetailEntityClone
                             {
                                 AddTime = s.AddTime,
                                 SocialName = o.Name,
                                 AddUser = s.AddUser,
                                 Contacts = s.Contacts,
                                 SocialNo = s.SocialNo,
                                 Context = s.Context,
                                 CoverCommunity = s.CoverCommunity,
                                 Desc = s.Desc,
                                 EndTime = s.EndTime,
                                 Id = s.Id,
                                 PayType = s.PayType,
                                 Phone = s.Phone,
                                 PubTime = s.PubTime,
                                 Score = s.Score,
                                 SerNum = s.SerNum,
                                 THSScore = s.THSScore,
                                 Type = s.Type,
                                 VHelpDesc = s.VHelpDesc,
                                 SerRecord = r,
                                 //UserApplyEntity = g.FirstOrDefault(x => x.SDId == s.Id),
                                 UserApplyEntity = a,
                               
                                 SocSerImgs = img.Where(x => x.SocSerId == s.Id).ToList(),
                                 VolCount = record.Count(x => x.SDId == s.Id && x.UASId == a.Id)

                             };

                var res = list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size);

                return Json(res.DistinctBy(x => x.Id), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detail(int Id)
        {
            ModelShowDetailUserApplyList model = new ModelShowDetailUserApplyList();
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var apply = db.UserApplyServiceTable;
                var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;

                ShowDetailUserApply list = (from d in detail
                                            where d.Id == Id
                                            select new ShowDetailUserApply
                                            {
                                                SocSerDetail = d,
                                                SOrg = sorg.Where(x => x.SocialNO == d.SocialNo).FirstOrDefault(),
                                                UserApplyList = apply.Where(x => x.SDId == d.Id).ToList()
                                            }).FirstOrDefault();
                if (list == null)
                {
                    return View(model);
                }
                model.SocSerDetail = list.SocSerDetail;
                model.SOrg = list.SOrg;
                //  model.UserApplyList = list.UserApplyList.ToList();
                model.UserRecApplyList = model.UserRecApplyList ?? new List<ApplyUserRecVol>();
                foreach (var item in list.UserApplyList)
                {
                    ApplyUserRecVol aurv = new ApplyUserRecVol();
                    //志愿者信息
                    IList<RecVol> rdvList = (from r in record
                                             join v in vol on r.VId equals v.Id
                                             where r.UASId == item.Id
                                             select new RecVol { RecordEntity = r, Vol = v }).ToList();

                    VolunteerEntity userInfo = vol.SingleOrDefault(x => x.Id == item.VolId);
                    aurv.SerVolList = rdvList;
                    aurv.ApplyUserInfo = userInfo;
                    aurv.UserApplyService = list.UserApplyList.FirstOrDefault(x => x.Id == item.Id);
                    model.UserRecApplyList.Add(aurv);
                }

                //var m = model;
                // return Json(model,JsonRequestBehavior.AllowGet);
                return View(model);
            }
        }
        /// <summary>
        /// 执行任务中的志愿者
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [SecurityNode(Name = "执行任务中的志愿者")]
        public ActionResult VolDoingIndex(SerRecordEntity model, GridSortOptions sort, int? page, int? pageSize = 20)
        {

            var pageNumber = page ?? 1;
            int size = pageSize ?? 20;
            BackAdminUser bUser = this.GetBackUserInfo();
            using (IplusOADBContext db = new IplusOADBContext())
            {


                var apply = db.UserApplyServiceTable;
                var vol = db.VolunteerEntityTable;
                var detail = db.SocServiceDetailEntityTable;
                var record = db.SerRecordTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;

                var list = from r in record
                           join d in detail on r.SDId equals d.Id into g
                           from gd in g.DefaultIfEmpty()
                           join v in vol on r.VId equals v.Id
                           join a in apply on r.UASId equals a.Id into g2
                           from ga in g2.DefaultIfEmpty()
                           join v2 in vol on ga.VolId equals v2.Id
                           join o in sorg on gd.SocialNo equals o.SocialNO

                           where v.DepId.StartsWith(bUser.DeptId)
                           select new ShowVolDoingModel
                           {
                               SocServiceDetail = gd,
                               Record = r,
                               User = v2,
                               Vol = v

                           };

                var res = list.OrderByDescending(x => x.Record.Id).ToPagedList(pageNumber - 1, size);

                return View(res);
            }
        }

        public ActionResult AppUserComment(SerRecordEntity model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var record = db.SerRecordTable;
                SerRecordEntity dbEntity = record.Find(model.Id);
                if (!dbEntity.BeginTime.HasValue)
                {
                    return Json(new { state = -1, msg = "还没有开始执行,不能评价" });
                }

                if (!dbEntity.EndTime.HasValue)
                {
                    return Json(new { state = -2, msg = "服务还没有执行完成,不能评价" });
                }

                dbEntity.Comment = model.Comment;
                db.Update<SerRecordEntity>(dbEntity);
                db.SaveChanges();

                return Json(new { state = 1, msg = "成功" });
            }
        }
        /// <summary>
        /// 通过Id获取服务详细信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public ActionResult AppGetByIdSocSerDetail(SelectSocSerModel model, GridSortOptions sort)
        {
            var pageNumber = 1;
            int size = 1;
            string depId = "";
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var vol = db.VolunteerEntityTable;
                depId = (from v in vol where v.Id == AdminUser.Id select v.DepId).FirstOrDefault();
            }
            return Json(SocSerService.TypeList(pageNumber, size, depId, model, sort), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Auditing(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //SocServiceDetailEntity entity = db.SocServiceDetailEntityTable.Find(id);
                GridSortOptions sort = new GridSortOptions();
                SelectSocSerModel model = new SelectSocSerModel();
                model.Id = id;
                SocServiceDetailEntityClone entity = SocSerService.TypeList(1, 1, "0", model, sort).FirstOrDefault();

                var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.Level <= 6).ToList();

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

        //[SecurityNode(Name = "修改社区服务内容")]
        public ActionResult PostAuditing(SocServiceAuditeLog entity)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                SocServiceDetailEntity data = db.SocServiceDetailEntityTable.Find(entity.SDId);

                data.State = entity.State;
                db.Update<SocServiceDetailEntity>(data);
                db.SaveChanges();

                entity.AuditeUserId = AdminUser.Id;
                entity.AddTime = DateTime.Now;
                db.Add<SocServiceAuditeLog>(entity);
                db.SaveChanges();

                db.Dispose();
                return Success("审核成功");
         
            }

        }
        [SecurityNode(Name = "待审核列表")]
        public ActionResult PostAuditingIndex(int? page, SelectSocSerModel model, GridSortOptions sort)
        {
            var pageNumber = page ?? 1;
            model.State = 0;//表示待审核的
            model.SocSerList = SocSerService.TypeListState(pageNumber, pageSize, null, model, sort);
            return View(model);
        }
    }
}