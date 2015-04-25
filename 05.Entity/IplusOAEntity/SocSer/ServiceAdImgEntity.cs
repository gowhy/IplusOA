using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class ServiceAdImgEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string HttpUrl { get; set; }
        public string SNo { get; set; }
        public string Des { get; set; }
        public int State { get; set; }
     
        public string CoverCommunity { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string CoverCommunityNames { get; set; }
    }

    public class ServiceAdImgEntityClone
   {
     
       public int Id { get; set; }
       public System.DateTime AddTime { get; set; }
       public string HttpUrl { get; set; }
       public string SNo { get; set; }
       public string Des { get; set; }
       public string DepName { get; set; }
       public int State { get; set; }
       public string CoverCommunity { get; set; }
       [System.ComponentModel.DataAnnotations.Schema.NotMapped]
       public string CoverCommunityNames { get; set; }
       private List<string> httpUrlList;

       public List<string> HttpUrlList
       {
           get
           {
               if (httpUrlList == null || httpUrlList.Count==0 )
               {
                   httpUrlList = new List<string>();
                   if (!string.IsNullOrEmpty(HttpUrl))
                   {
                       string[] tmp = HttpUrl.Split('#');
                       foreach (var item in tmp)
                       {
                           if (!string.IsNullOrEmpty(item))
                           {
                               httpUrlList.Add(item);
                           }
                       }

                   }
               }
               return httpUrlList;
           }
           set { httpUrlList = value; }
       }
       
   }
}
