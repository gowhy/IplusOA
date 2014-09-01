using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
  public  class BaseContext : DbContext
    {
        public BaseContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

    

        public void Update<T>(T entity) where T : class
        {
            DbSet<T> t = base.Set<T>();
            t.Attach(entity);
            this.Entry(entity).State = EntityState.Modified;
            this.SaveChanges();
        }

        public void Add<T>(T entity) where T : class
        {
            DbSet<T> t = base.Set<T>();
            t.Add(entity);
            this.SaveChanges();
        }


        public void Delete<T>(T entity) where T : class
        {
            if (entity==null)
            {
                return;
            }
            DbSet<T> t = base.Set<T>();
            t.Attach(entity);
            this.Entry(entity).State = EntityState.Deleted;
            this.SaveChanges();
        }

        public void Delete<T>(T entity, object id) where T : class
        {
            DbSet<T> t = base.Set<T>();
            if (GetById<T>(id) == null)
            {
                return;
            }
            t.Attach(entity);
            this.Entry(entity).State = EntityState.Deleted;
            this.SaveChanges();
        }

        public T GetById<T>(object id) where T : class
        {
            DbSet<T> t = base.Set<T>();

            return t.Find(id);
        }



        //protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, System.Collections.Generic.IDictionary<object, object> items)
        //{
        //    int wrongCount = 0;
        //    int allCount = 0;
        //    if (entityEntry.State == EntityState.Modified)
        //    {
        //        foreach (var name in entityEntry.OriginalValues.PropertyNames)
        //        {
        //            var prop = entityEntry.Property(name);
        //            if (prop.IsModified)
        //            {
        //                allCount++;
        //                if (prop.OriginalValue.Equals(prop.CurrentValue))
        //                    wrongCount++;
        //            }
        //        }
        //        if (wrongCount == allCount)
        //            entityEntry.State = EntityState.Unchanged;
        //    }
        //    return base.ValidateEntity(entityEntry, items);
        //}
    }
}
