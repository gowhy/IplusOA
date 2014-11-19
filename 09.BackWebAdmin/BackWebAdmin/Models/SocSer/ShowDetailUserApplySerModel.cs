using Common;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin.Models
{
    public class ShowDetailUserApplySerModel
    {

        public SocServiceDetailEntity Detail { get; set; }
        public SocialOrgEntity Org { get; set; }
        public IQueryable<ShowDetailUserApplySerEntity> ApplyEntiyList { get; set; }

        public IQueryable<UserApplyServiceEntity> ApplySerList { get; set; }

     
    }

    public class ShowDetailUserApplySerEntity
    {
        public IQueryable<SerRecordEntity> Record { get; set; }
        public UserApplyServiceEntity ApplyEntiy { get; set; }
        public VolunteerEntity UserEntiy { get; set; }
        public IQueryable<VolunteerEntity> Vol { get; set; }
 
  
    }
}