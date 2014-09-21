using Common;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin.Models
{
    public class SelectSorgModel
    {
        public string Type { get; set; }

        public string SocialNO { get; set; }
        public string Name { get; set; }
        public string RegNO { get; set; }

        public IPagedList<SocialOrgEntity> SocOrgList { get; set; }
    }
}