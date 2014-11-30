using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class SystemMsgEntity
    {
      
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AddUser { get; set; }
        public System.DateTime BeginTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int State { get; set; }
    }
}
