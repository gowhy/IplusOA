using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;
using DataLayer.IplusOADB;
using BackWebAdmin.Models;
using System.Linq.Expressions;

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

        public static dynamic TypeList(int pageNumber, int pageSize, string depId, string type = null)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {

                var ser = db.SocServiceDetailEntityTable;
                var sorg = db.SocialOrgEntityTable;
                var res = from s in ser
                          join o in sorg on s.SocialNo equals o.SocialNO
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
                              VHelpDesc = s.VHelpDesc
                          };
                res = res.Where(x => x.CoverCommunity.IndexOf(depId) != -1);
                if (type != null)
                {
                    res = res.Where(x => x.Type.Trim().ToUpper() == type.Trim().ToUpper());
                }
                return res.ToPagedList(pageNumber - 1, pageSize);
            }
        }

    }
}