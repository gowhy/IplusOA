using BackWebAdmin.Models;
using Common;
using IplusOAEntity;
using SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackWebAdmin.Controllers
{
    [BaseAuthenticationAttribute]
    public class BaseController : AdminController
    {
        public BaseController()
        {
          
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

   

        protected override void OnException(ExceptionContext filterContext)
        {


            if (filterContext.IsChildAction) return;

            filterContext.Result = Error(filterContext.Exception.Message);

            log4net.LogManager.GetLogger(this.GetType()).Error(filterContext.Exception.Message, filterContext.Exception);

            filterContext.ExceptionHandled = true;
        }

        #region 创建模块权限
         internal RoleModel CreateEmptyRoleModel()
        {
            return CreateRoleModel(null, new PermissionCollection());
        }

         internal RoleModel CreateRoleModel(RoleEntity role, PermissionCollection permissions)
        {

            var model = role == null ? new RoleModel() : new RoleModel
            {
                ID = role.Id,
                Name = role.Name,
                IsEffect = role.IsEffect
            };

            IDictionary<string, RoleModuleModel> modules = new Dictionary<string, RoleModuleModel>();

            Common.SecurityManager.Instance.LoadAssembly(typeof(WebApiApplication).Assembly);
            Common.SecurityManager.Instance.Init();

            foreach (var node in Common.SecurityManager.Instance.Nodes)
            {
                var v = node.Value;
                if (!modules.ContainsKey(v.ModuleKey))
                {
                    modules.Add(v.ModuleKey, new RoleModuleModel { Name = v.Module, Value = v.ModuleKey, IsSelect = permissions.HasModule(v) });
                }

                modules[v.ModuleKey].RoleNodes.Add(new RoleNodeModel()
                {
                    IsDisable = false,
                    IsSelect = !modules[v.ModuleKey].IsSelect && permissions.HasNode(v),
                    Name = v.Name,
                    Value = v.NodeKey
                });
            }

            model.Modules = modules.Values.ToList();

            return model;
        }
#endregion
	}
}