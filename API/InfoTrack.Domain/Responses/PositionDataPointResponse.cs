using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace InfoTrack.Domain.Responses;

[ExcludeFromCodeCoverage]
public record PositionDataPointResponse
{
    public DateTime SearchDate { get; set; }
    public IEnumerable<int> Positions { get; set; }
}