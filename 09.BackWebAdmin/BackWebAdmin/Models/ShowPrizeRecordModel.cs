using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;

namespace BackWebAdmin.Models
{
    public class ShowPrizeRecordModel
    {
        public Prize Prize { get; set; }
        public PrizeRecord PrizeRecord { get; set; }
        public VolunteerEntity VolunteerEntity { get; set; }
    }
}