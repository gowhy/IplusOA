using BackWebAdmin.CommService;
using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackWebAdmin.CommService
{
    public class PushMsg
    {
        public static void Send(int userId, string msg, string url = "url")
        {

            using (IplusOADBContext db = new IplusOADBContext())
            {
                var UserList = from c in db.AppMsgSendTable where c.UserId == userId select c.TCode;
                if (UserList != null)
                {
                    string[] uStringList = null;
                    uStringList = UserList.ToPagedList(0, 999).ToArray();
                    if (uStringList != null && uStringList.Length > 0)
                    {
                        //string failMsg = "您于" + model.AddTime.ToString("yyyy-MM-dd HH:mm") + "投诉的" + msg + "，管理员已回复";
                        //WindowsFormsApplication1.Form1.PushObject_all_regId_alert(failMsg, "", uStringList);
                        WindowsFormsApplication1.Form1.AsyncPush(WindowsFormsApplication1.Form1.PushObject_all_regId_alert(msg, url, uStringList));
                    }
                }
            }
        }
    }
}