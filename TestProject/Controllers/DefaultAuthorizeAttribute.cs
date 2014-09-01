using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using QDT.Common;
using QDT.MVC.Result;

namespace QDT.MVC.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DefaultAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public IIdentity User { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
           // Check.Argument.IsNotNull(filterContext, "filterContext");

            if (filterContext.IsChildAction)
            {
                return;
            }
            if (IsAuthorized(filterContext))
            {
                User = filterContext.HttpContext.User.Identity;
                HandleRoleAuthorized(filterContext);
                var cachePolicy = filterContext.HttpContext.Response.Cache;

                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, filterContext);
            }
            else
            {
                HandleUnauthorized(filterContext);
            }
        }
        protected virtual void HandleRoleAuthorized(AuthorizationContext filterContext)
        {

        }
        protected virtual void HandleUnauthorized(AuthorizationContext filterContext)
        {
           // Check.Argument.IsNotNull(filterContext, "filterContext");

            var isAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            //判断是否是Ajax登录，如果是，返回Ajax信息
            if (isAjax)
            {
                filterContext.Result = new RestUnauthorizedResult();
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public virtual bool IsAuthorized(AuthorizationContext filterContext)
        {
           // Check.Argument.IsNotNull(filterContext, "filterContext");

            IPrincipal principal = filterContext.HttpContext.User;

            return principal.Identity.IsAuthenticated;
        }


        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization((AuthorizationContext)data);
        }

        protected virtual HttpValidationStatus OnCacheAuthorization(AuthorizationContext filterContext)
        {
          //  Check.Argument.IsNotNull(filterContext, "filterContext");

            bool authorized = IsAuthorized(filterContext);

            return authorized ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }
    }
}
