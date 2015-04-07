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
    [SecurityModule(Name = "签到")]
    public class SignInController : BaseController
    {
        const int pageSize = 100;
        [SecurityNode(Name = "首页")]
        public ActionResult AppSignIn(SignIn signInModel)
        {
            AppReturnSignInModel returnModel = new AppReturnSignInModel();
            if (signInModel.UserId <= 0)
            {
                returnModel.State = -1;
                returnModel.Msg = "UserId(当前登录用户Id)是必填参数";
                return Json(returnModel);
            }


            using (IplusOADBContext db = new IplusOADBContext())
            {

                var sT = db.SignInTable;
                var vol = db.VolunteerEntityTable;
                int signInScore = 0;

                //获取用户数据库中最新的1条签到记录
                SignIn signInEntity = sT.Where(x => x.UserId == signInModel.UserId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (signInEntity == null)//从来未签到过获得1个积分
                {
                    signInScore = 1;
                }
                else
                {
                    int lastSignInWeek = Convert.ToInt32(signInEntity.SignInTime.DayOfWeek.ToString("d"));//上次签到是星期几
                    int week = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));//当前星期几

                    TimeSpan ts = DateTime.Now.Date - signInEntity.SignInTime.Date;//2个日期相减如果两天的查是1，表示是连续的2天签到
                    int days = ts.Days;

                    if (days == 0)//表示当天已经签到过
                    {
                        returnModel.State = 0;
                        returnModel.Msg = "当天已经签到过";
                        returnModel.LastSignInTime = signInEntity.Score;
                        return Json(returnModel);
                    }

                    #region 日期枚举
                    // 摘要: 
                    ////     表示星期日。
                    //Sunday = 0,
                    ////
                    //// 摘要: 
                    ////     表示星期一。
                    //Monday = 1,
                    ////
                    //// 摘要: 
                    ////     表示星期二。
                    //Tuesday = 2,
                    ////
                    //// 摘要: 
                    ////     表示星期三。
                    //Wednesday = 3,
                    ////
                    //// 摘要: 
                    ////     表示星期四。
                    //Thursday = 4,
                    ////
                    //// 摘要: 
                    ////     表示星期五。
                    //Friday = 5,
                    ////
                    //// 摘要: 
                    ////     表示星期六。
                    //Saturday = 6,
                    #endregion
                    if (week != 1 && (((days == 1) && (week - lastSignInWeek == 1)) || (week == 0 && lastSignInWeek == 6)))//表示连续签到获得积分，积分是当前星期几得积分
                    {
                        signInScore = signInEntity.Score + 1;//连续签到
                    }
                    else
                    {
                        signInScore = 1;//非连续签到只获得1个积分
                    }
                }

                //新增签到记录
                signInModel.Score = signInScore;
                signInModel.SignInTime = DateTime.Now;
                sT.Add(signInModel);
                db.SaveChanges();

                //给用户增加积分
                VolunteerEntity userEntity = vol.Find(signInModel.UserId);
                userEntity.Score = userEntity.Score + signInModel.Score;
                db.Update<VolunteerEntity>(userEntity);
                db.SaveChanges();

                returnModel.State = 1;
                returnModel.Score = userEntity.Score ?? 0;
                returnModel.LastSignInTime = signInScore;//连续签到次数
                returnModel.Msg = "签到成功,新增积分:" + signInModel.Score;
                return Json(returnModel);

            }
        }


        public ActionResult AppCheckSignIn(SignIn signInModel)
        {
            AppReturnSignInModel returnModel = new AppReturnSignInModel();
            if (signInModel.UserId <= 0)
            {
                returnModel.State = -1;
                returnModel.Msg = "UserId(当前登录用户Id)是必填参数";
                returnModel.LastSignInTime = 0;
                return Json(returnModel);
            }

            int signInScore = 0;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var sT = db.SignInTable;
                var vol = db.VolunteerEntityTable;

                //获取用户数据库中最新的1条签到记录
                SignIn signInEntity = sT.Where(x => x.UserId == signInModel.UserId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (signInEntity == null)//从来未签到
                {
                    returnModel.State = 1;
                    returnModel.Score = 0;
                    returnModel.LastSignInTime = 0;
                    returnModel.Msg = "从来未签到过";
                    return Json(returnModel);
                }
                else
                {
                    int lastSignInWeek = Convert.ToInt32(signInEntity.SignInTime.DayOfWeek.ToString("d"));//上次签到是星期几
                    int week = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));//当前星期几

                    TimeSpan ts = DateTime.Now.Date - signInEntity.SignInTime.Date;//2个日期相减如果两天的查是1，表示是连续的2天签到
                    int days = ts.Days;

                    if (days == 0)//表示当天已经签到过
                    {
                        returnModel.State = 0;
                        returnModel.LastSignInTime = signInEntity.Score;//已经连续签到次数
                        returnModel.Msg = "当天已经签到过" + signInEntity.Id + "##" + signInModel.UserId + "##" + signInEntity.SignInTime + "##" + DateTime.Now;
                        return Json(returnModel);
                    }
                    if (week != 1 && (((days == 1) && (week - lastSignInWeek == 1)) || (week == 0 && lastSignInWeek == 6)))//表示连续签到获得积分，积分是当前星期几得积分
                    {
                        returnModel.State = 1;
                        returnModel.LastSignInTime = signInEntity.Score;//已经连续签到次数

                    }
                    else
                    {
                        returnModel.State = 1;
                        returnModel.LastSignInTime = 0;//没有连续签到了

                    }

                }
                VolunteerEntity userEntity = vol.Find(signInModel.UserId);
                returnModel.Score = userEntity.Score ?? 0;

                return Json(returnModel);
            }
        }
    }
}