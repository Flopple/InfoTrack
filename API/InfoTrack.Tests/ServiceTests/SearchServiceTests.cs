using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using InfoTrack.Domain.Enums;
using InfoTrack.Domain.Models;
using InfoTrack.Domain.Requests;
using InfoTrack.Persistence.Context;
using InfoTrack.Services.Implementations;
using InfoTrack.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace InfoTrack.Tests.ServiceTests;

public class SearchServiceTests
{
    private static DatabaseContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DatabaseContext(options);
    }

    private static SearchResult CreateSampleSearchResult(int id = 1)
    {
        return new SearchResult
        {
            Id = id,
            Keywords = "land registry search",
            SearchTarget = "www.infotrack.co.uk",
            SearchEngine = SearchEngine.Google,
            SearchDate = DateTime.UtcNow,
            Positions = new List<SearchResultPosition>
            {
                new SearchResultPosition(id, 1),
                new SearchResultPosition(id, 3)
            }
        };
    }

    [Fact]
    public async Task GetSearchResultByIdAsync_GivenSearchResultExists_ShouldReturnData()
    {
        var databaseContext = CreateInMemoryDbContext();
        var searchResult = CreateSampleSearchResult();
        databaseContext.SearchResults.Add(searchResult);
        await databaseContext.SaveChangesAsync();

        var strategyResolver = Substitute.For<ISearchStrategyResolver>();
        var logger = Substitute.For<ILogger<SearchService>>();
        var service = new SearchService(databaseContext, strategyResolver, logger);

        var response = await service.GetSearchResultByIdAsync(searchResult.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(searchResult.Id, response.Data.Id);
        Assert.Equal(2, response.Data.Positions.Count());
    }

    [Fact]
    public async Task GetSearchResultByIdAsync_GivenSearchResultDoesNotExist_ShouldReturnNoData()
    {
        var databaseContext = CreateInMemoryDbContext();
        var strategyResolver = Substitute.For<ISearchStrategyResolver>();
        var logger = Substitute.For<ILogger<SearchService>>();
        var service = new SearchService(databaseContext, strategyResolver, logger);

        var response = await service.GetSearchResultByIdAsync(999);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Null(response.Data);
    }

    [Fact]
    public async Task GetSearchResultsAsync_GivenSearchResultExists_ShouldReturnPaginatedData()
    {
        var databaseContext = CreateInMemoryDbContext();
        for (var i = 0; i < 10; i++)
        {
            databaseContext.SearchResults.Add(CreateSampleSearchResult(i + 1));
        }

        await databaseContext.SaveChangesAsync();

        var resolver = Substitute.For<ISearchStrategyResolver>();
        var logger = Substitute.For<ILogger<SearchService>>();
        var service = new SearchService(databaseContext, resolver, logger);

        var response = await service.GetSearchResultsAsync(1, 5);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(5, response.Data.Items.Count());
        Assert.Equal(10, response.Data.TotalCount);
    }

    [Fact]
    public async Task CreateSearchResultAsync_GivenValidRequest_ShouldSaveToDatabase()
    {
        var databaseContext = CreateInMemoryDbContext();

        var expectedSearchResult = CreateSampleSearchResult(123);

        var resolver = Substitute.For<ISearchStrategyResolver>();
        var logger = Substitute.For<ILogger<SearchService>>();
        resolver.GetStrategies().Returns(
            new Dictionary<SearchEngine, Func<CreateSearchResultRequest, Task<SearchResult>>>
            {
                {
                    SearchEngine.Google, _ => Task.FromResult(expectedSearchResult)
                }
            });

        var service = new SearchService(databaseContext, resolver, logger);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchTarget = "www.infotrack.co.uk",
            SearchEngine = SearchEngine.Google
        };

        var response = await service.CreateSearchResultAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expectedSearchResult.Id, response.Data);

        var savedResult = await databaseContext.SearchResults.FirstOrDefaultAsync(x => x.Id == expectedSearchResult.Id);
        Assert.NotNull(savedResult);
    }

    [Fact]
    public async Task CreateSearchResultAsync_GivenNoSearchEngine_ShouldReturnBadRequestResponse()
    {
        var databaseContext = CreateInMemoryDbContext();

        var resolver = Substitute.For<ISearchStrategyResolver>();
        var logger = Substitute.For<ILogger<SearchService>>();
        resolver.GetStrategies()
            .Returns(new Dictionary<SearchEngine, Func<CreateSearchResultRequest, Task<SearchResult>>>());

        var service = new SearchService(databaseContext, resolver, logger);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchTarget = "www.infotrack.co.uk"
        };

        var response = await service.CreateSearchResultAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateSearchResultAsync_GivenDuplicateSearchToday_ShouldReturnBadRequest()
    {
        var databaseContext = CreateInMemoryDbContext();
        var existingResult = new SearchResult
        {
            Id = 1,
            Keywords = "land registry search",
            SearchTarget = "www.infotrack.co.uk",
            SearchEngine = SearchEngine.Google,
            SearchDate = DateTime.UtcNow,
            Positions = new List<SearchResultPosition>()
        };

        await databaseContext.SearchResults.AddAsync(existingResult);
        await databaseContext.SaveChangesAsync();

        var resolver = Substitute.For<ISearchStrategyResolver>();
        var logger = Substitute.For<ILogger<SearchService>>();
        var service = new SearchService(databaseContext, resolver, logger);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchTarget = "www.infotrack.co.uk",
            SearchEngine = SearchEngine.Google
        };

        var response = await service.CreateSearchResultAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(-1, response.Data);
        Assert.Equal("Search already performed today", response.Message);
    }


    [Fact]
    public async Task GetSearchEngineGraphData_GivenValidSearchResultData_ShouldReturnGraphData()
    {
        var databaseContext = CreateInMemoryDbContext();
    
        var today = DateTime.UtcNow.Date;
        var result = new SearchResult
        {
            Id = 1,
            SearchEngine = SearchEngine.Google,
            SearchDate = today,
            Keywords = "land registry search",
            SearchTarget = "www.infotrack.co.uk",
            Positions = new List<SearchResultPosition>
            {
                new SearchResultPosition(1, 1),
                new SearchResultPosition(1, 2),
                new SearchResultPosition(1, 3),
            }
        };
    
        databaseContext.SearchResults.Add(result);
        await databaseContext.SaveChangesAsync();
    
        var resolver = Substitute.For<ISearchStrategyResolver>();
        var logger = Substitute.For<ILogger<SearchService>>();
        var service = new SearchService(databaseContext, resolver, logger);
    
        var response = await service.GetSearchEngineGraphData(7);
    
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.SearchEnginePlotData.ContainsKey(SearchEngine.Google));
        var data = response.Data.SearchEnginePlotData[SearchEngine.Google];
    
        Assert.Single(data.SearchHistory);
        Assert.Single(data.PositionHistory);
        Assert.Equal(3, data.SearchHistory.First().TotalCount);
        Assert.Equal(new List<int> { 1, 2, 3 }, data.PositionHistory.First().Positions);
    }
}