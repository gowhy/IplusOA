using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
  public  class Wish
    {
     
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public Nullable< System.DateTime>  ApplyTime { get; set; }
        public Nullable<System.DateTime> AuditeTime { get; set; }
      
        public int State { get; set; }
        public int ApplyUserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string WishDesc { get; set; }
        public string Desc { get; set; }
     
    }

  public enum EnumWishState
  {
      待申请 = 0,
      申请中 = 1,
      申请通过 = 2,
      申请不通过 = 3,
  }
}
