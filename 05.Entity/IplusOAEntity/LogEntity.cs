using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class LogEntity
    {
   
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string Module { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Type { get; set; }
  
    }
}
