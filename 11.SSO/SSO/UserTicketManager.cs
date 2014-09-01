using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace SSO
{
   public class UserTicketManager
    {
        /// <summary>  
        /// 创建登录用户的票据信息  
        /// </summary>  
        /// <param name="strUserName"></param>  
       public static string CreateLoginUserTicket(BackAdminUser adminUser)
        {
           string strUserName=adminUser.UserName;
           int? roleId = adminUser.RoleId;
           long? deptId = adminUser.DeptId;
           int? socOrgId = adminUser.SocOrgId;
            //构造Form验证的票据信息  
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, strUserName, DateTime.Now, DateTime.Now.AddMinutes(90),
                true, string.Format("{0}:{1}:{2}:{3}", strUserName, roleId, deptId, socOrgId), FormsAuthentication.FormsCookiePath);

            string ticString = FormsAuthentication.Encrypt(ticket);

            //把票据信息写入Cookie和Session  
            //SetAuthCookie方法用于标识用户的Identity状态为true  
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserLoginCookieToken", ticString));
            FormsAuthentication.SetAuthCookie(strUserName, true);
            HttpContext.Current.Session["USER_LOGON_TICKET"] = ticString;

            //重写HttpContext中的用户身份，可以封装自定义角色数据；  
            //判断是否合法用户，可以检查：HttpContext.User.Identity.IsAuthenticated的属性值  
            string[] roles = ticket.UserData.Split(',');
            IIdentity identity = new FormsIdentity(ticket);
            IPrincipal principal = new GenericPrincipal(identity, roles);
            HttpContext.Current.User = principal;

            return ticString;//返回票据
        }
       /// <summary>
       /// 删除浏览器票据
       /// </summary>
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }  
        /// <summary>
        /// 解密票据信息
        /// </summary>
        /// <param name="encryptTicket"></param>
        /// <returns></returns>
        public static bool ValidateUserTicket(string encryptTicket, ref BackAdminUser user)
        {
            try
            {
                var userTicket = FormsAuthentication.Decrypt(encryptTicket);
                var userTicketData = userTicket.UserData;

                string[] userInfoArr = userTicketData.Split(':');

                user.UserName = userInfoArr[0];
                user.RoleId =int.Parse(userInfoArr[1]??"0");
                user.DeptId = long.Parse(userInfoArr[2] ?? "0");
                user.SocOrgId = int.Parse(userInfoArr[3] ?? "0");
             
                //string strUserName = adminUser.UserName;
                //int? roleId = adminUser.RoleId;
                //long? deptId = adminUser.DeptId;
                //int? socOrgId = adminUser.SocOrgId;


                //user.IsEffect = bool.Parse(userInfoArr[2]);
                //user.IsDefaultAdmin = bool.Parse(userInfoArr[3]);
                //检查用户名、密码是否正确，验证是合法用户  
                //var isQuilified = CheckUser(userName, password);  
                return true;
            }
            catch (Exception e)
            {
                user.Msg = e.Message;
            }
            return false;
        }

    }
}
