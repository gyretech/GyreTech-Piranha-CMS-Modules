using Piranha.AspNetCore.Identity.EF.Code;
using System;
using System.Linq;

namespace Piranha.AspNetCore.Identity.EF.Extensions
{
    public static class PagedListExtenstions
    {
        public static PagedList<TOutput> ConvertToModel<T, TOutput>(this PagedList<T> page, Converter<T, TOutput> converter)
        {
            if (page == null) return null;

            return new PagedList<TOutput>(page.Items.ConvertAll(converter), page.CurrentPage, page.ItemsPerPage, page.TotalItems);
        }

        public static PagedList<T> ToPage<T>(this IOrderedQueryable<T> orderedQuery, int page, int pageSize)
        {
            var pagedQuery = orderedQuery as IQueryable<T>;

            if (page <= 0) page = 1;

            if (page > 1) pagedQuery = pagedQuery.Skip((page - 1) * pageSize);

            pagedQuery = pagedQuery.Take(pageSize);

            return new PagedList<T>(pagedQuery.ToList(), page, pageSize, orderedQuery.LongCount());
        }
    }
}