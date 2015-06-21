using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;
namespace BackWebAdmin
{
    public class SunGovModel
    {
        public SunGovs SunGovEntity { get; set; }
        public BackAdminUser BackAdminUserEntity { get; set; }
        public DepartmentEntity DepartmentEntity { get; set; }
    }
}