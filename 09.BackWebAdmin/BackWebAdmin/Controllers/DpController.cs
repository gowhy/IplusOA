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


namespace BackWebAdmin.Controllers
{
    public class DpController : BaseController
    {
        //
        // GET: /Dp/
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult AjaxCount()
        {
            long? udepId = AdminUser.DeptId;

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
            db.Dispose();
            resList.OrderBy(x => x.OderBy);

            return Json(resList,JsonRequestBehavior.AllowGet);
        }

        public DpEntity CountSer(BaseContext db)
        {
            DpEntity res = new DpEntity();
            long? udepId = AdminUser.DeptId;

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
            res.Name = "辖区社会服务总计";
            res.Chlids.OrderBy(x => x.OderBy);
            return res;

        }


        public DpEntity CountOrg(BaseContext db)
        {
            long? udepId = AdminUser.DeptId;

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
            long? udepId = AdminUser.DeptId;

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
            return res;

        }


        public DpEntity CountSheQuvol(BaseContext db)
        {
            long? udepId = AdminUser.DeptId;

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
        
	}
}