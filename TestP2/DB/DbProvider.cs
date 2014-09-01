using System;
using System.Data.Entity.Infrastructure;
//using System.Data.Objects;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Objects;

namespace QDT.Core.MSData
{
    public class DbProvider : UnitOfWorkBase<DB>, IDbProvider
    {
        #region Construct and UnitOfWork

        public DbProvider(string nameOrConnectionString)
            : base(new DB(nameOrConnectionString))
        {
            CacheCollection = new Dictionary<Type, object>();
        }

        protected override EntityRepositoryBase<DB, T> GenericRepository<T>()
        {
            return new EntityRepository<T>(DbContext);
        }

        private readonly IDictionary<Type, object> CacheCollection;

        private static readonly IList<Type> CacheTypes=typeof(DB).Assembly.GetTypes().Where(t => t.BaseType != null
                                                                    && !t.IsGenericType
                                                                    && t.BaseType.IsGenericType
                                                                    && t.BaseType.GetGenericTypeDefinition() == typeof(EntityRepository<>)).ToList();

        #endregion

        public IRepository<TEntity> Repository<TEntity>() where TEntity : Entity
        {
            var type = typeof(IRepository<TEntity>);
            if (!CacheCollection.ContainsKey(type))
            {
                var complexRepo = RepositoryA<IRepository<TEntity>>();
                if (complexRepo == null)
                {
                    var tempRepo = GenericRepository<TEntity>();
                    CacheCollection.Add(type, tempRepo);
                }
                else
                {
                    CacheCollection.Add(type, complexRepo);
                }
            }
            return CacheCollection[typeof(IRepository<TEntity>)] as IRepository<TEntity>;
        }

        public void Refresh(Object entity)
        {
            ObjectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, entity);
        }


        private object RepositoryA<TRepository>() where TRepository : IRepository
        {
            var type = typeof(TRepository);

            var repositoryType = CacheTypes.FirstOrDefault(t => t.GetInterfaces().Contains(type));

            return repositoryType == null ? null : Activator.CreateInstance(repositoryType, DbContext);
        }

        #region Interface IDbProvider

        public int ExecuteSql(string sql, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public System.Collections.IEnumerable QuerySql(Type elementType, string sql, params object[] parameters)
        {
            return DbContext.Database.SqlQuery(elementType, sql, parameters);
        }

        public IEnumerable<TEntity> QuerySql<TEntity>(string sql, params object[] parameters)
        {
            return DbContext.Database.SqlQuery<TEntity>(sql, parameters);
        }

        public IQueryable<TEntity> D<TEntity>() where TEntity : Entity
        {
            return DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> NoTrack<TEntity>() where TEntity : Entity
        {
            var entities = ((IObjectContextAdapter)DbContext).ObjectContext.CreateObjectSet<TEntity>();
            entities.MergeOption = System.Data.Entity.Core.Objects.MergeOption.NoTracking;
            return entities;
        }

        #endregion
    }
}
