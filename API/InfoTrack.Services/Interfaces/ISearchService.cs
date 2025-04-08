using System.Threading.Tasks;
using InfoTrack.Domain.Requests;
using InfoTrack.Domain.Responses;

namespace InfoTrack.Services.Interfaces;

public interface ISearchService
{
    Task<ApiResponse<SearchResultResponse>> GetSearchResultByIdAsync(int id);

    Task<ApiResponse<PaginatedResponse<SearchResultResponse>>> GetSearchResultsAsync(int pageNumber, int pageSize);

    Task<ApiResponse<int>> CreateSearchResultAsync(CreateSearchResultRequest request);
    /// <summary>
    /// Transforms search history data into data to be used in a graph/plot.
    /// </summary>
    /// <param name="days">Number of days to search from.</param>
    /// <returns>SearchEngineGraphDataResponse</returns>
    Task<ApiResponse<SearchEngineGraphDataResponse>> GetSearchEngineGraphData(int days);
}