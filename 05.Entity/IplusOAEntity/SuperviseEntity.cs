using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public partial class SuperviseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DepId { get; set; }
        public string VDeptId { get; set; }
        public string Content { get; set; }
        public string ImgUrl { get; set; }
        public int AddUser { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Msg { get; set; }
        public int State { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }
        public string Address { get; set; }
    }

    public partial class SuperviseEntityClone
    {
        public int Id { get; set; }
        public string DepId { get; set; }
        public string VDeptId { get; set; }
        public string Content { get; set; }
        public string ImgUrl { get; set; }
        public int AddUser { get; set; }
        public string DepName { get; set; }
        public VolunteerEntity volEntity { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Msg { get; set; }
        public int State { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 网格管理员
        /// </summary>
        public GridMember GridMember { get; set; }
        
    }
}
