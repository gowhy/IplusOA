using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
//using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace SSO
{
    public class BaseAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>  
        /// 检查用户是否有该Action执行的操作权限  
        /// </summary>  
        /// <param name="actionContext"></param>  
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {

            //解密用户ticket,并校验用户名密码是否匹配  
            var encryptTicket = "";

            HttpCookie cookie = HttpContext.Current.Request.Cookies["UserLoginCookieToken"];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                encryptTicket = cookie.Value;
            }

            //检验用户ticket信息，用户ticket信息来自调用发起方  
            if (!string.IsNullOrEmpty(encryptTicket))
            {
                BackAdminUser user = new BackAdminUser();
                if (UserTicketManager.ValidateUserTicket(encryptTicket,ref user))
                {
                 
                    base.OnActionExecuting(actionContext);
                    HttpContext.Current.Items["Login_User_Info"] = user;
                    return;
                }
       

            }
            ErrorRedirect(actionContext);
            
            //else
            //{
            //        //如果请求Header不包含ticket，则判断是否是匿名调用  
            //        var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            //        bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

            //        //是匿名用户，则继续执行；非匿名用户，抛出“未授权访问”信息  
            //        if (isAnonymous)
            //            base.OnActionExecuting(actionContext);
            //        else
            //            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //}
        }
        private void ErrorRedirect(ActionExecutingContext filterContext)
        {

            filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Login", action = "Index" }));

        } 
    } 
}
