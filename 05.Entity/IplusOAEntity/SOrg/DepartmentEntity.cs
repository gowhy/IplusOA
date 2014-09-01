using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{

    public class DepartmentEntity
    {
        private long? _id;
        private long? _pid;
        private string _name;
        private DateTime _addtime = DateTime.Now;
        private int? _state = 1;
        private string _desc;
        /// <summary>
        /// auto_increment
        /// </summary>
        public long? Id
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
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
        public int? State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Desc
        {
            set { _desc = value; }
            get { return _desc; }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public bool IsCheck { get; set; }

        // public virtual ICollection<SocialOrgEntity> ListSocialOrgEntity { get; set; }
    }
}
