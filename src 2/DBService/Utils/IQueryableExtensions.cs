using Dtos.Common;
using System.Linq;

namespace DBService.Utils
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> GetRecordsByPages<T>(this IQueryable<T> queryable, PaginationDto pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
                .Take(pagination.RecordsPerPage);
        }
    }
}
