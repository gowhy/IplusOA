using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class SingleLoginCheck
    {
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }

        public int UserId { get; set; }

        public string PCode { get; set; }
        public string Phone { get; set; }
        public string LoginToken { get; set; }
        public int State { get; set; }
        
    }
}
