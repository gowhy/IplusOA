using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSO;
namespace BackWeb.Controllers
{

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "Home Page";
            AdminUserEntity admin = new AdminUserEntity();

            admin.Name = "wehy";
            admin.Password = "123456";

            SSO.UserTicketManager.CreateLoginUserTicket(admin);

            //AdminUserEntity admin =new AdminUserEntity();
            //string encryptTicket = "";
            //UserTicketManager.ValidateUserTicket(encryptTicket, ref admin);

            return View();
        }
    }
}
