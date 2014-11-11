using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class SerRecordEntity
    {
        public int Id { get; set; }
        public string No { get; set; }
        public Nullable<int> SDId { get; set; }
        public Nullable<int> SocOrgId { get; set; }
        public int VId { get; set; }
        public Nullable<System.DateTime> BeginTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Img { get; set; }
        public string Pos { get; set; }
        public string SerId { get; set; }
        public string City { get; set; }
        public string SocID { get; set; }
        public Nullable<int> Socre { get; set; }
        public string Comment { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Source { get; set; }

        public int UASId { get; set; }
    }
}
