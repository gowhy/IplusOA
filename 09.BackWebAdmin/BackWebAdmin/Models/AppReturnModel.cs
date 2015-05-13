using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin
{
    public class AppReturnModel
    {
        public int State { get; set; }
        public string Msg { get; set; }

        public string StateCode { get; set; }


    }
    public class AppReturnModel2
    {
        public int state { get; set; }
        public string msg { get; set; }

        public string stateCode { get; set; }


    }

    public class AppReturnSignInModel
    {
        public int State { get; set; }
        public string Msg { get; set; }

        public int Score { get; set; }
        /// <summary>
        /// 持续签到次数
        /// </summary>
        public int LastSignInTime { get; set; }

    }
}