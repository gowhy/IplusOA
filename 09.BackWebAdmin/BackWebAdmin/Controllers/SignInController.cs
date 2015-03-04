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
    [SecurityModule(Name = "中天社区幸福响当当")]
    public class SignInController : BaseController
    {
        const int pageSize = 100;
        [SecurityNode(Name = "首页")]
        public ActionResult AppSignIn(SignIn signInModel)
        {
            AppReturnModel returnModel = new AppReturnModel();
            if (signInModel.UserId<=0)
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

                    TimeSpan ts = DateTime.Now - signInEntity.SignInTime;//2个日期相减如果两天的查是1，表示是连续的2天签到
                    int days = ts.Days;
                    if (days == 0)//表示当天已经签到过
                    {
                        returnModel.State = 0;
                        returnModel.Msg = "当天已经签到过,当前用户积分:" + signInEntity.Score;
                        return Json(returnModel);
                    }
                    if ((days == 1) && (week - lastSignInWeek == 1))//表示连续签到获得积分，积分是当前星期几得积分
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
                returnModel.Msg = "签到成功,新增积分:" + signInModel.Score;
                return Json(returnModel);

            }
        }

    }
}