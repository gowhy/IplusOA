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

        public static IPagedList<SocServiceDetailEntity> TypeList(int pageNumber, int pageSize, string depId,string type=null)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                List<SocServiceDetailEntity> list = null;
                if (type==null)
                {
                    list = db.SocServiceDetailEntityTable.AsQueryable().Where(x => x.CoverCommunity.IndexOf(depId) != -1).ToList();
                }
                else
                {
                    list = db.SocServiceDetailEntityTable.AsQueryable().Where(x => x.CoverCommunity.IndexOf(depId) != -1 && x.Type.Trim().ToUpper() == type.Trim().ToUpper()).ToList();
                }
                if (list==null)
                {
                    return null;
                }
                return list.ToPagedList(pageNumber - 1, pageSize);
            }
        }
     
    }
}