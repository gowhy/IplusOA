using Common;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin.Models
{
    public class SelectUserApplySerModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime PubTime { get; set; }
        public DateTime PubTimeEnd { get; set; }

        public string SocialNo { get; set; }
        public string SocialName { get; set; }

        public IPagedList<ShowApplyEntity> ApplySerList { get; set; }

     
    }

    public class ShowApplyEntity
    {
        public UserApplyServiceEntity ApplyEntiy { get; set; }
        public VolunteerEntity UserEntiy { get; set; }
        public IQueryable<VolunteerEntity> Vol { get; set; }
        public SocServiceDetailEntity Detail { get; set; }
        public SerRecordEntity Record { get; set; }
        public IQueryable<SocialOrgEntity> Org { get; set; }
    }
}