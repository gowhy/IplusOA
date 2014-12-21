using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using DataLayer.IplusOADB;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using cn.jpush.api.push;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        #region 测试数据
        public static String TITLE = "社区1+1测试消息推送";
        public static String ALERT = "测试推送消息内容---韦";
        public static String MSG_CONTENT = "Test from C# v3 sdk - msgContent";
        public static String REGISTRATION_ID = "040bfc1ddfa";
        #endregion

        public static String TAG = "tag_api";
        public static String app_key = "35b9a424c942fc42f3b32dfb";
        public static String master_secret = "54749844fa6b0085348694ff";
        public static bool RunState = false;
        System.Timers.Timer myTimer; 
        public Form1()
        {
          
          
            InitializeComponent();
         
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            myTimer = new System.Timers.Timer(2000);//定时周期2秒
            myTimer.Elapsed += btnStart_Click;//到2秒了做的事件
            myTimer.AutoReset = true; //是否不断重复定时器操作
            myTimer.Enabled = true;
        }
        static  JPushClient client = new JPushClient(app_key, master_secret);


        private void btnStart_Click(object sender, EventArgs e)
        {
      
            RunState = true;
            DoAction();
        }

        public void DoAction()
        {


            try
            {
                Console.WriteLine("开始运行时间: " + DateTime.Now);
                AsyncPushNotice();
                AsyncPushWorkGuide();
                Console.WriteLine("结束运行时间: " + DateTime.Now);
            }
            catch (APIRequestException ex)
            {
                Console.WriteLine("Error response from JPush server. Should review and fix it. ");
                Console.WriteLine("HTTP Status: " + ex.Status);
                Console.WriteLine("Error Code: " + ex.ErrorCode);
                Console.WriteLine("Error Message: " + ex.ErrorCode);
            }
            catch (APIConnectionException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async static Task<MessageResult> AsyncPushNotice()
        {
            Thread.CurrentThread.IsBackground = true;
            MessageResult res = await PushNotice();
            return res;
        }
        public async static Task<MessageResult> PushNotice()
        {
          
            string IUrl = "Notice/AppView";
            PushPayload payload = null;
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var noticeTable = db.NoticeTable;
                NoticeEntity noticeModel = noticeTable.OrderBy(x=>x.Id).FirstOrDefault(x => x.State == 1 && x.DepId != null);
                if (noticeModel==null)
                {
                    return new MessageResult();
                }
                noticeModel.State = 2;
                db.Update<NoticeEntity>(noticeModel);
                db.SaveChanges();

                var UserList = from c in db.AppMsgSendTable
                               join v in db.VolunteerEntityTable on c.UserId equals v.Id
                               where v.DepId == noticeModel.DepId
                               select c.TCode;

                string[] uStringList = null;
                int i = 0;
                do
                {

                    uStringList = UserList.ToPagedList(i, 999).ToArray();
                    if (uStringList == null || uStringList.Length == 0)
                    {
                        break;
                    }
                    payload = PushObject_all_regId_alert(noticeModel.Title, IUrl + "?id=" + noticeModel.Id, uStringList);
                    i++;
                }
                while (uStringList != null && uStringList.Length == 999);


            }
            var ret = client.SendPush(payload);
            return ret;
          
        }


        public async static Task<MessageResult> AsyncPushWorkGuide()
        {
          
            MessageResult res = await PushWorkGuide();
            return res;
        }
        public async static Task<MessageResult> PushWorkGuide()
        {

            PushPayload payload = null;
            string IUrl = "WorkGuide/AppView";
            using (IplusOADBContext db = new IplusOADBContext())
            {
                var noticeTable = db.WorkGuideTable;
                WorkGuideEntity noticeModel = noticeTable.OrderBy(x => x.Id).FirstOrDefault(x => x.State == 1 && x.DepId != null);
                if (noticeModel == null)
                {
                    return new MessageResult();
                }
                noticeModel.State = 2;
                db.Update<WorkGuideEntity>(noticeModel);
                db.SaveChanges();

                var UserList = from c in db.AppMsgSendTable
                               join v in db.VolunteerEntityTable on c.UserId equals v.Id
                               where v.DepId == noticeModel.DepId
                               select c.TCode;

                string[] uStringList = null;
                int i = 0;
                do
                {

                    uStringList = UserList.ToPagedList(i, 999).ToArray();
                    if (uStringList == null || uStringList.Length == 0)
                    {
                        break;
                    }
                    payload = PushObject_all_regId_alert(noticeModel.Title, IUrl+"?id="+noticeModel.Id, uStringList);
                    i++;
                }
                while (uStringList != null && uStringList.Length == 999);


            }
            var ret = client.SendPush(payload);
            return ret;

        }

        public async static Task<MessageResult> AsyncPush(PushPayload payload)
        {
            var result = client.SendPush(payload);
            return result;
        
        }
        public async static Task<MessageResult> PushLoadMsg(PushPayload payload)
        {
            var result = client.SendPush(payload);
            return result;
        }

        public static PushPayload PushObject_All_All_Alert()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.all();
            pushPayload.notification = new Notification().setAlert(ALERT);
            return pushPayload;
        }
        public static PushPayload PushObject_all_alias_alert()
        {

            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_alias("alias1");
            pushPayload.notification = new Notification().setAlert(ALERT);
            return pushPayload;

        }
        
        public static PushPayload PushObject_all_regId_alert(string msg,string url,params string[] values)
        {

            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();


            AndroidNotification andorid = new AndroidNotification();
            andorid.AddExtra("IUrl", url);
          
            IosNotification ios = new IosNotification();
            ios.AddExtra("IUrl", url);

            pushPayload.audience = Audience.s_registrationId(values);

            Notification notification = new Notification();
            notification.setAlert(msg);
            notification.setAndroid(andorid);
            notification.setIos(ios);

            pushPayload.notification = notification;
            return pushPayload;

        }

        public static PushPayload PushObject_Android_Tag_AlertWithTitle()
        {
            PushPayload pushPayload = new PushPayload();

            pushPayload.platform = Platform.android();
            pushPayload.audience = Audience.s_tag("tag1");
            pushPayload.notification = Notification.android(ALERT, TITLE);

            return pushPayload;
        }
        public static PushPayload PushObject_android_and_ios()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            var audience = Audience.s_tag("tag1");
            pushPayload.audience = audience;
            var notification = new Notification().setAlert("alert content");
            notification.AndroidNotification = new AndroidNotification().setTitle("Android Title");
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("extra_key", "extra_value");

            pushPayload.notification = notification.Check();


            return pushPayload;
        }
        public static PushPayload PushObject_ios_tagAnd_alertWithExtrasAndMessage()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag_and("tag1", "tag_all");
            var notification = new Notification();
            notification.IosNotification = new IosNotification().setAlert(ALERT).setBadge(5).setSound("happy").AddExtra("from", "JPush");

            pushPayload.notification = notification;
            pushPayload.message = cn.jpush.api.push.mode.Message.content(MSG_CONTENT);
            return pushPayload;

        }
        public static PushPayload PushObject_ios_audienceMore_messageWithExtras()
        {

            var pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_tag("tag1", "tag2");
            pushPayload.message = cn.jpush.api.push.mode.Message.content(MSG_CONTENT).AddExtras("from", "JPush");
            return pushPayload;

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            RunState = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

     
    }
}
