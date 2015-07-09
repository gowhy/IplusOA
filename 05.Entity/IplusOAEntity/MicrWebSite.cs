using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class MicrWebSite
    {
        public int Id { get; set; }
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }
        public string Name { get; set; }
        public string DeptId { get; set; }
        public string ImgUrl { get; set; }
        public string LinkUrl { get; set; }
        public string Type { get; set; }
        public string UploadHtmlFile { get; set; }
       
       
    }
}
