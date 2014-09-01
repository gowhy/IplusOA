using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;


namespace QDT.Core.MSData
{

    public class EntityRepository<T> : EntityRepositoryBase<DB, T>
        where T : class 
    {
        public EntityRepository(DB dbContext)
            : base(dbContext, dbContext.Set<T>())
        {
            IsOwnContext = false;
        }
    }

    public abstract class EntityRepositoryBase<TContext, TObject> : IRepository<TObject>
        where TContext : DbContext
        where TObject : class
    {
        private readonly TContext context;
        private readonly DbSet<TObject> dbSet;

        protected TContext Context { get { return context; } }
        protected DbSet<TObject> DbSet { get { return dbSet; } }

        protected bool IsOwnContext;

        protected EntityRepositoryBase(TContext dbContext, DbSet<TObject> dbSet)
        {
            this.context = dbContext;
            this.dbSet = dbSet;
        }

        public void Dispose()
        {
            if ((IsOwnContext) && (Context != null))
            {
                Context.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public virtual IQueryable<TObject> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TObject> All(out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resultSet = skipCount == 0 ? DbSet.Take(size) : DbSet.Skip(skipCount).Take(size);
            total = _resultSet.Count();
            return null;
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        public virtual IQueryable<TObject> Filter<Key>(Expression<Func<TObject, Key>> sortingSelector, Expression<Func<TObject, bool>> filter, out int total, SortingOrders sortby = SortingOrders.Asc, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resultSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            total = _resultSet.Count();
            _resultSet = sortby == SortingOrders.Asc ? _resultSet.OrderBy(sortingSelector).AsQueryable() : _resultSet.OrderByDescending(sortingSelector).AsQueryable();
          //  _resultSet = skipCount == 0 ? _resultSet.Take(size) : _resultSet.Skip(skipCount).Take(size);
            return _resultSet;
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual int Count()
        {
            return DbSet.Count();
        }

        public virtual int Count(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate);
        }


        public TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual TObject Create(TObject t)
        {
            DbSet.Add(t);
            return t;
        }

        public virtual void Delete(TObject t)
        {
            //var entry = Context.Entry(t);
            DbSet.Remove(t);
            if (IsOwnContext)
                Context.SaveChanges();
        }

        public virtual int Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = DbSet.Where(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);

            if (IsOwnContext)
                return Context.SaveChanges();

            return objects.Count();
        }

        public virtual TObject Update(TObject t)
        {
            //var entry = Context.Entry(t);
            //DbSet.Attach(t);
            //entry.State = EntityState.Modified;
            //if (IsOwnContext)
            //    Context.SaveChanges();
            return t;
        }

        public virtual int Submit()
        {
            return Context.SaveChanges();
        }


    }
}
