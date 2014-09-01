using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityMap
{
   public class RolePermissionMap : EntityTypeConfiguration<RolePermissionEntity>
    {
       public RolePermissionMap()
       {

           // modelBuilder.Entity<RolePermissionEntity>()//
           HasRequired(t => t.Role)//
           .WithMany(t => t.ListRolePermission)//
           .HasForeignKey(d => d.RoleId);//

           ToTable("RolePermission");
       }
    }
}
