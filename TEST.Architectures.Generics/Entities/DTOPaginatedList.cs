using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Architectures.Generics.Entities
{
    public class DTOPaginatedList<T> : List<T> where T : BaseEntity, new()
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public DTOPaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < TotalPages); }
        }

        public static async Task<DTOPaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await Task.Run(() => source.CountAsync()).ConfigureAwait(true);
            var items = count > 2000 ? await Task.Run(() => source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()).ConfigureAwait(true) : await Task.Run(() => source.AsEnumerable().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()).ConfigureAwait(true);
            return new DTOPaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
