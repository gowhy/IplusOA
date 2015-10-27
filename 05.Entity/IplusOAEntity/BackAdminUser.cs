using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
   public class BackAdminUser
    {
        //public string UserName { get; set; }
        //public string PassWord { get; set; }
        //public int Id { get; set; }
        //public int RoleId { get; set; }

       public string LoginToken { get; set; }
        public string Msg { get; set; }

        /// <summary>
        /// auto_increment
        /// </summary>
        public int Id {  set ; get ;}
        /// <summary>
        /// 
        /// </summary>
        public string UserName{set; get ; }
        /// <summary>
        /// 
        /// </summary>
        public string PassWord{set ;get; }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime AddTime{set;get;}
        /// <summary>
        /// 
        /// </summary>
        public int? RoleId {set; get ;  }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("Department")]
        public string DeptId { set;get ; }
        /// <summary>
        /// 
        /// </summary>
        public string RealName{set ; get ;}
        /// <summary>
        /// 
        /// </summary>
        public int? State { set;get ; }
        /// <summary>
        /// 
        /// </summary>
       [ForeignKey("SocialOrg")]
        public int? SocOrgId { set;get ; }
     
        public RoleEntity Role { get; set; }

        public DepartmentEntity Department { get; set; }

        public SocialOrgEntity SocialOrg { get; set; }
        public string Phone { get; set; }
    }
}
