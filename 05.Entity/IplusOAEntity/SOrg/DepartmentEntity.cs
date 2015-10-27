using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{

    public class DepartmentEntity
    {
       
        /// <summary>
        /// auto_increment
        /// </summary>
        public string Id{ set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string PId {  set;get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name{ set; get; }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime AddTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? State { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Desc {set; get; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsCheck { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public Nullable<double> Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public Nullable<double> Lng { get; set; }
        // public virtual ICollection<SocialOrgEntity> ListSocialOrgEntity { get; set; }
        public int Level { get; set; }
    }
}
