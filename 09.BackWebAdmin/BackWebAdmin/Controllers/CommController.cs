using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.IO;
using ServiceAPI;
using System.Text;

namespace BackWebAdmin.Controllers
{
    public class CommController : Controller
    {
        const int pageSize = 20;



        public ActionResult AppGetDeptChild(string id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                if (id != null )
                {
                    var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().Where(x => x.PId == id).ToList();
                    return Json(list);
                }
                else
                {
                    var list = db.DepartmentTable.AsQueryable<DepartmentEntity>().ToList();
                    return Json(list);
                }
            }
        }
        //用户注册
        public ActionResult AppPostAddVol(VolunteerEntity entity, string code)
        {
            ReturnModel returnModel = new ReturnModel();
            try
            {


                if (entity == null || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(entity.Type) || string.IsNullOrEmpty(entity.PassWord) && (string.IsNullOrEmpty(entity.VID) && string.IsNullOrEmpty(entity.Phone)))
                {
                    Json("登陆账号、密码、验证码和用户类型是必填项", JsonRequestBehavior.AllowGet);
                }
            
                using (IplusOADBContext db = new IplusOADBContext())
                {

                     returnModel = VolService.AccountExist(entity);
                     if (returnModel == null || returnModel.State != 0)
                     {
                         return  Json(returnModel, JsonRequestBehavior.AllowGet);;
                     }

                    DateTime codeOutTime=DateTime.Now.AddMinutes(-10);
                  //  int existCount = db.SMSTable.Count(x => x.Phone == entity.Phone.Trim() && x.VCode == code.Trim() && x.AddTime< codeOutTime);
                    int existCount = db.SMSTable.Count(x => x.Phone == entity.Phone.Trim() && x.VCode == code.Trim() && x.AddTime > codeOutTime);
                    if (existCount == 0)
                    {
                        returnModel.Msg = "验证码失效,请重新获取";
                        returnModel.State = -2;
                        return Json(returnModel, JsonRequestBehavior.AllowGet);
                    }
                }

                returnModel = VolService.PostAddVol(entity, Request);
                return Json(returnModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                returnModel.Msg = ex.Message+ex.Source+ex.StackTrace+ex.TargetSite+ex.InnerException;
                returnModel.State = -4;
                return Json(returnModel, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult AccountExist(VolunteerEntity entity)
        {
            ReturnModel returnModel = VolService.AccountExist(entity);
            return Json(returnModel);
        }

        public ActionResult AppVolHeadImg(int id)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                byte[] image = (from c in db.VolunteerEntityTable where c.Id == id select c.VolHeadImg).FirstOrDefault<byte[]>();
                return new FileContentResult(image, "image/jpeg");
            }
        }
        //开机广告
        public ActionResult StartAdImgAppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var adimg = db.StartAdImgTable;

                var list = from a in adimg select a;

                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        //寻找最上级辖区开机广告
        public ActionResult StartAdImgAppStateIndex(string depId)
        {
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var adimg = db.StartAdImgTable;

                var depList = dep.ToList();
                List<StartAdImgEntity> list = adimg.ToList();
                StartAdImgEntity resultEntity = StartAdImgService.GetAppStateIndex(depId, depList, list);
                return Json(resultEntity, JsonRequestBehavior.AllowGet);
            }

        }

        //社区通知
        public ActionResult NoticeAppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var dep = db.DepartmentTable;
                var notice = db.NoticeTable;

                var list = from a in notice
                           join d in dep on a.DepId equals d.Id
                           select new NoticeEntityClone
                           {

                               DepName = d.Name,
                               DepId = a.DepId,
                               ImgUrl = a.ImgUrl,
                               Id = a.Id,
                               Des = a.Des,
                               AddTime = a.AddTime,
                               Title = a.Title
                           };
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }

        public ActionResult WorkGuideAppIndex(string depId, int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 20;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var work = db.WorkGuideTable;

                var list = from w in work select w;
                if (!string.IsNullOrEmpty(depId))
                {
                    list = list.Where(x => x.DepId == depId);
                }

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size));
            }
        }
        /// <summary>
        /// App版本检查接口
        /// </summary>
        /// <returns></returns>
        public ActionResult AppVerAppIndex()
        {
            var pageNumber = 1;
            var size = 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var adimg = db.AppVerTable;

                var list = from a in adimg select a;

                list = list.Where(x => x.State == 0);

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size), JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ServiceAdImgAppIndex(int? page, int? pageSize)
        {
            var pageNumber = page ?? 1;
            var size = pageSize ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {

                var adimg = db.ServiceAdImgTable;

                var list = from a in adimg select a;

                list = list.Where(x => x.State == 0);

                return Json(list.OrderByDescending(x => x.Id).ToPagedList(pageNumber - 1, size), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AppLog(LogEntity model)
        {
            ReturnModel res = new ReturnModel();
            using (IplusOADBContext db = new IplusOADBContext())
            {
                model.AddTime = DateTime.Now;
                db.Add<LogEntity>(model);
                db.SaveChanges();
                res.State = 1;
                res.Msg = "成功";
                return Json(res);
            }
        }

        public ActionResult AppSMS(SMSEntity entity)
        {
            try
            {
                string SMSUserName = System.Configuration.ConfigurationManager.AppSettings["SMSUserName"];
                string SMSPassWord = System.Configuration.ConfigurationManager.AppSettings["SMSPassWord"];
                string SMSServiceUrl = System.Configuration.ConfigurationManager.AppSettings["SMSServiceUrl"];


                Random rad = new Random();//实例化随机数产生器rad；
                int value = rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
                string code = value.ToString();
                string msg = "【社区1+1】欢迎成为社区1+1用户，您的注册验证码是：" + code + "。";
                msg = HttpUtility.UrlEncode(msg, Encoding.GetEncoding("GBK"));

                entity.AddTime = DateTime.Now;
                entity.Msg = msg;
                entity.VCode = code.Trim();

                HttpItem parm = new HttpItem();
                parm.ResultType = ResultType.String;
                parm.URL = string.Format("{0}?name={1}&password={2}&mobile={3}&message={4}",
                    SMSServiceUrl, SMSUserName, SMSPassWord, entity.Phone, msg);


                HttpHelper httpHelper = new HttpHelper();
                HttpResult httpResult = httpHelper.GetHtml(parm);
                string res = httpResult.Html;
                if (httpResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string[] str = res.Split(',');
                    if (str[0] == "succ")
                    {
                       
            
                        using (IplusOADBContext db = new IplusOADBContext())
                        {

                            db.Add<SMSEntity>(entity);
                            db.SaveChanges();

                            return Json(new { state = 1, msg = "发送成功" }, JsonRequestBehavior.AllowGet);

                        }

                    }
                    else
                    {
                        return Json(new { state = -1, msg = "发送失败,返回内容：" + httpResult.StatusCode + "   " + res }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { state = -2, msg = "发送失败,返回内容：" + httpResult.StatusCode + "   " + res }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new { state = -2, msg = ex.Message+ex.InnerException+ex.Source+ex.TargetSite+ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
          
        }

        public ActionResult AppSystemMsg(int? page)
        {

            var pageNumber = page ?? 1;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var msg = db.SystemMsgTable;

                var list = from m in msg where m.State == 0 && m.BeginTime < DateTime.Now && m.EndTime > DateTime.Now select m;

                return Json(list.ToPagedList(pageNumber - 1, pageSize));
            }
        }

    }
}