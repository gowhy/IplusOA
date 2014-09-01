using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IplusOAEntity;
namespace DataLayer.EntityMap
{
    public class SocialOrgEntityToSocialOrgMap : EntityTypeConfiguration<SocialOrgEntity>
    {
        public SocialOrgEntityToSocialOrgMap()
        {
            //HasRequired(t => t.Department)//
            //.WithMany(t => t.ListSocialOrgEntity)//
            //.HasForeignKey(d => d.DepId);//
            ToTable("SocialOrg");
        }
    }
}
