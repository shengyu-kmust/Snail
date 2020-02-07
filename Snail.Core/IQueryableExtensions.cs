using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Snail.Common
{
    public static class IQueryableExtensions
    {
        public static IPageResult<TSource> ToPageList<TSource>(this IQueryable<TSource> query,IPagination pagination)
        {
            if (pagination.PageSize<=0 || pagination.PageIndex<=0)
            {
                throw new ArgumentException("分页的PageSize或PageIndex要大于1", nameof(pagination));
            }
            int total = query.Count();
            if (total == 0 || total <= pagination.PageSize * (pagination.PageIndex - 1))
            {
                return new PageResult<TSource>
                {
                    Items = new List<TSource>(),
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    Total = total
                };
            }
            else
            {
                return new PageResult<TSource>
                {
                    Items = (pagination.PageIndex == 1 ? query : query.Skip((pagination.PageIndex - 1) * pagination.PageSize)).Take(pagination.PageSize).ToList(),
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    Total = total
                };
            }
        }

        public static IQueryable<TSource> Order<TSource>(this IQueryable<TSource> query, IQueryOrderBy queryOrderBy)
        {
            if (string.IsNullOrEmpty(queryOrderBy.OrderByField))
            {
                return query;
            }
            var prop = typeof(TSource).GetProperties().ToList().FirstOrDefault(a => a.Name.Equals(queryOrderBy.OrderByField, StringComparison.OrdinalIgnoreCase));
            if (prop == null)
            {
                return query;
            }
            var orderMethodName = queryOrderBy.OrderBySort == ESort.Asc ? "OrderBy" : "OrderByDescending";
            var param = Expression.Parameter(typeof(TSource));
            var types = new List<Type> { typeof(TSource), prop.PropertyType };
            Expression expr = Expression.Call(typeof(Queryable), orderMethodName, types.ToArray(), query.Expression, Expression.Lambda(Expression.Property(param, prop.Name), param));
            query = query.AsQueryable().Provider.CreateQuery<TSource>(expr);
            return query;
        }
    }

    public interface IQueryOrderBy
    {
        string OrderByField { get; set; }
        ESort? OrderBySort { set; get; }
    }
    public enum ESort
    {
        Asc,
        Des
    }
}
