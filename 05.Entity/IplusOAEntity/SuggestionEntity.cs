using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public partial class SuggestionEntity
    {
        public int Id { get; set; }
        public string DepId { get; set; }
        public string Content { get; set; }
        public int AddUser { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}
