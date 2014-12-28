using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class PrizeRecord
    {
        public int Id { get; set; }
        public int AprizeId { get; set; }

        public int UserId { get; set; }
        public int AddUserId { get; set; }
        
        public DateTime AddTime { get; set; }
        public string Desc { get; set; }

    }
}
