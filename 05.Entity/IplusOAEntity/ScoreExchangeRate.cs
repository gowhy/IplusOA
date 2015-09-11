using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
  public  class ScoreExchangeRate
    {

        public int Id { get; set; }
        public double Rate { get; set; }
        public int RateBase { get; set; }
        public int AddUserId { get; set; }
        public string AddUserDeptId { get; set; }
        public string DepId { get; set; }
        public DateTime AddTime { get; set; }
    }
}
