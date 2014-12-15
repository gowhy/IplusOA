using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using BackWebAdmin.CommService;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using BackWebAdmin.Models;
using System.IO;
using Common.Dynamic;

namespace BackWebAdmin.Controllers
{
    [SecurityModule(Name = "App消息推送")]
    public class AppMsgSendController : BaseController
    {
        [SecurityNode(Name = "列表页")]
        // GET: AppMsgSend
        public ActionResult Index()
        {
            return View();
        }
    }
}