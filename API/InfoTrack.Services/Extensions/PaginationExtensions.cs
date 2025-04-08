using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using InfoTrack.Domain.Responses;
using Microsoft.EntityFrameworkCore;

namespace InfoTrack.Services.Extensions;

[ExcludeFromCodeCoverage]
public static class PaginationExtensions
{
    public static async Task<PaginatedResponse<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize
    )
    {
        var totalCount = await source.CountAsync();

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<T>(
            items,
            totalCount,
            pageNumber,
            pageSize
        );
    }
}