using Common;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin.Models
{
    public class SelectSocSerModel   
    {
        public string type { get; set; }
        public IPagedList<SocServiceDetailEntity> SocSerList { get; set; }

    }
}