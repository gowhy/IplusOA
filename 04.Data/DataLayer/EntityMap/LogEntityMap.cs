using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityMap
{
    public class LogEntityMap : EntityTypeConfiguration<LogEntity>
    {
        public LogEntityMap()
        {
            ToTable("log");
        }
    }
}
