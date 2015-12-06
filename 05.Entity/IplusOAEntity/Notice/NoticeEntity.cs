using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class NoticeEntity
    {
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string DepId { get; set; }
        public string ImgUrl { get; set; }
        public string Des { get; set; }
        public string Title { get; set; }
        public string AddUser { get; set; }
        public int State { get; set; }

        public string UploadHtmlFile { get; set; }
        public string LinkSocSerUrl { get; set; }
    }
   public class NoticeEntityClone
   {
       public int Id { get; set; }
       public System.DateTime AddTime { get; set; }
       public string DepId { get; set; }
       public string DepName { get; set; }
       
       public string ImgUrl { get; set; }
       public string Des { get; set; }
       public string Title { get; set; }
       public string AddUser { get; set; }
       public int State { get; set; }
       public string UploadHtmlFile { get; set; }
       public string LinkSocSerUrl { get; set; }
   }
   public enum NoticePushState
   {
       未推送 = 0,
       推送中 = 1,
       推送完成 = 2

   }
}
