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

public class GoogleSearchStrategy : ISearchStrategy
{
    private readonly InfoTrackSettings _infoTrackSettings;
    private readonly IHttpClientFactory _httpClientFactory;

    public GoogleSearchStrategy(IOptions<InfoTrackSettings> options, IHttpClientFactory httpClientFactory)
    {
        _infoTrackSettings = options.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<SearchResult> PerformSearchAsync(CreateSearchResultRequest request)
    {
        // For demonstration purposes for if Google detects this is a scraper / bot.
        var useDummyResponse = _infoTrackSettings.UseDummyResponse;
        if (useDummyResponse)
        {
            return DummySearchResult.GetDummySearchResult(request);
        }

        var baseUrl = _infoTrackSettings.Google.BaseUrl;
        var defaultRequestHeader = _infoTrackSettings.Google.DefaultRequestHeaders;
        var maxResults = _infoTrackSettings.Google.MaxResults;

        var query = $"{baseUrl}?num={maxResults}&q={Uri.EscapeDataString(request.Keywords)}";

        var httpClient = _httpClientFactory.CreateClient("GoogleSearch");
        httpClient.DefaultRequestHeaders.Add("User-Agent", defaultRequestHeader);

        var response = await httpClient.GetStringAsync(query);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(response);

        var searchResults = GetSearchResultNodes(htmlDoc,
            "//div[@class='g']",
            "//div[contains(@class, 'tF2Cxc')]",
            "//div[contains(@class, 'yuRUbf')]");

        var positions = new List<SearchResultPosition>();

        if (searchResults != null)
        {
            for (var i = 0; i < searchResults.Count; i++)
            {
                var resultHtml = searchResults[i].InnerHtml;
                if (resultHtml.Contains(request.SearchTarget))
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