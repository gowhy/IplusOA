using Common;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BackWebAdmin
{
    public class SMSComm
    {

        /// <summary>
        /// 发送登陆验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static ReturnModel SendLoginCodeSMS(string phone)
        {
            ReturnModel ret = new ReturnModel();
      

            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
            string code = value.ToString();
            string msg = "【社区1+1】欢迎你，您的登陆验证码是：" + code + "。";

            SMSEntity entity = new SMSEntity();
            entity.AddTime = DateTime.Now;
            entity.Msg = msg;
            entity.VCode = code.Trim();
            entity.BType = 2;
            entity.Phone = phone;

            ReturnModel retMSM = SendSMS(entity.Phone, msg);
            if (retMSM.State == 1)
            {
                using (IplusOADBContext db = new IplusOADBContext())
                {
                    db.Add<SMSEntity>(entity);
                    db.SaveChanges();

                    ret.State = 1;
                    ret.Msg = "发送成功";//发送消息成功,保存数据库成功
                }
            }
            else
            {
                ret.State = retMSM.State;
                ret.Msg = retMSM.Msg;
            }
            return ret;
        }

        public static ReturnModel SendSMS(string phone, string sendMsg)
        {

            sendMsg = HttpUtility.UrlEncode(sendMsg, Encoding.GetEncoding("GBK"));

            ReturnModel ret = new ReturnModel();
            try
            {
                string SMSUserName = System.Configuration.ConfigurationManager.AppSettings["SMSUserName"];
                string SMSPassWord = System.Configuration.ConfigurationManager.AppSettings["SMSPassWord"];
                string SMSServiceUrl = System.Configuration.ConfigurationManager.AppSettings["SMSServiceUrl"];

                HttpItem parm = new HttpItem();
                parm.ResultType = ResultType.String;
                parm.URL = string.Format("{0}?name={1}&password={2}&mobile={3}&message={4}",
                    SMSServiceUrl, SMSUserName, SMSPassWord, phone, sendMsg);

                HttpHelper httpHelper = new HttpHelper();
                HttpResult httpResult = httpHelper.GetHtml(parm);
                string res = httpResult.Html;
                if (httpResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string[] str = res.Split(',');
                    if (str[0] == "succ")
                    {
                            ret.Msg = "发送成功";
                            ret.State = 1;
                    }
                    else
                    {
                        ret.Msg  = "发送失败,返回内容：" + httpResult.StatusCode + "   " + res;
                        ret.State = -1;
                    }
                }
                else
                {
                    ret.Msg = "发送失败,返回内容：" + httpResult.StatusCode + "   " + res;
                    ret.State = -2;
                }
            }
            catch (Exception ex)
            {
                ret.Msg = ex.Message + ex.InnerException + ex.Source + ex.TargetSite + ex.StackTrace;
                ret.State = -2;
            }
            return ret;
        }



        /// <summary>
        /// 商户登陆发送手机验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static ReturnModel SendLoginCodeSMSBySeller(string phone)
        {
            ReturnModel ret = new ReturnModel();


            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
            string code = value.ToString();
            string msg = "【社区1+1】欢迎你，您的登陆验证码是：" + code + "。";

            SMSEntity entity = new SMSEntity();
            entity.AddTime = DateTime.Now;
            entity.Msg = msg;
            entity.VCode = code.Trim();
            entity.BType = 3;
            entity.Phone = phone;

            ReturnModel retMSM = SendSMS(entity.Phone, msg);
            if (retMSM.State == 1)
            {
                using (IplusOADBContext db = new IplusOADBContext())
                {
                    db.Add<SMSEntity>(entity);
                    db.SaveChanges();

                    ret.State = 1;
                    ret.Msg = "发送成功";//发送消息成功,保存数据库成功
                }
            }
            else
            {
                ret.State = retMSM.State;
                ret.Msg = retMSM.Msg;
            }
            return ret;
        }
    }
}