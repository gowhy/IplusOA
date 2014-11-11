using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
  public  class UserApplyServiceEntity
    {
        public int Id { get; set; }
        public int VolId { get; set; }
        public System.DateTime AddTime { get; set; }
        public int SDId { get; set; }
        public int State { get; set; }
        public int Num { get; set; }

        public string Msg { get; set; }
    }
}
