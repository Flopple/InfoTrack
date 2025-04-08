using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using InfoTrack.Domain.Enums;
using InfoTrack.Domain.Models;

namespace InfoTrack.Domain.Responses;

[ExcludeFromCodeCoverage]
public record SearchResultResponse
{
    public int Id { get; init; }
    public string Keywords { get; init; }
    public string SearchTarget { get; init; }
    public SearchEngine SearchEngine { get; init; }
    public IEnumerable<int> Positions { get; init; }
    public int TotalResults { get; init; }
    public DateTime SearchDate { get; init; }

    public SearchResultResponse(SearchResult searchResult)
    {
        Id = searchResult.Id;
        Keywords = searchResult.Keywords;
        SearchEngine = searchResult.SearchEngine;
        SearchTarget = searchResult.SearchTarget;
        Positions = searchResult.Positions.Select(x => x.Position);
        TotalResults = searchResult.Positions.Count();
        SearchDate = searchResult.SearchDate;
    }

    public static IEnumerable<SearchResultResponse> ToSearchResultResponse(IEnumerable<SearchResult> searchResults) =>
        searchResults.Select(x => new SearchResultResponse(x));
}