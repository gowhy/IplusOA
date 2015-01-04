using DataLayer.IplusOADB;
using IplusOAEntity;
using SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BusLogic.Login
{
    public class Login
    {
        public static bool VLogin( BackAdminUser admin)
        {
            IplusOADBContext db = null;
            try
            {
                db = new IplusOADBContext();

                  admin = db.BackAdminUserEntityDBSet.FirstOrDefault<BackAdminUser>(x => x.UserName == admin.UserName && x.PassWord == admin.PassWord);
                  if (admin != null)
                  {
                      admin.LoginToken = SSO.UserTicketManager.CreateLoginUserTicket(admin);
                      return true;
                  }
                  else
                  {
                      return false;
                  }
            }
            catch (Exception e)
            {
                admin.Msg = e.Message;
                throw e;
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
        }

        public static VolunteerEntityClone VLoginApp(BackAdminUser admin, string type)
        {
            if (admin == null)
            {
                return null;
            }
            IplusOADBContext db = null;
            try
            {
                db = new IplusOADBContext();


                var vol = db.VolunteerEntityTable;
                var dep = db.DepartmentTable;
                var sorg = db.SocialOrgEntityTable;

                var longVol = from v in vol
                              join d in dep on v.DepId equals d.Id into gd
                              join s in sorg on v.SocialNO equals s.SocialNO into gs
                              
                              select new VolunteerEntityClone
                              {
                                  
                                  
                                  DepName = gd.FirstOrDefault().Name,
                                  Address = v.Address,
                                  CardNum = v.CardNum,
                                  CardType = v.CardType,
                                  DepId = v.DepId,
                                  Doing = v.Doing,
                                  EMail = v.EMail,
                                  GroupID = v.GroupID,
                                  Honor = v.Honor,
                                  Number = v.Number,
                                  Id = v.Id,
                                  Phone = v.Phone,
                                  QQ = v.QQ,
                                  RealName = v.RealName,
                                  RealNameState = v.RealNameState,
                                  Score = v.Score,
                                  SocialNO = v.SocialNO,
                                  State = v.State,
                                  ThsScore = v.ThsScore,
                                  UerName = v.UerName,
                                  Type = v.Type,
                                  VID = v.VID,
                                  WeiXin = v.WeiXin,
                                  SocialName = gs.FirstOrDefault().Name,
                                  PassWord=v.PassWord,
                                  Sex=v.Sex,
                                  Age=v.Age

                              };

                if (!string.IsNullOrEmpty(type) && type == "志愿者") longVol = longVol.Where(x => x.VID == admin.UserName && x.PassWord == admin.PassWord);
                if (!string.IsNullOrEmpty(type) && type == "普通用户") longVol = longVol.Where(x => x.Phone == admin.UserName && x.PassWord == admin.PassWord);

                VolunteerEntityClone resVol = longVol.FirstOrDefault();
                if (resVol != null)
                {
                    admin.UserName = resVol.Phone;
                    admin.Id = resVol.Id;
                    admin.DeptId = resVol.DepId;
                    resVol.LoginState = 1;//登陆成功
                    admin.LoginToken = SSO.UserTicketManager.CreateLoginUserTicket(admin);
                    return resVol;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                admin.Msg = e.Message;
                throw e;
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
        }
        /// <summary>
        /// 验证用户登陆
        /// </summary>
        /// <param name="encryptTicket"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool ValidateUserTicket(string encryptTicket, ref BackAdminUser user)
        {

           return SSO.UserTicketManager.ValidateUserTicket(encryptTicket, ref user);
        }
        public static void LoginOut()
        {

              SSO.UserTicketManager.Logout();
        }

    }
}
