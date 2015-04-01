using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class VillageCompany
    {
        public int Id { get; set; }
        public DateTime AddTime { get; set; }
        public int AddUserId { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string LinkMan { get; set; }
        public string WebSite { get; set; }
        public string MangerVillage { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string MangerVillageName { get; set; }
        public string Desc { get; set; }

        public string DeptId { get; set; }
    }
}
