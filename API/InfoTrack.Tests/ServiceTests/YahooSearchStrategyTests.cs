using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InfoTrack.Common.Settings;
using InfoTrack.Domain.Enums;
using InfoTrack.Domain.Models;
using InfoTrack.Domain.Requests;
using InfoTrack.Services.Implementations.SearchStrategies;
using InfoTrack.Services.Interfaces;
using InfoTrack.Tests.ServiceTests.Helpers;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace InfoTrack.Tests.ServiceTests;

public class YahooSearchStrategyTests
{
    private ISearchStrategy _yahooSearchStrategy;
    private readonly IOptions<InfoTrackSettings> _infoTrackSettingsMock;

    public YahooSearchStrategyTests()
    {
        var settings = new InfoTrackSettings
        {
            UseDummyResponse = false,
            Yahoo = new SearchEngineSettings
            {
                BaseUrl = "https://search.yahoo.com/search",
                MaxResults = 100,
                DefaultRequestHeaders = "Default Headers"
            }
        };

        _infoTrackSettingsMock = Substitute.For<IOptions<InfoTrackSettings>>();
        _infoTrackSettingsMock.Value.Returns(settings);
    }

    [Fact]
    public async Task PerformSearchAsync_GivenValidHtmlAndMultipleMatches_ShouldReturnCorrectPositions()
    {
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        var mockHttpClient = new HttpMessageHandlerMock(YahooResponseHelper.GetYahooHtmlWithMatches())
            .ToHttpClient();

        mockHttpClientFactory
            .CreateClient("YahooSearch")
            .Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Yahoo,
            SearchTarget = "www.infotrack.co.uk"
        };

        _yahooSearchStrategy = new YahooSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await _yahooSearchStrategy.PerformSearchAsync(request);

        var expected = new SearchResult
        {
            Positions = new List<SearchResultPosition>
            {
                new SearchResultPosition { Position = 2 },
                new SearchResultPosition { Position = 4 },
                new SearchResultPosition { Position = 5 }
            }
        };

        Assert.Equal(expected.Positions.Count(), actual.Positions.Count());
        for (var i = 0; i < expected.Positions.Count(); i++)
        {
            Assert.Equal(
                expected.Positions.ElementAt(i).Position,
                actual.Positions.ElementAt(i).Position
            );
        }
    }

    [Fact]
    public async Task PerformSearchAsync_GivenNoMatches_ShouldReturnEmptyPositions()
    {
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        var mockHttpClient = new HttpMessageHandlerMock(YahooResponseHelper.GetYahooHtmlWithNoMatches())
            .ToHttpClient();

        mockHttpClientFactory.CreateClient("YahooSearch").Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Yahoo,
            SearchTarget = "www.infotrack.co.uk"
        };

        _yahooSearchStrategy = new YahooSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await _yahooSearchStrategy.PerformSearchAsync(request);

        Assert.Empty(actual.Positions);
    }

    [Fact]
    public async Task PerformSearchAsync_GivenDifferentClassNames_ShouldFindMatchesCorrectly()
    {
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        var mockHttpClient = new HttpMessageHandlerMock(YahooResponseHelper.GetYahooHtmlWithDifferentClassNames())
            .ToHttpClient();

        mockHttpClientFactory.CreateClient("YahooSearch").Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Yahoo,
            SearchTarget = "www.infotrack.co.uk"
        };

        var expected = new SearchResult
        {
            Positions = new List<SearchResultPosition>
            {
                new SearchResultPosition { Position = 1 },
                new SearchResultPosition { Position = 3 }
            }
        };

        _yahooSearchStrategy = new YahooSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await _yahooSearchStrategy.PerformSearchAsync(request);

        Assert.Equal(expected.Positions.Count(), actual.Positions.Count());
        for (var i = 0; i < expected.Positions.Count(); i++)
        {
            Assert.Equal(
                expected.Positions.ElementAt(i).Position,
                actual.Positions.ElementAt(i).Position
            );
        }
    }

    [Fact]
    public async Task PerformSearchAsync_GivenVariantDomains_ShouldFindExactMatchesOnly()
    {
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        var mockHttpClient = new HttpMessageHandlerMock(YahooResponseHelper.GetYahooHtmlWithVariantDomains())
            .ToHttpClient();

        mockHttpClientFactory.CreateClient("YahooSearch").Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Yahoo,
            SearchTarget = "www.infotrack.co.uk"
        };

        _yahooSearchStrategy = new YahooSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await _yahooSearchStrategy.PerformSearchAsync(request);

        Assert.Single(actual.Positions);
        Assert.Equal(4, actual.Positions.First().Position);
    }
}