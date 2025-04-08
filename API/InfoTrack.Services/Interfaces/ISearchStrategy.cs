using System.Threading.Tasks;
using InfoTrack.Domain.Models;
using InfoTrack.Domain.Requests;

namespace InfoTrack.Services.Interfaces;

public interface ISearchStrategy
{
    Task<SearchResult> PerformSearchAsync(CreateSearchResultRequest request);
}