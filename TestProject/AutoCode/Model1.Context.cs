﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestProject.AutoCode
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class oa2Entities : DbContext
    {
        public oa2Entities()
            : base("name=oa2Entities")
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
        public virtual DbSet<sertype> sertype { get; set; }
        public virtual DbSet<socialorg> socialorg { get; set; }
        public virtual DbSet<socserdetailjoin> socserdetailjoin { get; set; }
        public virtual DbSet<socserimg> socserimg { get; set; }
        public virtual DbSet<socservicedetail> socservicedetail { get; set; }
        public virtual DbSet<volunteer> volunteer { get; set; }
        public virtual DbSet<serrecord> serrecord { get; set; }
        public virtual DbSet<startadimg> startadimg { get; set; }
        public virtual DbSet<notice> notice { get; set; }
        public virtual DbSet<suggestion> suggestion { get; set; }
        public virtual DbSet<workguide> workguide { get; set; }
        public virtual DbSet<userapplyservice> userapplyservice { get; set; }
    }
}
