using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hospital_FinalP.Controllers
{
    public class Helper
    {
        public static async Task<(IEnumerable<T> items, int totalCount)> PaginateAsync<T>(
        IQueryable<T> query,
        int? page,
        int? perPage)
        {
            int totalCount = await query.CountAsync();

            if (!page.HasValue || !perPage.HasValue)
            {
                return (await query.ToListAsync(), totalCount);
            }

            int currentPage = page.Value > 0 ? page.Value : 1;
            int itemsPerPage = perPage.Value > 0 ? perPage.Value : 10;

            int totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
            currentPage = currentPage > totalPages ? totalPages : currentPage;

            int skip = Math.Max((currentPage - 1) * itemsPerPage, 0);

            IEnumerable<T> items = await query.Skip(skip).Take(itemsPerPage).ToListAsync();

            return (items, totalCount);
        }
    }
}
