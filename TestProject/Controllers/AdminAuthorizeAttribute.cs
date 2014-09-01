using QDT.MVC.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace QDT.Web.Admin.Code
{
    public class AdminAuthorizeAttribute : RoleAuthorizeAttribute
    {
        protected override void HandleRoleAuthorized(AuthorizationContext filterContext)
        {
            if(!IsPermission(filterContext)){
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Action = "NoPermission", Controller = "Account" }));
            }
        }

        protected override bool IsPermission(AuthorizationContext filterContext)
        {
            return false;
           // return IoC.Resolve<IAdminService>().GetAdmin(User.Name).IsDefaultAdmin || base.IsPermission(filterContext);
        }

    }
}