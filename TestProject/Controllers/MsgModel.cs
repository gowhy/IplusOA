using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QDT.Web.Admin.Models
{
    public class MsgModel
    {   
        public int WaitSecond { get; set; }

        public string JumpUrl { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// true为成功，false为失败
        /// </summary>
        public bool Type { get; set; }
    }
}
