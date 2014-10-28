using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using DataLayer;
using MySql.Data.MySqlClient;
using BackWebAdmin.CommService;
using System.Data.Entity;
using BackWebAdmin.Models;


namespace BackWebAdmin.Controllers
{
      [SecurityModule(Name = "大屏")]
    public class DpController : BaseController
    {
        

        // GET: /Dp/
        [SecurityNode(Name = "首页")]
        public ActionResult Index()
        {
            return View();
        }

        #region 大屏数量统计 Ajax数据
        //[HttpPost]
        public ActionResult AjaxCount()
        {
            string udepId = AdminUser.DeptId;

            List<DpEntity> resList = new List<DpEntity>();
            IplusOADBContext db = new IplusOADBContext();
            //社会组织
            resList.Add(CountOrg(db));
            //社会服务
            resList.Add(CountSer(db));
            //社会服务自愿者
            resList.Add(CountSerVol(db));
            //辖区自愿者

            resList.Add(CountSheQuvol(db));

            DepartmentEntity dep=db.DepartmentTable.SingleOrDefault(x=>x.Id==udepId);

            db.Dispose();
            resList.OrderBy(x => x.OderBy);

            return Json(new { resList ,dep.Name}, JsonRequestBehavior.AllowGet);
        }

        public DpEntity CountSer(BaseContext db)
        {
            DpEntity res = new DpEntity();
            string udepId = AdminUser.DeptId;

            string sql = string.Format(@" SELECT  COUNT(S.`Id`) `count`,T.`Name`,T.`Code`
                                            ,COUNT(CASE  WHEN s.`EndTime` <NOW()   THEN s.`Id`  ELSE NULL END) OLD
                                            ,COUNT(CASE  WHEN  NOW() BETWEEN s.`PubTime` AND s.`EndTime` THEN s.`Id`  ELSE NULL END) NOW
                                            ,COUNT(CASE  WHEN  s.`PubTime` >NOW() THEN s.`Id`  ELSE NULL END) Future

                                             FROM  sertype t LEFT JOIN   socservicedetail  s ON  t.`Code`=s.`Type` 

                                             AND S.`Id`  NOT IN (
                                                                  SELECT j.`SSDetailId`  FROM socserdetailjoin  j WHERE j.`State`<>1 AND s.`Id`=j.`SSDetailId` AND j.`DepId` LIKE '{0}%'
                                                                 ) 

                                             AND  CoverCommunity LIKE '%{0}%' 

                                             GROUP BY T.`Code`", udepId);
            MySqlParameter[] parm = new MySqlParameter[] { };
            List<DpEntity> ser = db.Database.SqlQuery<DpEntity>(sql, parm).ToList();
            res.Chlids = ser;
            res.OderBy = 2;
            res.old = ser.Sum(x => x.old);
            res.now = ser.Sum(x => x.now);
            res.Name = "辖区社会服务总计";
            res.Chlids.OrderBy(x => x.OderBy);
            return res;

        }


        public DpEntity CountOrg(BaseContext db)
        {
            string udepId = AdminUser.DeptId;

            string sql = string.Format(@"SELECT  COUNT( s.`SocialNo`)  COUNT 

                            ,COUNT(CASE  WHEN s.`EndTime` <NOW()   THEN s.`SocialNo`  ELSE NULL END) OLD
                            ,COUNT(CASE  WHEN  NOW() BETWEEN s.`PubTime` AND s.`EndTime` THEN s.`SocialNo`  ELSE NULL END) NOW
                            ,COUNT(CASE  WHEN  s.`PubTime` >NOW() THEN s.`SocialNo`  ELSE NULL END) Future

                            FROM  socservicedetail  s

                            WHERE CoverCommunity LIKE '%{0}%' 
                            AND   NOT EXISTS (
                            SELECT j.`SSDetailId`  FROM socserdetailjoin  j WHERE j.`State`<>1 AND s.`Id`=j.`SSDetailId` AND j.`DepId` LIKE '{0}%'

                            )", udepId);

            MySqlParameter[] parm = new MySqlParameter[] { };

            DpEntity org = db.Database.SqlQuery<DpEntity>(sql, parm).SingleOrDefault();
            org.Name = "辖区社会组织总计";
            return org;

        }
        public DpEntity CountSerVol(BaseContext db)
        {
            DpEntity res = new DpEntity();
            res.Name = "辖区服务志愿者总计";
            string udepId = AdminUser.DeptId;

            string sql = string.Format(@"  SELECT  COUNT(r.`Id`) `count`,T.`Name`,T.`Code`

                                            ,COUNT(CASE  WHEN s.`EndTime` <NOW()   THEN  r.`Id`   ELSE NULL END) OLD
                                            ,COUNT(CASE  WHEN  NOW() BETWEEN s.`PubTime` AND s.`EndTime` THEN r.`Id`  ELSE NULL END) NOW
                                            ,COUNT(CASE  WHEN  s.`PubTime` >NOW() THEN r.`Id`   ELSE NULL END) Future  
  
                                            FROM  sertype t LEFT JOIN   socservicedetail  s
                                            ON  t.`Code`=s.`Type`  LEFT JOIN serrecord  r  ON r.`SDId`=s.`Id`
                                            AND S.`Id`  NOT IN (
                                            SELECT j.`SSDetailId`  FROM socserdetailjoin  j WHERE j.`State`<>1 AND s.`Id`=j.`SSDetailId` AND j.`DepId` LIKE '{0}%'
                                            ) 

                                            AND  CoverCommunity LIKE '%{0}%'    GROUP BY T.`Code` ", udepId);
            MySqlParameter[] parm = new MySqlParameter[] { };
            List<DpEntity> SerVol = db.Database.SqlQuery<DpEntity>(sql, parm).ToList();
            res.Chlids = SerVol;
            res.OderBy = 3;
            res.old = SerVol.Sum(x => x.old);
            res.now = SerVol.Sum(x => x.now);
            return res;

        }


        public DpEntity CountSheQuvol(BaseContext db)
        {
            string udepId = AdminUser.DeptId;

            string sql = string.Format(@"SELECT COUNT(*) `count`
                                                ,COUNT(CASE  WHEN  v.`AddTime` <CURDATE()   THEN v.`Id`  ELSE NULL END) `old`
                                                ,COUNT(CASE  WHEN  v.`AddTime` >CURDATE()  THEN v.`Id`  ELSE NULL END) `now`
                                                 FROM volunteer  v WHERE   depid LIKE '{0}%'   ", udepId);
            MySqlParameter[] parm = new MySqlParameter[] { };
            DpEntity org = db.Database.SqlQuery<DpEntity>(sql, parm).SingleOrDefault();
            org.Name = "辖区注册志愿者总计";
            org.OderBy = 5;
            return org;

        }
        #endregion

        public ActionResult SerVolShow()
        {
            string depId = AdminUser.DeptId.ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                //var SerList = db.SocServiceDetailEntityTable.AsQueryable()//
                //    .Where(x => x.CoverCommunity.IndexOf(depId) != -1//
                //              && x.PubTime < DateTime.Now //
                //              && x.EndTime > DateTime.Now
                //           )//
                //    .ToList();

                var SerList = (from x in db.SocServiceDetailEntityTable
                              where x.CoverCommunity.IndexOf(depId) != -1
                                  && x.PubTime < DateTime.Now
                                  && x.EndTime > DateTime.Now

                               select new ShowSocSerModel { Context = x.Context, PubTime = x.PubTime, EndTime = x.EndTime }).OrderByDescending(x => x.PubTime).Skip(0).Take(10);
             

                DbSet<SocServiceDetailEntity> soc = db.SocServiceDetailEntityTable;
                DbSet<SerRecordEntity> record = db.SerRecordTable;
                DbSet<VolunteerEntity> vol = db.VolunteerEntityTable;
                DbSet<DepartmentEntity> dep = db.DepartmentTable;

              DateTime gdate=DateTime.Now.AddHours(-8);
                var vlist = (from r in record
                            join s in soc on r.SDId equals s.Id
                            join v in vol on r.VId equals v.VID
                            join d in dep on r.SocID equals d.Id
                            where r.BeginTime<DateTime.Now //
                                && r.BeginTime != null//
                                && (r.BeginTime > gdate)//
                                &&(r.EndTime > DateTime.Now || r.EndTime == null)//
                                &&s.PubTime <= DateTime.Now //
                                &&s.EndTime >= DateTime.Now

                            select new { s.Context, v.RealName, r.BeginTime,d.Name,CDate=DateTime.Now  }).ToList();

                dic.Add("ser", SerList.ToList());
                dic.Add("vol", vlist.ToList());
            }

            return Json(dic);
        }
    }
}