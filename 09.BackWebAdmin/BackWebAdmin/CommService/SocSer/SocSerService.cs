using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;
using DataLayer.IplusOADB;

namespace BackWebAdmin.CommService
{
    public class SocSerService
    {
        public static IPagedList<SocServiceDetailEntity> CList(int pageNumber, int pageSize, string depId)
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                var list = db.SocServiceDetailEntityTable.AsQueryable().Where(x => x.CoverCommunity.IndexOf(depId) != -1).ToList();
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