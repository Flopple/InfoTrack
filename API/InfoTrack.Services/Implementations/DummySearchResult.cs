using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InfoTrack.Domain.Models;
using InfoTrack.Domain.Requests;

namespace InfoTrack.Services.Implementations;

[ExcludeFromCodeCoverage]
public static class DummySearchResult
{
    public static SearchResult GetDummySearchResult(CreateSearchResultRequest request) =>
        new SearchResult(
            request.Keywords,
            request.SearchTarget,
            request.SearchEngine,
            new List<SearchResultPosition>
            {
                new SearchResultPosition(1),
                new SearchResultPosition(10),
                new SearchResultPosition(33),
            },
            DateTime.UtcNow
        );
}