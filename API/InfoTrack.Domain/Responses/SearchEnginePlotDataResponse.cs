using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace InfoTrack.Domain.Responses;

[ExcludeFromCodeCoverage]
public record SearchEnginePlotDataResponse
{
    public IEnumerable<SearchCountDataPointResponse> SearchHistory { get; set; }
    public IEnumerable<PositionDataPointResponse> PositionHistory { get; set; }
}