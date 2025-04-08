using InfoTrack.Common.Settings;
using InfoTrack.Domain.Enums;
using InfoTrack.Services.Implementations;
using InfoTrack.Services.Implementations.SearchStrategies;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace InfoTrack.Tests.ServiceTests;

public class SearchStrategyResolverTests
{
    private readonly IOptions<InfoTrackSettings> _infoTrackSettingsMock;

    public SearchStrategyResolverTests()
    {
        var settings = new InfoTrackSettings();

        _infoTrackSettingsMock = Substitute.For<IOptions<InfoTrackSettings>>();
        _infoTrackSettingsMock.Value.Returns(settings);
    }

    [Fact]
    public void GetStrategies_ShouldContainExpectedStrategies()
    {
        var googleStrategy = Substitute.For<GoogleSearchStrategy>(_infoTrackSettingsMock, null);
        var yahooStrategy = Substitute.For<YahooSearchStrategy>(_infoTrackSettingsMock, null);

        var resolver = new SearchStrategyResolver(googleStrategy, yahooStrategy);

        var strategies = resolver.GetStrategies();

        Assert.True(strategies.ContainsKey(SearchEngine.Google));
        Assert.True(strategies.ContainsKey(SearchEngine.Yahoo));
        Assert.Equal(googleStrategy.PerformSearchAsync, strategies[SearchEngine.Google]);
        Assert.Equal(yahooStrategy.PerformSearchAsync, strategies[SearchEngine.Yahoo]);
    }
}