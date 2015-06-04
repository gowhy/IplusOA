using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class VolShowHtmlFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string HtmlTitle { get; set; }
        public string HtmlFileUrl { get; set; }
        public int volShowId { get; set; }
       
    }
}
