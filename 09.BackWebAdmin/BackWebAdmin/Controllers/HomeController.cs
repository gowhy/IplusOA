using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "Home-主页")]
    public class HomeController : BaseController
    {

        //public ActionResult Index()
        //{
        //    var s = AdminUser;

        //    return View();
        //}
        [SecurityNode(Name = "测试API")]
        public JsonResult TestAPI()
        {
            string userToken = "8F93F21C92FA8B926F44D993CE0B36919A7F4EA1394430E08883AE157CF3EA7B504E1AD4E9FE9E6FF0886EA8CB4886DA7A8DC20A9B16A97B3939AAFC10E7B3472A34E8294BE5D4C887188AE59F64548422034FD2F48D5797A81F11D8AE6F9C8062B0DF96992885FD6E60DC38F5D7D61EDA342A6D2465B052A271019236898C12733D7FB1";
            base.SetUserToken(userToken);
            return Json(AdminUser);
        }

        [SecurityNode(Name = "默认Index页")]
        //
        // GET: /Default/
        //  [OutputCache(Duration = 6000)]
        public ActionResult Index()
        {
            IplusOADBContext db = new IplusOADBContext();
            var list = db.MenuEntityTable.AsQueryable<MenuEntity>().ToList();

            var rolelist = db.RolePermission.AsQueryable().Where(x => x.RoleId == AdminUser.RoleId).ToList();

            db.Dispose();

            List<MenuEntity> userMenuList = new List<MenuEntity>();
            foreach (var ritem in rolelist)
            {
                if (!string.IsNullOrEmpty(ritem.Module))
                {

                    userMenuList.AddRange(list.Where(x => x.Module.ToLower() == ritem.Module.ToLower()));
                }
                else
                {
                    userMenuList.AddRange(list.Where(x => x.file.Trim().ToLower() == "/" + ritem.Node.Trim().ToLower().Replace('_', '/')));
                }
            }

            List<MenuEntity> Root = userMenuList.Where(x => x.pId == 0).ToList();

            foreach (var item in Root)
            {
                item.ChildList = userMenuList.Where(x => x.pId == item.id).ToList();
            }

            return View(Root);
        }

        // [OutputCache(Duration = 6000)]
        public ActionResult Top()
        {
            ViewBag.username = AdminUser.UserName;
            var nav = SiteMapManager.SiteMaps.DefaultSiteMap.RootNode.ChildNodes;

            return View(nav);
        }

        //   [OutputCache(Duration = 6000)]
        public ActionResult Left3(string key)
        {
            //IplusOADBContext db = new IplusOADBContext();
            //var list = db.MenuEntityTable.AsQueryable<MenuEntity>().ToList();

            //db.Dispose();


            //ViewData["menuJsonDate"] = ToJson(list);

            IplusOADBContext db = new IplusOADBContext();
            var list = db.MenuEntityTable.AsQueryable<MenuEntity>().ToList();
            db.Dispose();


            var smap = list.Where(x => x.pId == 0);

            List<Common.SiteMapNode> list2 = new List<Common.SiteMapNode>();
            foreach (var item in smap)
            {
                Common.SiteMapNode tmp = new Common.SiteMapNode();
                tmp.ActionName = item.name;
                tmp.Url = item.file;

                var cmap = list.Where(x => x.pId == item.id);
                foreach (var item2 in cmap)
                {
                    Common.SiteMapNode tmp2 = new Common.SiteMapNode();
                    tmp2.ActionName = item2.name;
                    tmp2.Url = item2.file;

                    tmp.ChildNodes.Add(tmp2);
                }
                list2.Add(tmp);
            }




            return View(list2);

        }

        public ActionResult Left(string key)
        {
            IplusOADBContext db = new IplusOADBContext();
            var list = db.MenuEntityTable.AsQueryable<MenuEntity>().ToList();

            var rolelist = db.RolePermission.AsQueryable().Where(x => x.RoleId == AdminUser.RoleId).ToList();

            db.Dispose();

            List<MenuEntity> userMenuList = new List<MenuEntity>();
            foreach (var ritem in rolelist)
            {
                if (!string.IsNullOrEmpty(ritem.Module))
                {

                    userMenuList.AddRange(list.Where(x => x.Module.ToLower() == ritem.Module.ToLower()));
                }
                else
                {
                    userMenuList.AddRange(list.Where(x => x.file.Trim().ToLower() == "/" + ritem.Node.Trim().ToLower().Replace('_', '/')));
                }
            }

            //ViewData["menuJsonDate"] = HelpSerializer.JSONSerialize<List<MenuEntity>>(userMenuList);
            return View(userMenuList);

        }

        public string ToJson(IList<MenuEntity> customer)
        {

            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(List<MenuEntity>));
            MemoryStream ms = new MemoryStream();

            ds.WriteObject(ms, customer);

            string strReturn = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return strReturn;
        }


        [OutputCache(Duration = 300)]
        public ActionResult Main()
        {
            var now = DateTime.Now.Date;
            var threeDay = now.AddDays(2);

            return View();
        }

        public ActionResult RemoveCache()
        {
            var url = Url.Action("Main", "Home");
            if (url != null) HttpResponse.RemoveOutputCacheItem(url);
            return RedirectToAction("Main");
        }

        public ActionResult Drag()
        {
            return View();
        }

        public ActionResult Footer()
        {
            return View();
        }
        public ActionResult Tree()
        {
            return View();
        }
    }
}
