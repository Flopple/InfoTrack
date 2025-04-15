using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using InfoTrack.Domain.Requests;
using InfoTrack.Services.Interfaces;
using InfoTrack.Web.API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.Web.API.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("[controller]")]
public class SearchResultsController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchResultsController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSearchResults([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var response = await _searchService.GetSearchResultsAsync(pageNumber, pageSize);
        return ApiResponseFactory.CreateResponse(response);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSearchResultById(int id)
    {
        var response = await _searchService.GetSearchResultByIdAsync(id);
        return ApiResponseFactory.CreateResponse(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSearchResult([FromBody] CreateSearchResultRequest request)
    {
        var response = await _searchService.CreateSearchResultAsync(request);
        return ApiResponseFactory.CreateResponse(response);
    }

    [HttpGet("search-engine-graph-data")]
    public async Task<IActionResult> GetSearchEngineGraphDataResponse([FromQuery] int days = 30)
    {
        var response = await _searchService.GetSearchEngineGraphData(days);
        return ApiResponseFactory.CreateResponse(response);
    }
}