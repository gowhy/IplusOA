using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;
namespace BackWebAdmin
{
    public class MicrWebSiteModel
    {
        public MicrWebSite MicrWebSiteEntity { get; set; }
        public BackAdminUser BackAdminUserEntity { get; set; }
        public DepartmentEntity DepartmentEntity { get; set; }
    }
}