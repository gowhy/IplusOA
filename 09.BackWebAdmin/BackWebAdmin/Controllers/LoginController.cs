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



namespace BackWebAdmin.Controllers
{
    public class LoginController : AdminController
    {

        public ActionResult Index()
        {
             return View();
        }


        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            BackAdminUser admin = new BackAdminUser();
            admin.UserName = model.UserName;
            admin.PassWord = model.Password;
            if (Login.VLogin( admin))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);

        }

        [HttpPost]
        public ActionResult App(LoginModel model,string type)
        {
            BackAdminUser admin = new BackAdminUser();
            admin.UserName = model.UserName;
            admin.PassWord = model.Password;
            VolunteerEntityClone res = Login.VLoginApp(admin, type);
          
            if (res != null)
            {
                res.Msg += "登录成功";
                return Json(res);
            }
            else
            {
                return Json("用户名或者密码不对");
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
         

            Login.ValidateUserTicket(admin.LoginToken,ref admin);

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


                var query = from v in  soc
                            join s in vol on v.SocialNO equals s.SocialNO into ogroup
                            select new { v2 = v.SocialNO, s2 = ogroup.Count() };
                string str=query.ToString()+ "\\r\\n";
                foreach (var order in query)
                {

                    
                   str+=string.Format( "  CustomerID: {0}  Orders Count: {1} ",
                        order.v2, 
                        order.s2);
                }
                var sss = str;
                return Content(str);
            }

        }
    }
}