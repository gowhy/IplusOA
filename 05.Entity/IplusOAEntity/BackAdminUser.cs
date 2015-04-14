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
        #region Model
        private int _id;
        private string _username="";
        private string _password;
        private DateTime _addtime =DateTime.Now;
        private int? _roleid;
        private string _deptid;
        private string _realname;
        private int? _state;
        private int? _socorgid;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PassWord
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("Department")]
        public string DeptId
        {
            set { _deptid = value; }
            get { return _deptid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
       [ForeignKey("SocialOrg")]
        public int? SocOrgId
        {
            set { _socorgid = value; }
            get { return _socorgid; }
        }
        #endregion Model
        public RoleEntity Role { get; set; }

        public DepartmentEntity Department { get; set; }

        public SocialOrgEntity SocialOrg { get; set; }
        public string Phone { get; set; }
    }
}
