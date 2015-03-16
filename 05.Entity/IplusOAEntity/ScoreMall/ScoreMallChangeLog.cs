using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
 public   class ScoreMallChangeLog
    {
        public int Id { get; set; }
        public DateTime AddTime { get; set; }
        public int UserId { get; set; }
        public int ScoreMallId { get; set; }
        public int UseScore { get; set; }

    }
}
