using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;

namespace BackWebAdmin.Models
{
    public class RecVol
    {
        public SerRecordEntity RecordEntity { get; set; }
        public  VolunteerEntity Vol { get; set; }
     
    }
    public class ShowDetailUserApply
    {
        public SocServiceDetailEntity SocSerDetail { get; set; }
        public SocialOrgEntity SOrg { get; set; }
        public IList<UserApplyServiceEntity> UserApplyList { get; set; }
        
    }

    public class ModelShowDetailUserApplyList
    {
        public SocServiceDetailEntity SocSerDetail { get; set; }

        public SocialOrgEntity SOrg { get; set; }
        public IList<ApplyUserRecVol> UserRecApplyList { get; set; }


    }

    public class ApplyUserRecVol
    {
        public IList<RecVol> SerVolList { get; set; }
        public VolunteerEntity ApplyUserInfo { get; set; }
        public UserApplyServiceEntity UserApplyService { get; set; }
    }

}