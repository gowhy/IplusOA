using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IplusOAEntity
{
  public  class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsEffect { get; set; }

      //  public bool IsDelete { get; set; }

        public virtual ICollection<RolePermissionEntity> ListRolePermission { get; set; }
    }
}
