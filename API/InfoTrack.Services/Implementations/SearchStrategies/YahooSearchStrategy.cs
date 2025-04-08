using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using InfoTrack.Common.Settings;
using InfoTrack.Domain.Models;
using InfoTrack.Domain.Requests;
using InfoTrack.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace InfoTrack.Services.Implementations.SearchStrategies;

public class YahooSearchStrategy : ISearchStrategy
{
    private readonly InfoTrackSettings _infoTrackSettings;
    private readonly IHttpClientFactory _httpClientFactory;

    public YahooSearchStrategy(IOptions<InfoTrackSettings> options, IHttpClientFactory httpClientFactory)
    {
        _infoTrackSettings = options.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<SearchResult> PerformSearchAsync(CreateSearchResultRequest request)
    {
        // For demonstration purposes for if Yahoo detects this is a scraper / bot.
        if (_infoTrackSettings.UseDummyResponse)
        {
            return DummySearchResult.GetDummySearchResult(request);
        }

        var baseUrl = _infoTrackSettings.Yahoo.BaseUrl;
        var defaultRequestHeader = _infoTrackSettings.Yahoo.DefaultRequestHeaders;
        var maxResults = _infoTrackSettings.Yahoo.MaxResults;

        var query = $"{baseUrl}?n={maxResults}&p={Uri.EscapeDataString(request.Keywords)}";

        var httpClient = _httpClientFactory.CreateClient("YahooSearch");
        httpClient.DefaultRequestHeaders.Add("User-Agent", defaultRequestHeader);

        var response = await httpClient.GetStringAsync(query);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(response);

        var searchResults = GetSearchResultNodes(htmlDoc,
            "//div[contains(@class, 'Sr')]",
            "//div[contains(@class, 'dd algo')]",
            "//li[contains(@class, 'ov-a fz-16')]"
        );

        var positions = new List<SearchResultPosition>();

        if (searchResults != null)
        {
            for (var i = 0; i < searchResults.Count; i++)
            {
                var resultHtml = searchResults[i].InnerHtml;
                if (resultHtml.Contains(request.SearchTarget, StringComparison.OrdinalIgnoreCase))
                {
                    positions.Add(new SearchResultPosition(i + 1));
                }
            }
        }

        return new SearchResult(
            request.Keywords,
            request.SearchTarget,
            request.SearchEngine,
            positions,
            DateTime.UtcNow
        );
    }

    private List<HtmlNode> GetSearchResultNodes(HtmlDocument htmlDoc, params string[] selectors)
    {
        var results = new List<HtmlNode>();

        foreach (var selector in selectors)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes(selector);
            if (nodes != null)
            {
                results.AddRange(nodes);
            }
        }

        return results;
    }
}