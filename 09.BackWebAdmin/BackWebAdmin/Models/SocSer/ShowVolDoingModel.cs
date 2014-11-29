using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;

namespace BackWebAdmin.Models
{
    public class ShowVolDoingModel
    {
        public VolunteerEntity Vol { get; set; }
        public VolunteerEntity User    { get; set; }
        public SerRecordEntity Record { get; set; }
        public SocServiceDetailEntity SocServiceDetail { get; set; }
    }
}