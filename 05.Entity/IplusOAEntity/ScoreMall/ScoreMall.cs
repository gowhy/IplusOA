using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    /// <summary>
    /// 积分商城
    /// </summary>
   public class ScoreMall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddTime { get; set; }

        public string Desc { get; set; }
        public string ImgUrl { get; set; }


        public int Count { get; set; }
        public int UseScore { get; set; }
        public int Price { get; set; }

        public int State { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int AddUserId { get; set; }

    }
}
