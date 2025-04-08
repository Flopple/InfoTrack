using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace InfoTrack.Domain.Responses;

[ExcludeFromCodeCoverage]
public class PaginatedResponse<T>
{
    public IEnumerable<T> Items { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    
    public PaginatedResponse(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}