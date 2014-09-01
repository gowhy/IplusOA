
using DataLayer.EntityMap;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.IplusOADB
{
    public partial class IplusOADBContext : BaseContext
    {
        public IplusOADBContext()
            : base("Conniplusoa_db")
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 禁用多对多关系表的级联删除
            //modelBuilder.Entity<TestMap>();
            modelBuilder.Configurations.Add(new BackAdminUserToAdminUserMap());
            modelBuilder.Configurations.Add(new RolePermissionMap());
            modelBuilder.Configurations.Add(new RoleEntityToRoleMap());
            modelBuilder.Configurations.Add(new DepartmentEntityToDepartmentMap());
            modelBuilder.Configurations.Add(new SocialOrgEntityToSocialOrgMap());
            modelBuilder.Configurations.Add(new VolunteerEntityToVolunteerMap());
            modelBuilder.Configurations.Add(new SocServiceDetailEntityToSocServiceDetailMap());
            modelBuilder.Configurations.Add(new MenuEntityToMenuMap());
            
        }


        /// <summary>
        /// 后台用户管理表
        /// </summary>
        public virtual DbSet<BackAdminUser> BackAdminUserEntityDBSet { get; set; }

        /// <summary>
        /// 角色权限控制表
        /// </summary>
        public virtual DbSet<RolePermissionEntity> RolePermission { get; set; }
        public virtual DbSet<RoleEntity> RoleTable { get; set; }
        public virtual DbSet<DepartmentEntity> DepartmentTable { get; set; }
        public virtual DbSet<SocialOrgEntity> SocialOrgEntityTable { get; set; }

        public virtual DbSet<SocServiceDetailEntity> SocServiceDetailEntityTable { get; set; }
        public virtual DbSet<VolunteerEntity> VolunteerEntityTable { get; set; }
        public virtual DbSet<MenuEntity> MenuEntityTable { get; set; }
       
        
    }
}
