using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class GridMember
    {
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string DeptId { get; set; }
        public string VDeptId { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string VDeptName{ get; set; }
        public string GridNo { get; set; }
        public string GridName { get; set; }

        public string GridPhone { get; set; }
        public string GridHeaderImg { get; set; }
        public int AddUserId { get; set; }
        public string Desc { get; set; }
    }
}
