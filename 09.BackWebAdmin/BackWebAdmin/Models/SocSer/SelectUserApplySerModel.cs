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

        public IEnumerable<VolunteerEntity> VolList { get; set; }
        public IEnumerable<SerRecordEntity> RecordList { get; set; }
        public IQueryable<SocialOrgEntity> Org { get; set; }

        public VolunteerEntity VolModel { get; set; }

        public IList<ShowApplyEntity2> RecVol { get; set; }

    }

    public class ShowApplyEntity2
    {
     
        public SerRecordEntity Record { get; set; }

        public VolunteerEntity VolModel { get; set; }
    }
}