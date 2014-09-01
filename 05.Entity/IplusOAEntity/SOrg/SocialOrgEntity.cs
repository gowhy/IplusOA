using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
  public  class SocialOrgEntity
    {

        #region Model
        private int _id;
        private long? _pid;
        private long? _depid;
        private string _type;
        private string _socialno="";
        private string _name;
        private string _regno;
        private string _regtype;
        private string _orgno;
        private string _busdesc;
        private DateTime? _regtime;
        private DateTime? _effectivetime;
        private int? _state=0;
        private DateTime _addtime =DateTime.Now;
        private string _adduser="";
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
        public long? PId
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long? DepId
        {
            set { _depid = value; }
            get { return _depid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SocialNO
        {
            set { _socialno = value; }
            get { return _socialno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RegNO
        {
            set { _regno = value; }
            get { return _regno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RegType
        {
            set { _regtype = value; }
            get { return _regtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrgNo
        {
            set { _orgno = value; }
            get { return _orgno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BusDesc
        {
            set { _busdesc = value; }
            get { return _busdesc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RegTime
        {
            set { _regtime = value; }
            get { return _regtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EffectiveTime
        {
            set { _effectivetime = value; }
            get { return _effectivetime; }
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
        public string AddUser
        {
            set { _adduser = value; }
            get { return _adduser; }
        }

      //  public DepartmentEntity Department { get; set; }

        #endregion Model

    }
}
