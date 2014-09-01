using System;
using System.Linq;
using System.Web.Mvc;
//using QDT.Common;
//using QDT.Common.Data;
//using QDT.Core;
//using QDT.Core.Domain;
//using QDT.MVC;
//using QDT.Services.LogMoudle;
//using QDT.Web.Admin.Code;
//using QDT.Web.Admin.Models;
using QDT.Web.Admin.Code;
using QDT.Core.Domain;
using QDT.Web.Admin.Models;
using QDT.Common;
namespace QDT.Web.Admin.Controllers
{
   
   [AdminAuthorize]
    public abstract class BaseController : Controller
    {

   

        public AdminUser AdminUser
        {
            get
            {
                var admin = HttpContext.Items["QDT_Admin"] as AdminUser;
                if (admin == null)
                {
                   // admin = DbProvider.D<AdminUser>().FirstOrDefault(x => x.Name == User.Identity.Name);
                    HttpContext.Items["QDT_Admin"] = admin;
                }
                return admin;
            }
        }

        protected ActionResult Message(MsgModel model)
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new JsonMessage(model.Type, model.Message), JsonRequestBehavior.AllowGet);
            }
            return View("_Message", model);
        }

        protected ActionResult Error(string msg)
        {
            return Error("操作失败", msg);
        }

        protected ActionResult Error(string title, string msg)
        {
            return Message(new MsgModel
                {
                    Title = title,
                    Message = msg,
                    JumpUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"),
                    WaitSecond = 3,
                    Type = false
                });
        }

        protected ActionResult Error()
        {
            bool sumErr = ModelState.ContainsKey("");
            if (!sumErr)
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError modelError in modelState.Errors)
                    {
                        if (!string.IsNullOrWhiteSpace(modelError.ErrorMessage))
                        {
                            return Error(modelError.ErrorMessage);
                        }
                    }
                }
            }

            return Error(ModelState[""].Errors[0].ErrorMessage);
        }

        protected ActionResult Success(string msg)
        {
            return Success("操作成功", msg,
                           Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"));
        }

        protected ActionResult Success(string title, string msg)
        {
            return Success(title, msg,
                           Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"));
        }

        protected ActionResult Success(string title, string msg, string jumpUrl)
        {
            return Message(new MsgModel
                {
                    Title = title,
                    Message = msg,
                    JumpUrl = jumpUrl,
                    WaitSecond = 3,
                    Type = true
                });
        }

        public void Logs(string logInfo, params object[] param)
        {
            //IoC.Resolve<IAdminLogService>()
            //   .AddAdminLog(AdminUser, logInfo.FormatWith(param), Request.Url == null ? null : Request.Url.AbsolutePath,
            //                Utility.GetIP());
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            

            if (filterContext.IsChildAction) return;

            filterContext.Result = Error(filterContext.Exception.Message);

           // Log.Error(filterContext.Exception.Message, filterContext.Exception);

            filterContext.ExceptionHandled = true;
        }
    }
}