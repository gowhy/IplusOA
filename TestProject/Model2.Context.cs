﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestProject
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class iplusoa_dbEntities : DbContext
    {
        public iplusoa_dbEntities()
            : base("name=iplusoa_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<adminuser> adminuser { get; set; }
        public virtual DbSet<department> department { get; set; }
        public virtual DbSet<menu> menu { get; set; }
        public virtual DbSet<role> role { get; set; }
        public virtual DbSet<rolepermission> rolepermission { get; set; }
        public virtual DbSet<socialorg> socialorg { get; set; }
        public virtual DbSet<socserdetailjoin> socserdetailjoin { get; set; }
        public virtual DbSet<socservicedetail> socservicedetail { get; set; }
        public virtual DbSet<volunteer> volunteer { get; set; }
        public virtual DbSet<serrecord> serrecord { get; set; }
    }
}
