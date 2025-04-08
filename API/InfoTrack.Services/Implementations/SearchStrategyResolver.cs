using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrack.Domain.Enums;
using InfoTrack.Domain.Models;
using InfoTrack.Domain.Requests;
using InfoTrack.Services.Implementations.SearchStrategies;
using InfoTrack.Services.Interfaces;

namespace InfoTrack.Services.Implementations;

public class SearchStrategyResolver : ISearchStrategyResolver
{
    private readonly Dictionary<SearchEngine, Func<CreateSearchResultRequest, Task<SearchResult>>> _strategies;

    public SearchStrategyResolver(
        GoogleSearchStrategy googleSearchStrategy,
        YahooSearchStrategy yahooSearchStrategy)
    {
        _strategies = new Dictionary<SearchEngine, Func<CreateSearchResultRequest, Task<SearchResult>>>
        {
            { SearchEngine.Google, googleSearchStrategy.PerformSearchAsync },
            { SearchEngine.Yahoo, yahooSearchStrategy.PerformSearchAsync }
        };
    }

    public Dictionary<SearchEngine, Func<CreateSearchResultRequest, Task<SearchResult>>> GetStrategies()
        => _strategies;
}