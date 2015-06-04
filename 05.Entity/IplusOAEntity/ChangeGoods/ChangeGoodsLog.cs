using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class ChangeGoodsLog
    {
        public int Id { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Desc { get; set; }
        public int ChangeGoodsId { get; set; }
        public int AdminUserId { get; set; }
        public int ChangeCount { get; set; }
        
        public int UserId { get; set; }

    }
}
