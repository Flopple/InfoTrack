using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InfoTrack.Domain.Requests;
using InfoTrack.Domain.Responses;
using InfoTrack.Persistence.Context;
using InfoTrack.Services.Extensions;
using InfoTrack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InfoTrack.Services.Implementations;

public sealed class SearchService : ISearchService
{
    private readonly DatabaseContext _databaseContext;
    private readonly ISearchStrategyResolver _searchStrategyResolver;
    private readonly ILogger<SearchService> _logger;

    public SearchService(
        DatabaseContext databaseContext,
        ISearchStrategyResolver searchStrategyResolver,
        ILogger<SearchService> logger
    )
    {
        _databaseContext = databaseContext;
        _searchStrategyResolver = searchStrategyResolver;
        _logger = logger;
    }

    public async Task<ApiResponse<SearchResultResponse>> GetSearchResultByIdAsync(int id)
    {
        var searchResult = await _databaseContext.SearchResults
            .Include(x => x.Positions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (searchResult == null)
        {
            _logger.LogInformation("Search result with ID {id} not found", id);
            return new ApiResponse<SearchResultResponse>(null, HttpStatusCode.NotFound, "Search result not found");
        }

        var response = new SearchResultResponse(searchResult);

        return new ApiResponse<SearchResultResponse>(response, HttpStatusCode.OK);
    }

    public async Task<ApiResponse<PaginatedResponse<SearchResultResponse>>> GetSearchResultsAsync(
        int pageNumber,
        int pageSize
    )
    {
        var paginatedSearchResults = await _databaseContext.SearchResults
            .Include(x => x.Positions)
            .OrderByDescending(x => x.SearchDate)
            .ToPaginatedListAsync(pageNumber, pageSize);

        var paginatedResponse = new PaginatedResponse<SearchResultResponse>(
            SearchResultResponse.ToSearchResultResponse(paginatedSearchResults.Items),
            paginatedSearchResults.TotalCount,
            paginatedSearchResults.PageNumber,
            paginatedSearchResults.PageSize
        );

        return new ApiResponse<PaginatedResponse<SearchResultResponse>>(paginatedResponse, HttpStatusCode.OK);
    }

    public async Task<ApiResponse<int>> CreateSearchResultAsync(CreateSearchResultRequest request)
    {
        var existingSearchResult = await _databaseContext.SearchResults
            .FirstOrDefaultAsync(x => x.SearchTarget == request.SearchTarget
                                      && x.Keywords == request.Keywords
                                      && x.SearchEngine == request.SearchEngine
                                      && x.SearchDate.Date == DateTime.UtcNow.Date);
        if (existingSearchResult != null)
        {
            _logger.LogInformation("Search result with ID {id} already exists", existingSearchResult.Id);
            return new ApiResponse<int>(-1, HttpStatusCode.BadRequest, "Search already performed today");
        }

        var searchStrategies = _searchStrategyResolver.GetStrategies();

        var exists = searchStrategies.TryGetValue(request.SearchEngine, out var searchStrategy);
        if (!exists)
        {
            _logger.LogInformation("Search engine not found: {searchEngine}", request.SearchEngine);
            return new ApiResponse<int>(-1, HttpStatusCode.BadRequest, "Search engine not found");
        }

        var searchResult = await searchStrategy(request);

        await _databaseContext.SearchResults.AddAsync(searchResult);
        await _databaseContext.SaveChangesAsync();
        _logger.LogInformation("Search result saved with id: {id}", searchResult.Id);

        return new ApiResponse<int>(searchResult.Id, HttpStatusCode.Created, "Search Result Successfully Created");
    }

    public async Task<ApiResponse<SearchEngineGraphDataResponse>> GetSearchEngineGraphData(int days)
    {
        var startDate = DateTime.UtcNow.AddDays(-days).Date;

        var engineDateGroups = await _databaseContext.SearchResults
            .Include(searchResult => searchResult.Positions)
            .Where(searchResult => searchResult.SearchDate >= startDate)
            .GroupBy(searchResult => new { searchResult.SearchEngine, searchResult.SearchDate.Date })
            .Select(grouping => new
            {
                grouping.Key.SearchEngine,
                SearchDate = grouping.Key.Date,
                SearchResults = grouping.ToList()
            })
            .ToListAsync();

        var searchEnginePlotData = engineDateGroups
            .GroupBy(grouping => grouping.SearchEngine)
            .ToDictionary(
                grouping => grouping.Key,
                grouping => new SearchEnginePlotDataResponse
                {
                    SearchHistory = grouping
                        .Select(dateGroup => new SearchCountDataPointResponse
                        {
                            SearchDate = dateGroup.SearchDate,
                            TotalCount = dateGroup.SearchResults.Sum(searchResult => searchResult.Positions.Count())
                        })
                        .OrderBy(x => x.SearchDate)
                        .ToList(),

                    PositionHistory = grouping
                        .Select(dateGroup => new PositionDataPointResponse
                        {
                            SearchDate = dateGroup.SearchDate,
                            Positions = dateGroup.SearchResults
                                .SelectMany(searchResult => searchResult.Positions.Select(p => p.Position))
                                .OrderBy(position => position)
                                .ToList()
                        })
                        .OrderBy(positionDataPoint => positionDataPoint.SearchDate)
                        .ToList()
                }
            );

        var response = new SearchEngineGraphDataResponse(searchEnginePlotData);

        return new ApiResponse<SearchEngineGraphDataResponse>(response, HttpStatusCode.OK);
    }
}