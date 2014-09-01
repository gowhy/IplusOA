using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //EFDBTest ef = new EFDBTest();

            //ef.Name = "2013name";
            //ef.time = DateTime.Now;

            //testBFEntities t = new testBFEntities();

            //t.EFDBTest.Add(ef);
            //t.SaveChanges();

           // TestEntity t = new TestEntity();
           // t.Name = DateTime.Now.ToString();
           // t.password = "123456";

            DB_Test db = new DB_Test();
           // db.Add(t);
           ////// db.Database.Connection.Open();
           // db.DB_TestEntity.Add(t);
           // db.SaveChanges();

            ViewBag.Title = "Home Page";


            //testBFEntities t = new testBFEntities();
            //t.EFDBTest.Attach(ef);
            //t.Entry(ef).State = EntityState.Modified;
            //t.SaveChanges();


            TestEntity t2 = new TestEntity();
            t2.Id = 113;
            t2.Name = DateTime.Now.ToString();
            t2.password = "123456";
            db.Update<TestEntity>(t2);

          TestEntity TT=  db.GetById<TestEntity>(14);

            TestEntity t3 = new TestEntity();
            t3.Id = 411;
            t3.Name = DateTime.Now.ToString();
            t3.password = "123456";
            db.Delete(t3, t3.Id );

            db.Dispose();
            //db.DB_TestEntity.Attach(t2);
           // db.Entry(t2).State = EntityState.Modified;
            //db.SaveChanges();

      
            return View();
        }

        public class DB_Test : DbContext
        {
            public DB_Test()
                : base("name=mysqlTest")
            {
            }


            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                // 禁用多对多关系表的级联删除
                //modelBuilder.Entity<TestMap>();
                modelBuilder.Configurations.Add(new TestMap());

            }

            public void Update<T>(T entity) where T : class
            {
                DbSet<T> t = base.Set<T>();
                t.Attach(entity);
                this.Entry(entity).State = EntityState.Modified;
                this.SaveChanges();
                // this.Dispose();
            }

            public void Add<T>(T entity) where T : class
            {


                DbSet<T> t = base.Set<T>();
                t.Add(entity);
                this.SaveChanges();
                // this.Dispose();

            }

            public void Delete<T>(T entity,object id) where T : class
            {
                DbSet<T> t = base.Set<T>();
                if (GetById<T>(id)==null)
                {
                    return;
                } 
                t.Attach(entity);
                this.Entry(entity).State = EntityState.Deleted;
                this.SaveChanges();
                // this.Dispose();
            }

            public T GetById<T>(object id) where T : class
            {
                DbSet<T> t = base.Set<T>();

                return t.Find(id);
              
               
                // this.Dispose();
            }

            protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, System.Collections.Generic.IDictionary<object, object> items)
            {
                int wrongCount = 0;
                int allCount = 0;
                if (entityEntry.State == EntityState.Modified || entityEntry.State == EntityState.Deleted)
                {
                    foreach (var name in entityEntry.OriginalValues.PropertyNames)
                    {
                        var prop = entityEntry.Property(name);
                        if (prop.IsModified)
                        {
                            allCount++;
                            if (prop.OriginalValue.Equals(prop.CurrentValue))
                                wrongCount++;
                        }
                    }
                    if (wrongCount == allCount)
                        entityEntry.State = EntityState.Unchanged;
                }
                return base.ValidateEntity(entityEntry, items);
            }
        }

        public class BaseDBset<T> : DbSet
        {

            //private System.Collections.IList local = new List<T>();
            //public override System.Collections.IList Local
            //{
            //    get
            //    {
            //        return local;
            //    }
            //}
            //public override object Attach(object entity)
            //{
              
            //    local.Add(entity);
            //    return entity;
            //}

            //public override object Add(object entity)
            //{
            //    local.Add(entity);
            //    return entity;
            //}


            //public void Update<T>(T entity) where T : class
            //{
            //    BaseDBset<T> S = new BaseDBset<T>();
            //    S.Attach(entity);
            //    this.Entry(entity).State = EntityState.Modified;
            //    this.SaveChanges();
            //    this.Dispose();
            //}
        }

        public class TestEntity
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string password { get; set; }

        }

        public class TestMap : EntityTypeConfiguration<TestEntity>
        {
            public TestMap()
            {
              

                //this.HasKey(t => t.Id);
                //this.Property(t => t.Id).HasColumnName("ID");
                this.Property(t => t.Name).HasColumnName("name2");
                this.Property(t => t.password).HasColumnName("password2");
                  this.ToTable("table1");
            }



        }

    }
}
