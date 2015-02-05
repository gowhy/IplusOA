using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;

namespace BackWebAdmin
{
    public class ShowWishModel
    {
        public Wish Wish { get; set; }
        public VolunteerEntity User { get; set; }
    }
}