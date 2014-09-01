using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BackWebAdmin.Models
{
    public class RoleModel
    {
        public RoleModel()
        {
            IsEffect = true;
            Modules = new List<RoleModuleModel>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsEffect { get; set; }

        public IList<RoleModuleModel> Modules { get; set; }
    }

    public class RoleModuleModel
    {
        public RoleModuleModel()
        {
            RoleNodes = new List<RoleNodeModel>();
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsSelect { get; set; }

        public IList<RoleNodeModel> RoleNodes { get; set; }
    }

    public class RoleNodeModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsDisable { get; set; }

        public bool IsSelect { get; set; }
    }

    public class RoleEditRole
    {
        public int ID { get; set; }
        
        [Required]
        public string Name { get; set; }

        public bool IsEffect { get; set; }

        public string[] RoleModule { get; set; }

        public string[] RoleNode { get; set; }
    }
}