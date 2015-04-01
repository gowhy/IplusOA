
using DataLayer.EntityMap;
using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            modelBuilder.Configurations.Add(new SocSerDetailJoinEntityToSocSerDetailJoinMap());
            modelBuilder.Configurations.Add(new SerRecordMap());
            modelBuilder.Configurations.Add(new SocSerImgMap());
            modelBuilder.Configurations.Add(new StartAdImgMap());
            modelBuilder.Configurations.Add(new NoticeMap());
            modelBuilder.Configurations.Add(new WorkGuideEntityMap());
            modelBuilder.Configurations.Add(new SuggestionEntityMap());
            modelBuilder.Configurations.Add(new SuperviseEntityMap());
            modelBuilder.Configurations.Add(new ServiceAdImgMap());
            modelBuilder.Configurations.Add(new AppVerMap());
            modelBuilder.Configurations.Add(new UserApplyServiceMap());
            modelBuilder.Configurations.Add(new LogEntityMap());
            modelBuilder.Configurations.Add(new SMSEntityMap());
            modelBuilder.Configurations.Add(new SystemMsgEntityMap());

            modelBuilder.Configurations.Add(new MicroShopEntityMap());
            modelBuilder.Configurations.Add(new MicroShopCallRecordEntityMap());
            // modelBuilder.Entity<VolunteerEntity>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


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
        public virtual DbSet<SocSerDetailJoinEntity> SocSerDetailJoinEntityTable { get; set; }

        public virtual DbSet<SerRecordEntity> SerRecordTable { get; set; }

        public virtual DbSet<SocSerImgEntity> SocSerImgTable { get; set; }
        public virtual DbSet<StartAdImgEntity> StartAdImgTable { get; set; }

        public virtual DbSet<NoticeEntity> NoticeTable { get; set; }

        public virtual DbSet<WorkGuideEntity> WorkGuideTable { get; set; }

        public virtual DbSet<SuggestionEntity> SuggestionTable { get; set; }

        public virtual DbSet<SuperviseEntity> SuperviseTable { get; set; }

        public virtual DbSet<ServiceAdImgEntity> ServiceAdImgTable { get; set; }

        public virtual DbSet<AppVerEntity> AppVerTable { get; set; }
        public virtual DbSet<UserApplyServiceEntity> UserApplyServiceTable { get; set; }

        public virtual DbSet<LogEntity> LogTable { get; set; }
        public virtual DbSet<SMSEntity> SMSTable { get; set; }
        public virtual DbSet<SystemMsgEntity> SystemMsgTable { get; set; }
        public virtual DbSet<MicroShopCallRecordEntity> MicroShopCallRecordEntityTable { get; set; }
        public virtual DbSet<MicroShopEntity> MicroShopEntityTable { get; set; }

        /// <summary>
        /// 消息推送
        /// </summary>
        public virtual DbSet<AppCodeMsgSend> AppMsgSendTable { get; set; }

        public virtual DbSet<Prize> PrizeTable { get; set; }
        public virtual DbSet<PrizeRecord> PrizeRecordTable { get; set; }
        public virtual DbSet<SocServiceAuditeLog> SocServiceAuditeLogTable { get; set; }

        public virtual DbSet<ChangeGoods> ChangeGoodsTable { get; set; }
        public virtual DbSet<ChangeGoodsLog> ChangeGoodsLogTable { get; set; }

        public virtual DbSet<Wish> WishTable { get; set; }

        public virtual DbSet<SignIn> SignInTable { get; set; }

        public virtual DbSet<ScoreMall> ScoreMallTable { get; set; }
        public virtual DbSet<ScoreMallChangeLog> ScoreMallChangeLogTable { get; set; }
        /// <summary>
        /// 刮刮卡
        /// </summary>
        public virtual DbSet<ScratchCard> ScratchCardTable { get; set; }
        /// <summary>
        /// 社会服务评论
        /// </summary>
        public virtual DbSet<SocServiceDetailComment> SocServiceDetailCommentTable { get; set; }

        //点赞
        public virtual DbSet<SocServiceDetailGood> SocServiceDetailGoodTable { get; set; }

        //分享
        public virtual DbSet<SocServiceDetailShare> SocServiceDetailShareTable { get; set; }

        public virtual DbSet<GridMember> GridMemberTable { get; set; }
        public virtual DbSet<VillageCompany> VillageCompanyTable { get; set; }
        

    }
}
