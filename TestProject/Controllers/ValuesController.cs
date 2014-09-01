using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;

namespace TestProject.Controllers
{
  

    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }


    }

    public class AccountModel  
    {  
        /// <summary>  
        /// 创建登录用户的票据信息  
        /// </summary>  
        /// <param name="strUserName"></param>  
        internal string CreateLoginUserTicket(string strUserName, string strPassword)  
        {  
            //构造Form验证的票据信息  
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, strUserName, DateTime.Now, DateTime.Now.AddMinutes(90),  
                true, string.Format("{0}:{1}", strUserName, strPassword), FormsAuthentication.FormsCookiePath);  
  
            string ticString = FormsAuthentication.Encrypt(ticket);  
  
            //把票据信息写入Cookie和Session  
            //SetAuthCookie方法用于标识用户的Identity状态为true  
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticString));  
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
        /// 解密票据信息
        /// </summary>
        /// <param name="encryptTicket"></param>
        /// <returns></returns>
        private bool ValidateUserTicket(string encryptTicket)
        {
            var userTicket = FormsAuthentication.Decrypt(encryptTicket);
            var userTicketData = userTicket.UserData;

            string userName = userTicketData.Substring(0, userTicketData.IndexOf(":"));
            string password = userTicketData.Substring(userTicketData.IndexOf(":") + 1);

            //检查用户名、密码是否正确，验证是合法用户  
            //var isQuilified = CheckUser(userName, password);  
            return true;
        }
        /// <summary>  
        /// 获取用户权限列表数据  
        /// </summary>  
        /// <param name="userName"></param>  
        /// <returns></returns>  
        internal string GetUserAuthorities(string userName)  
        {  
            //从WebApi 访问用户权限数据，然后写入Session  
            string jsonAuth = "[{\"Controller\": \"SampleController\", \"Actions\":\"Apply,Process,Complete\"}, {\"Controller\": \"Product\", \"Actions\": \"List,Get,Detail\"}]";  
            //var userAuthList = ServiceStack.Text.JsonSerializer.DeserializeFromString(jsonAuth, typeof(UserAuthModel[]));  
           // HttpContext.Current.Session["USER_AUTHORITIES"] = userAuthList;  
  
            return jsonAuth;  
        }  
  
        /// <summary>  
        /// 读取数据库用户表数据，判断用户密码是否匹配  
        /// </summary>  
        /// <param name="name"></param>  
        /// <param name="password"></param>  
        /// <returns></returns>  
        internal bool ValidateUserLogin(string name, string password)  
        {  
            //bool isValid = password == passwordInDatabase;  
            return true;  
        }  
  
        /// <summary>  
        /// 用户注销执行的操作  
        /// </summary>  
        internal void Logout()  
        {  
            FormsAuthentication.SignOut();  
        }  
    }

    /// <summary>  
    /// 基本验证Attribtue，用以Action的权限处理  
    /// </summary>  
    public class BaseAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>  
        /// 检查用户是否有该Action执行的操作权限  
        /// </summary>  
        /// <param name="actionContext"></param>  
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //检验用户ticket信息，用户ticket信息来自调用发起方  
            if (actionContext.Request.Headers.Authorization != null)
            {
                //解密用户ticket,并校验用户名密码是否匹配  
                var encryptTicket = actionContext.Request.Headers.Authorization.Parameter;
                if (ValidateUserTicket(encryptTicket))
                    base.OnActionExecuting(actionContext);
                else
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                //检查web.config配置是否要求权限校验  
                bool isRquired = (WebConfigurationManager.AppSettings["WebApiAuthenticatedFlag"].ToString() == "true");
                if (isRquired)
                {
                    //如果请求Header不包含ticket，则判断是否是匿名调用  
                    var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                    bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

                    //是匿名用户，则继续执行；非匿名用户，抛出“未授权访问”信息  
                    if (isAnonymous)
                        base.OnActionExecuting(actionContext);
                    else
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    base.OnActionExecuting(actionContext);
                }
            }
        }

        /// <summary>  
        /// 校验用户ticket信息  
        /// </summary>  
        /// <param name="encryptTicket"></param>  
        /// <returns></returns>  
        private bool ValidateUserTicket(string encryptTicket)
        {
            var userTicket = FormsAuthentication.Decrypt(encryptTicket);
            var userTicketData = userTicket.UserData;

            string userName = userTicketData.Substring(0, userTicketData.IndexOf(":"));
            string password = userTicketData.Substring(userTicketData.IndexOf(":") + 1);

            //检查用户名、密码是否正确，验证是合法用户  
            //var isQuilified = CheckUser(userName, password);  
            return true;
        }
    } 
}
