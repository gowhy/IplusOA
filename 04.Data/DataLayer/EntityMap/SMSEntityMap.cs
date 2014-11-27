using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityMap
{
    public class SMSEntityMap : EntityTypeConfiguration<SMSEntity>
    {
        public SMSEntityMap()
        {
            ToTable("sms");
        }
    }
}
