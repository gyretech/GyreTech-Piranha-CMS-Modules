using System.Collections.Generic;
using System.Linq;

namespace Piranha.AspNetCore.Identity.EF
{
    public class PagedList<T>
    {
        public PagedList() { }

        public PagedList(List<T> items, long currentPage, long itemsPerPage, long totalItems)
        {
            Set(items, currentPage, itemsPerPage, totalItems);
        }

        public long CurrentPage { get; private set; }

        public long TotalPages { get; private set; }

        public long TotalItems { get; private set; }

        public long ItemsPerPage { get; private set; }

        public List<T> Items { get; private set; }

        public object Context { get; private set; }

        public bool HasPreviousPage { get { return this.CurrentPage > 1L; } }

        public bool HasNextPage { get { return this.CurrentPage < this.TotalPages; } }

        public IEnumerator<T> GetEnumerator()
        {
            var items = Items;
            return items == null ? Enumerable.Empty<T>().GetEnumerator() : items.GetEnumerator();
        }

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    return this.GetEnumerator();
        //}

        public void Set(List<T> items, long currentPage, long itemsPerPage, long totalItems)
        {
            Items = items;
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = itemsPerPage == 0 ? 0 : totalItems / itemsPerPage;
            if ((totalItems % itemsPerPage) != 0) TotalPages++;

        }
    }
}