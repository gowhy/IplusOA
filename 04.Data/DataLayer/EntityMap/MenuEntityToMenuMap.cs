using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IplusOAEntity;

namespace DataLayer
{
    public class MenuEntityToMenuMap : EntityTypeConfiguration<MenuEntity>
    {
        public MenuEntityToMenuMap()
        {
            ToTable("menu");
        }
    }
}
