using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
  public  class Seller
    {
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Phone { get; set; }
        public string SellName { get; set; }
        public string DeptId { get; set; }
        public int AddUserId { get; set; }
      
    }
}
