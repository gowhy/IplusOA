using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityMap
{
    public class DepartmentEntityToDepartmentMap : EntityTypeConfiguration<DepartmentEntity>
    {
      public DepartmentEntityToDepartmentMap()
      {
          ToTable("Department");
      }
    }
}
