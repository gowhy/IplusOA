using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class SocServiceDetailEntityClone
    {

        #region Model
        private int _id;
        private string _sernum;
        private string _type;
        private string _socialno;
        private string _context;
        private DateTime? _pubtime;
        private DateTime? _endtime;
        private string _desc;
        private string _vhelpdesc;
        private string _covercommunity;
        private int? _score = 0;
        private int? _thsscore;
        private string _phone;
        private string _contacts;
        private DateTime _addtime = DateTime.Now;
        private string _adduser;

        public string PayType { get; set; }

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
        public string SerNum
        {
            set { _sernum = value; }
            get { return _sernum; }
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
        public string SocialNo
        {
            set { _socialno = value; }
            get { return _socialno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Context
        {
            set { _context = value; }
            get { return _context; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? PubTime
        {
            set { _pubtime = value; }
            get { return _pubtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Desc
        {
            set { _desc = value; }
            get { return _desc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string VHelpDesc
        {
            set { _vhelpdesc = value; }
            get { return _vhelpdesc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CoverCommunity
        {
            set { _covercommunity = value; }
            get { return _covercommunity; }
        }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string CoverCommunityNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Score
        {
            set { _score = value; }
            get { return _score; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? THSScore
        {
            set { _thsscore = value; }
            get { return _thsscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Contacts
        {
            set { _contacts = value; }
            get { return _contacts; }
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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string SocialName { get; set; }

        public List<SocSerImgEntity> SocSerImgs { get; set; }

      //  public UserApplyServiceEntity  UserApplyEntity { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public SerRecordEntity SerRecord { get; set; }
        #endregion Model
    }
}
