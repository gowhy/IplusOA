using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
//using System.Data.Objects;
using System.Linq.Expressions;
//using QDT.Common.Events;
using System.Data.Entity.Core.Objects;

namespace QDT.Core.MSData
{
    public abstract class UnitOfWorkBase<TDbContext> : UnitOfWorkForEvent
        where TDbContext : DbContext
    {
        private readonly TDbContext dbContext;

        protected abstract EntityRepositoryBase<TDbContext, T> GenericRepository<T>() where T : class;

        protected UnitOfWorkBase(TDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        public ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)dbContext).ObjectContext; }
        }

        public TDbContext DbContext
        {
            get { return dbContext; }
        }

        public override T GetByID<T>(object id)
        {
            return GenericRepository<T>().Find(id);
        }

        public override T Add<T>(T t)
        {
            return GenericRepository<T>().Create(t);
        }

        public override void Update<T>(T t)
        {
            GenericRepository<T>().Update(t);
        }

        public override void Delete<T>(T t)
        {
            GenericRepository<T>().Delete(t);
        }

        public override void Delete<T,TKey>(TKey id){
            GenericRepository<T>().Delete(o => o.Id.Equals(id));
        }

        public override int Delete<T>(Expression<Func<T, bool>> predicate)
        {
            return GenericRepository<T>().Delete(predicate);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if(dbContext!=null) dbContext.Dispose();
                    UncommittedEvents.Clear();
                }

                _disposed = true;
            }
        }

        protected override int DoCommit()
        {
            return dbContext.SaveChanges();
        }
    }
}
