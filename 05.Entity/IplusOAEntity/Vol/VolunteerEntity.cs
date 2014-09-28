using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
    public class VolunteerEntity
    {
        #region Model
    
        private string _number;
        private string _type;
        private string _depid;
        private string _address;
        private int? _realnamestate;
        private string _realname;
        private string _cardtype;
        private string _cardnum;
        private string _uername;
        private int? _honor;
        private string _phone;
        private string _email;
        private string _qq;
        private string _weixin;
        private int? _groupid;
        private int? _thsscore;
        private int? _score;
        private string _vid;

        private int? _state;
        /// <summary>
        /// auto_increment
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Number
        {
            set { _number = value; }
            get { return _number; }
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
        public string DepId
        {
            set { _depid = value; }
            get { return _depid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RealNameState
        {
            set { _realnamestate = value; }
            get { return _realnamestate; }
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
        public string CardType
        {
            set { _cardtype = value; }
            get { return _cardtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CardNum
        {
            set { _cardnum = value; }
            get { return _cardnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UerName
        {
            set { _uername = value; }
            get { return _uername; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Honor
        {
            set { _honor = value; }
            get { return _honor; }
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
        public string EMail
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QQ
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeiXin
        {
            set { _weixin = value; }
            get { return _weixin; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ThsScore
        {
            set { _thsscore = value; }
            get { return _thsscore; }
        }
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
        public string VID
        {
            set { _vid = value; }
            get { return _vid; }
        }

        /// <summary>
        /// 社会组织编号
        /// </summary>
        public string SocialNO { get; set; }
        /// <summary>
        /// 审核通过是1,其他状态为审核未通过
        /// </summary>
        public int? State
        {
            set { _state = value; }
            get { return _state; }
        }

        public string Msg { get; set; }

        public string PassWord { get; set; }

        /// <summary>
        /// 是否接受任务
        /// </summary>
        public int? Doing { get; set; }
        public byte[] VolHeadImg { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public int LoginState { get; set; }
        #endregion Model

    }
}
