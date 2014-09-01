using System;
using System.Linq;
using System.Linq.Expressions;


namespace QDT.Core.MSData
{
    public enum SortingOrders
    {
        Asc,
        Desc
    }

    public static class OrderByHelper
    {

        //public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending) where T : class
        //{
        //    Type type = typeof(T);

        //    PropertyInfo property = type.GetProperty(propertyName??"ID");
        //    if (property == null)
        //        return source;

        //    ParameterExpression param = Expression.Parameter(type, "p");
        //    Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
        //    LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

        //    string methodName = ascending ? "OrderBy" : "OrderByDescending";

        //    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

        //    return source.Provider.CreateQuery<T>(resultExp);
        //}

        
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending) where T : class {
            //改用microsoft的动态linq库，更安全准确
            var sort = "{0} {1}".FormatWith(propertyName ?? "ID", ascending ? "asc" : "desc");
            return source.OrderBy(sort);
        }

        /// <summary>
        /// 获取对应的字段名
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static string GetMemberName<TSource,TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            string fieldName = null;
            var exp = keySelector.Body as UnaryExpression;
            if (exp == null)
            {
                var body = keySelector.Body as MemberExpression;
                if (body != null) fieldName = body.Member.Name;
            }
            else
            {
                var memberExpression = exp.Operand as MemberExpression;
                if (memberExpression != null) fieldName = memberExpression.Member.Name;
            }
            return fieldName;
        }
    }
}
