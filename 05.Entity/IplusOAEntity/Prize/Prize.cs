using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class Prize
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string DeptId { get; set; }
        public int AddUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
      
        public DateTime AddTime { get; set; }
        public string Description { get; set; }

    }
}
