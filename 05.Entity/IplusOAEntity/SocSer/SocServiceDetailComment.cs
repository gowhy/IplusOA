using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class SocServiceDetailComment
    {
        public int Id { get; set; }
        public int SdId { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public DateTime AddTime { get; set; }
    }
}
