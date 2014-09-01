using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RoleEntityToRoleMap : EntityTypeConfiguration<RoleEntity>
    {
        public RoleEntityToRoleMap()
        {
            //HasKey(x => x.Id);
           
            ToTable("role");
        }
    }
}
