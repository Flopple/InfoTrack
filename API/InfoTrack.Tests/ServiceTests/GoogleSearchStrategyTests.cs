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

public class GoogleSearchStrategyTests
{
    private ISearchStrategy _googleSearchStrategy;
    private readonly IOptions<InfoTrackSettings> _infoTrackSettingsMock;


    public GoogleSearchStrategyTests()
    {
        var settings = new InfoTrackSettings
        {
            UseDummyResponse = false,
            Google = new SearchEngineSettings
            {
                BaseUrl = "https://www.google.com/search",
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
        var mockHttpClient = new HttpMessageHandlerMock(GoogleResponseHelper.GetGoogleHtmlWithMatches())
            .ToHttpClient();

        mockHttpClientFactory
            .CreateClient("GoogleSearch")
            .Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Google,
            SearchTarget = "www.infotrack.co.uk"
        };

        var expected = new SearchResult
        {
            Positions = new List<SearchResultPosition>
            {
                new SearchResultPosition { Position = 2 },
                new SearchResultPosition { Position = 5 },
                new SearchResultPosition { Position = 9 }
            }
        };

        _googleSearchStrategy = new GoogleSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await _googleSearchStrategy.PerformSearchAsync(request);

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
        var mockHttpClient = new HttpMessageHandlerMock(GoogleResponseHelper.GetGoogleHtmlWithNoMatches())
            .ToHttpClient();

        mockHttpClientFactory
            .CreateClient("GoogleSearch")
            .Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Google,
            SearchTarget = "www.infotrack.co.uk"
        };

        _googleSearchStrategy = new GoogleSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await _googleSearchStrategy.PerformSearchAsync(request);

        Assert.Empty(actual.Positions);
    }


    [Fact]
    public async Task PerformSearchAsync_GivenDifferentClassNames_ShouldFindMatchesCorrectly()
    {
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        var mockHttpClient = new HttpMessageHandlerMock(GoogleResponseHelper.GetGoogleHtmlWithDifferentClassNames())
            .ToHttpClient();

        mockHttpClientFactory.CreateClient("GoogleSearch").Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Google,
            SearchTarget = "www.infotrack.co.uk"
        };
        var expected = new SearchResult
        {
            Positions = new List<SearchResultPosition>
            {
                new SearchResultPosition { Position = 1 },
                new SearchResultPosition { Position = 4 }
            }
        };

        var strategy = new GoogleSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await strategy.PerformSearchAsync(request);

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
    public async Task PerformSearchAsync_GivenVariantDomains_ShouldFindExactMatches()
    {
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        var mockHttpClient = new HttpMessageHandlerMock(GoogleResponseHelper.GetGoogleHtmlWithVariantDomains())
            .ToHttpClient();

        mockHttpClientFactory.CreateClient("GoogleSearch").Returns(mockHttpClient);

        var request = new CreateSearchResultRequest
        {
            Keywords = "land registry search",
            SearchEngine = SearchEngine.Google,
            SearchTarget = "www.infotrack.co.uk"
        };

        var strategy = new GoogleSearchStrategy(_infoTrackSettingsMock, mockHttpClientFactory);

        var actual = await strategy.PerformSearchAsync(request);


        Assert.Single(actual.Positions);
        Assert.Equal(5, actual.Positions.First().Position);
    }
}