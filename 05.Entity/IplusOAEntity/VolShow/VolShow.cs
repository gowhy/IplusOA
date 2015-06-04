using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class VolShow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string UploadHtmlFile { get; set; }
        public string ImgUrl { get; set; }
        public string Des { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string DepId { get; set; }
        public int State { get; set; }
        public int AddUserId { get; set; }
        public string VolType { get; set; }//志愿者类型

        public List<VolShowHtmlFile> VolShowHtmlFileList { get; set; }
       

    }
   public class VolShow_HtmlFile
   {
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Id { get; set; }
       public System.DateTime AddTime { get; set; }
       public string UploadHtmlFile { get; set; }
       public string ImgUrl { get; set; }
       public string Des { get; set; }
       public string Phone { get; set; }
       public string Name { get; set; }
       public string DepId { get; set; }
       public int State { get; set; }
       public int AddUserId { get; set; }
       public string VolType { get; set; }//志愿者类型

       public List<VolShowHtmlFile> VolShowHtmlFileList { get; set; }


   }
}
