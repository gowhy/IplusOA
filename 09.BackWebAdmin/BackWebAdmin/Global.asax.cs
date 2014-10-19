using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BackWebAdmin
{
    public class WebApiApplication : System.Web.HttpApplication
    {

       
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

             
            //CommonAction s = new CommonAction();
           
            
            ////配置基于MVC的相关配置
            //QDTMvcConfig.Configure(config =>
            //{
            //    config.SiteMap(maps => maps.Register<XmlSiteMap>("default", x => x.Load()));
            //   // config.Sercurity(typeof(MvcApplication).Assembly, x => x.RegisterService<AdminService>());
            //  //  log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
            //   // Log.SetLog(LogManager.GetLogger("Loger"));
            //}
            //    );
           
          
        }
    }
}
