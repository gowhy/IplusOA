using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IplusOAEntity
{
  
    public class MenuEntity
    {


        // public  List<MenuEntity>  ChildMenu { get; set; }
        #region Model
        private int _id;
        private int? _pid;
        private string _name;
        private string _file;
        private bool _open = true;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? pId
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string file
        {
            set { _file = value; }
            get { return _file; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool open
        {
            set { _open = value; }
            get { return _open; }
        }

        public string AText { get; set; }

        public string Module { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<MenuEntity> ChildList;

        #endregion Model

    }
}
