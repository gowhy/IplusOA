using BackWebAdmin.Models;
using Common;
using Common.Cache;
using DataLayer.IplusOADB;
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

        public ICacheProvider CacheManger = new WebCacheProvider();
        public BaseController()
        {


        }

        protected BackAdminUser GetBackUserInfo()
        {
            BackAdminUser backUser = new BackAdminUser();
            using (IplusOADBContext db = new IplusOADBContext())
            {

                if (AdminUser != null && AdminUser.Id > 0)
                {
                    var vol = db.BackAdminUserEntityDBSet;
                    backUser = (from v in vol where v.Id == AdminUser.Id select v).FirstOrDefault();
                }
            }
            return backUser;
        }

        protected VolunteerEntity GetAppUserInfo()
        {
            VolunteerEntity backUser = new VolunteerEntity();
            using (IplusOADBContext db = new IplusOADBContext())
            {

                if (AdminUser != null && AdminUser.Id > 0)
                {
                    var vol = db.VolunteerEntityTable;
                    backUser = (from v in vol where v.Id == AdminUser.Id select v).FirstOrDefault();
                }
            }
            return backUser;
        }
        protected VolunteerEntity GetAppUserInfoById(int userId)
        {
            VolunteerEntity backUser = new VolunteerEntity();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var vol = db.VolunteerEntityTable;
                backUser = (from v in vol where v.Id == userId select v).FirstOrDefault();
            }
            return backUser;
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


            LogEntity model = new LogEntity();
            model.Module = "OA";
            model.UserId = AdminUser.UserName;
            model.Type = "Exception";
            model.Content = filterContext.Exception.Message + filterContext.Exception.InnerException
                + filterContext.Exception.Source + filterContext.Exception.StackTrace + filterContext.Exception.TargetSite;
            //  log4net.LogManager.GetLogger(this.GetType()).Error(filterContext.Exception.Message, filterContext.Exception);
            using (IplusOADBContext db = new IplusOADBContext())
            {
                model.AddTime = DateTime.Now;
                db.Add<LogEntity>(model);
                db.SaveChangesAsync();
            }
            filterContext.Result = Error(filterContext.Exception.Message);
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

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
         
        }
       
    }
}