using BackWebAdmin.Models;
using SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.IplusOADB;
using System.Data.Entity;
using IplusOAEntity;
using BusLogic.Login;
using BackWebAdmin.CommService;



namespace BackWebAdmin.Controllers
{
    public class LoginController : AdminController
    {

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登陆验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ActionResult SMSLogin(string phone)
        {
            ReturnModel ret = SMSComm.SendLoginCodeSMS(phone);

            return Json(ret);

        }

        public ActionResult SMSLoginV2(string userName, string passWord)
        {
            BackAdminUser admin;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                admin = db.BackAdminUserEntityDBSet.FirstOrDefault<BackAdminUser>(x => x.UserName == userName && x.PassWord == passWord);

            }
            ReturnModel ret = new ReturnModel();
            if (admin == null)
            {
                ret.State = 0;
                ret.Msg = "用户名或者密码错误";
                return Json(ret);
            }
            ret = SMSComm.SendLoginCodeSMS(admin.Phone);

            return Json(ret);

        }

        [HttpPost]
        public ActionResult Index(LoginModel model, string phone, string code)
        {
            ReturnModel returnModel = new ReturnModel();
            BackAdminUser admin = new BackAdminUser();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                
                admin.UserName = model.UserName;
                admin.PassWord = model.Password; 

                admin = db.BackAdminUserEntityDBSet.FirstOrDefault<BackAdminUser>(x => x.UserName == admin.UserName && x.PassWord == admin.PassWord);
                phone = admin.Phone;

                DateTime codeOutTime = DateTime.Now.AddMinutes(-10);
                int existCount = db.SMSTable.Count(x => x.Phone == phone.Trim() && x.VCode == code.Trim() && x.BType == 2 && x.AddTime > codeOutTime);
                if (existCount == 0)
                {
                    returnModel.Msg = "验证码失效,请重新获取";
                    returnModel.State = -2;
                    return View(returnModel);
                }
            }
         
            if (Login.VLogin(admin))
            {
                return RedirectToAction("Index", "Home");
            }
            returnModel.Msg = "用户名或者密码错误";

            return View(returnModel);

        }

        public ActionResult AppLoginCheck(int userId, string type, string pCode)
        {
            
            SingleLoginCheck singleLoginCheck = new SingleLoginCheck();
            singleLoginCheck.UserId = userId;
            singleLoginCheck.PCode = pCode;///每个手机的唯一编码

            AppReturnModel ret=new AppReturnModel();
            if (singleLoginCheck.UserId > 0 && !string.IsNullOrEmpty(pCode) && !SingleLoginCheckService.Check(singleLoginCheck))
            {
                ret.Msg += "已有其他手机登陆,请重新登录";
                ret.State = 2;
            
            }
            else
            {
                ret.State = 1;
            }
            return Json(ret);
        }
        [HttpPost]
        public ActionResult App(LoginModel model, string type,string pCode)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return Json("UserName、Password是必填参数");
            }
            BackAdminUser admin = new BackAdminUser();
            VolunteerEntityClone resFail = new VolunteerEntityClone();
            try
            {


                admin.UserName = model.UserName;
                admin.PassWord = model.Password;
                VolunteerEntityClone res = Login.VLoginApp(admin, type);

                if (res != null)
                {
                    SingleLoginCheck singleLogin = new SingleLoginCheck();
                    singleLogin.UserId = res.Id;
                    singleLogin.PCode = pCode;///每个手机的唯一编码
                    singleLogin.Phone = res.Phone;
                    singleLogin.LoginToken = base.AdminUser.LoginToken;
                    SingleLoginCheckService.SaveLoginRecord(singleLogin);     
      
                    res.PassWord = null;
                    res.Msg += "登录成功";
                    res.State = 1;
                    return Json(res);
                }
                else
                {

                    resFail.State = 0;
                    resFail.Msg = "用户名或者密码不对." + admin.Msg;

                    return Json(resFail);
                }
            }
            catch (Exception ex)
            {
                resFail.State = 0;
                resFail.Msg = "登陆异常,异常信息." + ex.Message + admin.Msg;
                return Json(resFail);
                throw;
            }

        }


        public ActionResult LoginAPI([System.Web.Http.FromBody]LoginModel model)
        {

            //LoginModel model = new LoginModel();
            model.UserName = "weihy";
            model.Password = "123456";
            AdminUserEntity admin = new AdminUserEntity();
            //admin.Name = model.UserName;
            //admin.Password = model.Password;
            //Login.VLogin(admin);

            return Json(admin, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetList(BackAdminUser admin)
        {
            admin = admin ?? new BackAdminUser();
            admin.LoginToken = "70529DB457D3889D94E5D8B55F6AAE242521194B4F4283DB09747328A15821F321BE8CD8C2AC2491D0E2F211591A84F0FA62AF4CF3C9E95C08944D733E2120447B270B51AD8BE85B2641CCF09D160EF1C0215FD5345B9F7B27E5F3196F4B5BE66F71781C32E654046C63A490CE5CBE7B0884E45B05D6F958674A427B1EBFECBAB934E3F6B6B9596B760ED832A0B31196";


            Login.ValidateUserTicket(admin.LoginToken, ref admin);

            return Json(admin, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Out()
        {
            Login.LoginOut();
            return RedirectToAction("Index", "Login");

        }
        public ActionResult Select(BackAdminUser model)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {

                DbSet<VolunteerEntity> vol = db.VolunteerEntityTable;
                DbSet<SocialOrgEntity> soc = db.SocialOrgEntityTable;


                var query = from v in soc
                            join s in vol on v.SocialNO equals s.SocialNO into ogroup
                            select new { v2 = v.SocialNO, s2 = ogroup.Count() };
                string str = query.ToString() + "\\r\\n";
                foreach (var order in query)
                {


                    str += string.Format("  CustomerID: {0}  Orders Count: {1} ",
                         order.v2,
                         order.s2);
                }
                var sss = str;
                return Content(str);
            }

        }

        /// <summary>
        /// 登陆前获取验证码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult AppSellerLoginSendSmsCode(string userName, string password)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                Seller entity = db.SellerTable.Where(x => x.UserName == userName && x.PassWord == password).FirstOrDefault();
                if (entity != null)
                {
                    ReturnModel ret = SMSComm.SendLoginCodeSMSBySeller(entity.Phone);
                    return Json(ret);
                }
                else
                {
                    ReturnModel ret = new ReturnModel();
                    ret.State = 0;
                    ret.Msg = "用户名或者密码错误";
                    return Json(ret);
                }

            }
        }
        /// <summary>
        /// 商户登陆
        /// </summary>
        /// <param name="model"></param>
        /// <param name="vCode">手机验证码</param>
        /// <returns></returns>
        public ActionResult AppSellerLogin(string userName, string password, string vCode)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Json("UserName、Password是必填参数");
            }
            BackAdminUser admin = new BackAdminUser();

            try
            {
                admin.UserName = userName;
                admin.PassWord = password;

                using (IplusOADBContext db = new IplusOADBContext())
                {
                   
                    Seller entity = db.SellerTable.Where(x => x.UserName == userName && x.PassWord == password).FirstOrDefault();
                    if (entity != null)
                    {
                        SingleLoginCheck loginModel = new SingleLoginCheck();
                        loginModel.UserId = entity.Id;


                        //if (!SingleLoginCheckService.Check(loginModel))
                        //{

                        //    entity = new Seller();
                        //    entity.State = -3;
                        //    entity.Msg = "必须从新登陆." + admin.Msg;
                        //    return Json(entity);
                        //}

                        DateTime codeOutTime = DateTime.Now.AddMinutes(-10);
                        int existCount = db.SMSTable.Count(x => x.Phone == entity.Phone && x.VCode == vCode.Trim() && x.BType == 3 && x.AddTime > codeOutTime);
                        if (existCount == 0)
                        {
                            admin.Msg = "验证码失效,请重新获取";
                            admin.State = -2;
                            return View(admin);
                        }



                        entity.PassWord = null;
                        entity.Msg += "登录成功";
                        entity.State = 1;

                        admin.UserName = entity.UserName;
                        admin.Id = entity.Id;
                        admin.LoginToken = SSO.UserTicketManager.CreateLoginUserTicket(admin);
                        return Json(entity);
                    }
                    else
                    {
                        entity = new Seller();
                        entity.State = 0;
                        entity.Msg = "用户名或者密码不对." + admin.Msg;

                        return Json(entity);
                    }

                }

            }
            catch (Exception ex)
            {
                admin.State = 0;
                admin.Msg = "登陆异常,异常信息." + ex.Message + admin.Msg;
                return Json(admin);
                throw;
            }

        }

        public ActionResult AppOut()
        {
            Login.LoginOut();

            ReturnModel ret = new ReturnModel();

            using (IplusOADBContext db = new IplusOADBContext())
            {
                //每次登陆记录登陆信息
                SingleLoginCheck slc = db.SingleLoginCheckTable.Find(base.AdminUser.Id);
                slc.State = 1;
                db.Update(slc);
                db.SaveChanges();
            }

            ret.State = 1;
            ret.Msg = "注销成功";
            return Json(ret);

        }
    }
}