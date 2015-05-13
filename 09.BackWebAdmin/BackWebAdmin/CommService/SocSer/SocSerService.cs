using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;
using DataLayer.IplusOADB;
using BackWebAdmin.Models;
using System.Linq.Expressions;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;

namespace BackWebAdmin.CommService
{
    public class SocSerService
    {
        public static IPagedList<SocServiceDetailEntity> CList(int pageNumber, int pageSize, Expression<Func<SocServiceDetailEntity, bool>> filter)
        {
            //var filter = PredicateExtensionses.True<SocServiceDetailEntity>();

            //if (!string.IsNullOrWhiteSpace(model.Type)) filter = filter.And(x => x.Type == model.Type);
            //if (!string.IsNullOrWhiteSpace(model.SocialNo)) filter = filter.And(x => x.SocialNo == model.SocialNo);
            //if (model.PubTime != default(DateTime)) filter = filter.And(x => x.PubTime >= model.PubTime);
            //if (model.PubTimeEnd != default(DateTime)) filter = filter.And(x => x.PubTime <= model.PubTimeEnd);

            //filter = filter.And((x => x.CoverCommunity.IndexOf(depId) != -1));

            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.SocServiceDetailEntityTable.Where(filter);
                return list.ToPagedList(pageNumber - 1, pageSize);
            }


        }

        public static IPagedList<SocServiceDetailEntityClone> TypeList(int pageNumber, int pageSize, string depId, SelectSocSerModel model, GridSortOptions sort)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                var ser = db.SocServiceDetailEntityTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;
                var res = from s in ser
                          join o in sorg on s.SocialNo equals o.SocialNO
                          //join i in img on s.Id equals i.SocSerId
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
                              State = s.State,
                              ISUserApply = s.ISUserApply,
                              CommentSocre = s.CommentSocre,
                              CommentTotal = s.CommentTotal,
                              GoodScore = s.GoodScore,
                              GoodTotal = s.GoodTotal,
                              ShareScore = s.ShareScore,
                              ShareTotal = s.ShareTotal,
                               LinkSocSerUrl=s.LinkSocSerUrl,
                               UploadHtmlFile=s.UploadHtmlFile,
                              SocSerImgs = img.Where(x => x.SocSerId > 0 && x.SocSerId == s.Id).ToList()

                          };

                if (!string.IsNullOrEmpty(depId)) res = res.Where(x => x.CoverCommunity.IndexOf(depId) != -1);
                if (!string.IsNullOrEmpty(model.Type)) res = res.Where(x => x.Type.Trim().ToUpper() == model.Type.Trim().ToUpper());
                if (!string.IsNullOrEmpty(model.SocialNo)) res = res.Where(x => x.SocialNo.Trim().ToUpper() == model.SocialNo.Trim().ToUpper());
                if (model.PubTime != default(DateTime)) res = res.Where(x => x.PubTime >= model.PubTime);
                if (model.PubTimeEnd != default(DateTime)) res = res.Where(x => x.PubTime <= model.PubTimeEnd);
                if (!string.IsNullOrEmpty(model.SocialName)) res = res.Where(x => x.SocialName.Contains(model.SocialName));
                if (model.Id>0) res = res.Where(x => x.Id==model.Id);
                if (!string.IsNullOrEmpty(model.AddUser)) res = res.Where(x => x.AddUser==(model.AddUser));



                res = res.OrderBy(sort.Column, sort.Direction == SortDirection.Descending);

                return res.ToPagedList(pageNumber - 1, pageSize);
            }
        }


        public static IPagedList<SocServiceDetailEntityClone> TypeList_UserApply(int pageNumber, int pageSize, string depId, SelectSocSerModel model, GridSortOptions sort)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                var ser = db.SocServiceDetailEntityTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;
                var res = from s in ser
                          join o in sorg on s.SocialNo equals o.SocialNO
                          //join i in img on s.Id equals i.SocSerId
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
                              ISUserApply = s.ISUserApply,
                              CommentSocre = s.CommentSocre,
                              CommentTotal = s.CommentTotal,
                              GoodScore = s.GoodScore,
                              GoodTotal = s.GoodTotal,
                              ShareScore = s.ShareScore,
                              ShareTotal = s.ShareTotal,
                              LinkSocSerUrl = s.LinkSocSerUrl,
                              UploadHtmlFile = s.UploadHtmlFile,
                              SocSerImgs = img.Where(x => x.SocSerId == s.Id).ToList()

                          };

                if (!string.IsNullOrEmpty(depId)) res = res.Where(x => x.CoverCommunity.IndexOf(depId) != -1);
                if (!string.IsNullOrEmpty(model.Type)) res = res.Where(x => x.Type.Trim().ToUpper() == model.Type.Trim().ToUpper());
                if (!string.IsNullOrEmpty(model.SocialNo)) res = res.Where(x => x.SocialNo.Trim().ToUpper() == model.SocialNo.Trim().ToUpper());
                if (model.PubTime != default(DateTime)) res = res.Where(x => x.PubTime >= model.PubTime);
                if (model.PubTimeEnd != default(DateTime)) res = res.Where(x => x.PubTime <= model.PubTimeEnd);
                if (!string.IsNullOrEmpty(model.SocialName)) res = res.Where(x => x.SocialName.Contains(model.SocialName));
                if (model.Id > 0) res = res.Where(x => x.Id == model.Id);



                res = res.OrderBy(sort.Column, sort.Direction == SortDirection.Descending);

                return res.ToPagedList(pageNumber - 1, pageSize);
            }
        }

        public static IPagedList<SocServiceDetailEntityClone> TypeListState(int pageNumber, int pageSize, string depId, SelectSocSerModel model, GridSortOptions sort)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                var ser = db.SocServiceDetailEntityTable;
                var sorg = db.SocialOrgEntityTable;
                var img = db.SocSerImgTable;
                var res = from s in ser
                          join o in sorg on s.SocialNo equals o.SocialNO
                          //join i in img on s.Id equals i.SocSerId
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
                              State = s.State,
                              ISUserApply = s.ISUserApply,
                              CommentSocre = s.CommentSocre,
                              CommentTotal = s.CommentTotal,
                              GoodScore = s.GoodScore,
                              GoodTotal = s.GoodTotal,
                              ShareScore = s.ShareScore,
                              ShareTotal = s.ShareTotal,
                              LinkSocSerUrl = s.LinkSocSerUrl,
                              UploadHtmlFile = s.UploadHtmlFile,
                              SocSerImgs = img.Where(x => x.SocSerId > 0 && x.SocSerId == s.Id).ToList()

                          };

                if (!string.IsNullOrEmpty(depId)) res = res.Where(x => x.CoverCommunity.IndexOf(depId) != -1);
                if (!string.IsNullOrEmpty(model.Type)) res = res.Where(x => x.Type.Trim().ToUpper() == model.Type.Trim().ToUpper());
                if (!string.IsNullOrEmpty(model.SocialNo)) res = res.Where(x => x.SocialNo.Trim().ToUpper() == model.SocialNo.Trim().ToUpper());
                if (model.PubTime != default(DateTime)) res = res.Where(x => x.PubTime >= model.PubTime);
                if (model.PubTimeEnd != default(DateTime)) res = res.Where(x => x.PubTime <= model.PubTimeEnd);
                if (!string.IsNullOrEmpty(model.SocialName)) res = res.Where(x => x.SocialName.Contains(model.SocialName));
                if (model.Id > 0) res = res.Where(x => x.Id == model.Id);
                res = res.Where(x => x.State == model.State);




                res = res.OrderBy(sort.Column, sort.Direction == SortDirection.Descending);

                return res.ToPagedList(pageNumber - 1, pageSize);
            }
        }

        public static AppReturnModel2 AppVolApplyDoService(SerRecordEntity model)
        {
            AppReturnModel2 retMsg = new AppReturnModel2();
            if ( model.SDId == 0)
            {
                retMsg.state = -3;
                retMsg.msg = "自愿者ID（VId）或者服务Id（SDId）,是必填参数";
                return retMsg; 
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
                    retMsg.state = 0;
                    retMsg.msg = "该服务你已经成功申请成为志愿者,不能再申请.";
                    return retMsg;
                    //return Json(new { state = 0, msg = "该服务你已经成功申请成为志愿者,不能再申请." });
                }

                SocServiceDetailEntity sdEntity = detail.Find(model.SDId);

                if (sdEntity.ISUserApply == 0)//0表示，需要普通用户申请的服务,反面是1表示是不需要普通用户申请的默认
                {
                    UserApplyServiceEntity userApply = (from a in apply
                                                        where a.SDId == model.SDId
                                                        && (a.Num > (record.Count(x => x.UASId == a.Id))
                                                        || record.Count(x => x.UASId == a.Id) == 0) && a.State == 1
                                                        select a).OrderByDescending(x => x.Id).FirstOrDefault();

                    if (userApply == null)
                    {
                        retMsg.state = -1;
                        retMsg.msg = "所申请参与的志愿者服务,已经被其他志愿者先申请,没有位置了.";
                        return retMsg;
                        //return Json(new { state = -1, msg = "所申请参与的志愿者服务,已经被其他志愿者先申请,没有位置了." });
                    }

                    if (userApply.VolId == model.VId)
                    {
                        retMsg.state = -2;
                        retMsg.msg = "该服务你已作受众,不能再申请成为志愿者为项目提供服务.";
                        return retMsg;
                        //return Json(new { state = -2, msg = "该服务你已作受众,不能再申请成为志愿者为项目提供服务." });

                    }

                    model.UASId = userApply.Id;
                }

                model.SDId = model.SDId;


                db.Add(model);
                db.SaveChanges();
                retMsg.state = 1;
                retMsg.msg = "成功";
                return retMsg;
                //return Json(new { state = 1, msg = "成功" });
            }

        }

        public static AppReturnModel2  AppUserApplyService(UserApplyServiceEntity entity)
        {
            AppReturnModel2 retMsg = new AppReturnModel2();
            try
            {
                if (entity.VolId <= 0 || entity.SDId <= 0)
                {
                    retMsg.state = -1;
                    retMsg.msg = "VolId或者SDId是必填参数，切必须大于0";
                    return retMsg;
                }

                entity.AddTime = DateTime.Now;
                using (IplusOADBContext db = new IplusOADBContext())
                {
                    var table = db.UserApplyServiceTable;

                    if (table.Count(x => x.VolId == entity.VolId && x.SDId == entity.SDId) > 0)
                    {
                        retMsg.state =0;
                        retMsg.msg = "失败,已经申请该服务";
                        return retMsg;

                        //return Json(new { state = 0, msg = "失败,已经申请该服务" });
                    }
                    else
                    {
                        entity.AddTime = DateTime.Now;
                        db.Add(entity);
                        db.SaveChanges();
                        retMsg.state = 1;
                        retMsg.msg = "成功";
                        return retMsg;
                        //return Json(new { state = 1, msg = "成功" });
                    }

                }

            }
            catch (Exception ex)
            {
                retMsg.state = -1;
                retMsg.msg = ex.Message + ex.TargetSite;
                return retMsg;

                //return Json(new { state = -1, msg = "接口执行异常：" + ex.Message + ex.TargetSite });
                throw;
            }
        }

    }
}