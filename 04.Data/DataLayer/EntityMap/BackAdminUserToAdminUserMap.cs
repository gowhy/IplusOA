using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityMap
{
    public class BackAdminUserToAdminUserMap : EntityTypeConfiguration<BackAdminUser>
    {
        public BackAdminUserToAdminUserMap()
       {
           HasKey(x => x.Id);
           //this.Property(t => t.UserName).HasColumnName("name2");
           //this.Property(t => t.password).HasColumnName("password2");
           this.Ignore(t => t.LoginToken);
           this.Ignore(t => t.Msg);
           this.ToTable("adminuser");
       }
    }
}
