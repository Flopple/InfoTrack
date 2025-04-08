using InfoTrack.Domain.Enums;

namespace InfoTrack.Domain.Requests;

public record CreateSearchResultRequest
{
    public string Keywords { get; set; }
    public string SearchTarget { get; set; }
    public SearchEngine SearchEngine { get; set; }
}