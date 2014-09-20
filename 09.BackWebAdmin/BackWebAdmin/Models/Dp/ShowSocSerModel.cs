using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin.Models
{
    public class ShowSocSerModel
    {
        private string pubTimeString;
        public string PubTimeString
        {

            get { return PubTime.Value.ToString("yyyy-MM-dd HH"); }
            set { pubTimeString = value; }
        }
        private string endTimeString;
        public string EndTimeString
        {
            get { return EndTime.Value.ToString("yyyy-MM-dd HH"); }
           
             set { endTimeString = value; }
        }

        public DateTime? PubTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Context { get; set; }
    }
}