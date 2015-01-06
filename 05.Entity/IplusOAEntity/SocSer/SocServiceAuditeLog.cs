using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class SocServiceAuditeLog
    {
        public int Id { get; set; }

        public int SDId { get; set; }
        public int AuditeUserId { get; set; }
        public DateTime AddTime { get; set; }
        public string Msg { get; set; }
        public int State { get; set; }

    }
}
