using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
 public   class SocSerDetailJoinEntity
    {
        private int _id;
        private int? _ssdetailid;
        private long? _depid;
        private int? _state;
        private string _msg;
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
        public int? SSDetailId
        {
            set { _ssdetailid = value; }
            get { return _ssdetailid; }
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
        public int? State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msg
        {
            set { _msg = value; }
            get { return _msg; }
        }
    }
}
