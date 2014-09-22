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

        public static VolunteerEntity VLoginApp(BackAdminUser admin, string type)
        {
            if (admin==null)
            {
                return null;
            }
            IplusOADBContext db = null;
            try
            {
                db = new IplusOADBContext();
                VolunteerEntity volentity = null;
                if (type == "志愿者")
                {
                     volentity = db.VolunteerEntityTable.FirstOrDefault<VolunteerEntity>(x => x.VID == admin.UserName && x.PassWord == admin.PassWord);
                }
                if (type == "普通用户")
                {
                    volentity = db.VolunteerEntityTable.FirstOrDefault<VolunteerEntity>(x => x.Phone == admin.UserName && x.PassWord == admin.PassWord);
                }

                if (volentity != null)
                {
                    admin.DeptId = volentity.DepId;
                    volentity.LoginState = 1;//登陆成功
                    admin.LoginToken = SSO.UserTicketManager.CreateLoginUserTicket(admin);
                    return volentity;
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
