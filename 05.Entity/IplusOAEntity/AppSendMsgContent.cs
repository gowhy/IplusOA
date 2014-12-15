using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class AppSendMsgContent
    {

        public int Id { get; set; }
        public int AddUserId { get; set; }
        public string Content { get; set; }
        public DateTime AddTime { get; set; }
        public int State { get; set; }
       
    }
}
