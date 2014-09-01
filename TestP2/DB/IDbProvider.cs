using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using QDT.Common.Data;

namespace QDT.Core.MSData
{
    public interface IDbProvider : IUnitOfWork
    {
        int ExecuteSql(string sql, params object[] parameters);

        IEnumerable QuerySql(Type elementType, string sql, params object[] parameters);

        IEnumerable<TEntity> QuerySql<TEntity>(string sql, params object[] parameters) ;

        IQueryable<TEntity> D<TEntity>() where TEntity:Entity;

        IQueryable<TEntity> NoTrack<TEntity>() where TEntity : Entity;

        IRepository<TEntity> Repository<TEntity>() where TEntity : Entity;

        void Refresh(Object entity);
    }
}