using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InfoTrack.Domain.Enums;

namespace InfoTrack.Domain.Models;

[ExcludeFromCodeCoverage]
public class SearchResult
{
    public int Id { get; init; }
    public string Keywords { get; init; }
    public string SearchTarget { get; init; }
    public SearchEngine SearchEngine { get; init; }
    public IEnumerable<SearchResultPosition> Positions { get; init; }
    public DateTime SearchDate { get; init; }

    public SearchResult()
    {
    }
    
    public SearchResult(
        string keywords,
        string searchTarget,
        SearchEngine searchEngine,
        IEnumerable<SearchResultPosition> positions,
        DateTime searchDate
    )
    {
        Keywords = keywords;
        SearchTarget = searchTarget;
        SearchEngine = searchEngine;
        Positions = positions;
        SearchDate = searchDate;
    }

    public SearchResult(
        int id,
        string keywords,
        string searchTarget,
        SearchEngine searchEngine,
        IEnumerable<SearchResultPosition> positions,
        DateTime searchDate
    )
    {
        Id = id;
        Keywords = keywords;
        SearchTarget = searchTarget;
        SearchEngine = searchEngine;
        Positions = positions;
        SearchDate = searchDate;
    }
}