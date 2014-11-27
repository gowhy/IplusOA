using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class SMSEntity
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Msg { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Code { get; set; }
    }
}
