using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    /// <summary>
    /// 大屏统计数量
    /// </summary>
   public class DpEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int old { get; set; }
        public int now { get; set; }
        public int Future { get; set; }

        public int Count { get; set; }

        public int OderBy { get; set; }

        public IList<DpEntity> Chlids { get; set; }
    }


   public class DpChlidEntity
   {
       public string Name { get; set; }
       public string Code { get; set; }
       public int OldCount { get; set; }
       public int NowCount { get; set; }
       public int FutureCount { get; set; }

       public int Count { get; set; }

       public int OderBy { get; set; }

   }
}
