using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IplusOAEntity;

namespace DataLayer.EntityMap
{
   public class SerRecordMap : EntityTypeConfiguration<SerRecordEntity>
    {
       public SerRecordMap()
       {
           ToTable("serrecord");
       }
    }
}
