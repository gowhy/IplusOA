using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IplusOAEntity;

namespace BackWebAdmin.Models
{
    public class SelectVolModel
    {
        public string RealName { get; set; }
        public string Phone { get; set; }
        public string CardNum { get; set; }
        public string Type { get; set; }
        

        public IPagedList<VolunteerEntity> VolList { get; set; }
    }
}