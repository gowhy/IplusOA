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
                              State=s.State,
                              ISUserApply=s.ISUserApply,
                              SocSerImgs = img.Where(x => x.SocSerId >0&& x.SocSerId == s.Id).ToList()

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
    }
}