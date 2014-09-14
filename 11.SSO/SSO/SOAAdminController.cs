using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SSO
{

    public abstract class SOAAdminController : Controller
    {
        string UserLoginCookieTokenString = "";

  
        public BackAdminUser AdminUser
        {
            get
            {

                BackAdminUser admin = HttpContext.Items["Login_User_Info"] as BackAdminUser;
                if (admin == null)
                {
                    admin = new BackAdminUser();
                    if (string.IsNullOrEmpty(UserLoginCookieTokenString))
                   {
                       return null;
                   }
                    UserTicketManager.ValidateUserTicket(UserLoginCookieTokenString, ref admin);
                    HttpContext.Items["Login_User_Info"] = admin;
                }
                return admin;
            }
        }
     
    }
}
