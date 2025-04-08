using System.Diagnostics.CodeAnalysis;

namespace InfoTrack.Domain.Models;

[ExcludeFromCodeCoverage]
public class SearchResultPosition
{
    public int SearchResultId { get; init; }
    public int Position { get; init; }

    public SearchResultPosition()
    {
    }

    public SearchResultPosition(
        int position
    )
    {
        Position = position;
    }

    public SearchResultPosition(
        int searchResultId,
        int position
    )
    {
        SearchResultId = searchResultId;
        Position = position;
    }
}