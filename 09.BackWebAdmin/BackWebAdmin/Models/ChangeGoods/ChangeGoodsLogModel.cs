using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;

namespace BackWebAdmin
{
    public class ChangeGoodsLogModel
    {
        public VolunteerEntity User { get; set; }
        public ChangeGoodsLog CGoodsLogs { get; set; }
        public ChangeGoods CGoods { get; set; }
    }
}