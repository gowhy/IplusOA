using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackWebAdmin
{
    public class AppStartModel
    {
        public StartAdImgEntity StartAdImg { get; set; }
        public AppVerEntity AppVer { get; set; }

        public List<ServiceAdImgEntity> ListServiceAdImg { get; set; }
    }
}