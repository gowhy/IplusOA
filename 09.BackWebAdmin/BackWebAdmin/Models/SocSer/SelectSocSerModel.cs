﻿using Common;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin.Models
{
    public class SelectSocSerModel   
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime PubTime { get; set; }
        public DateTime PubTimeEnd { get; set; }

        public string SocialNo { get; set; }
        public string SocialName { get; set; }

        public int VId { get; set; }

        public int State { get; set; }

        public string AddUser { get; set; }
        public IPagedList<SocServiceDetailEntityClone> SocSerList { get; set; }

    }
}