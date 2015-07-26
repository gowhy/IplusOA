using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{

   public class ChangeGoods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string Desc { get; set; }
        public System.DateTime AddTime { get; set; }

        public string Barcode { get; set; }
        public string ImgUrl { get; set; }
        public int Count { get; set; }

        public int AddUserId { get; set; }
        public string CoverCommunity { get; set; }
    }
}
