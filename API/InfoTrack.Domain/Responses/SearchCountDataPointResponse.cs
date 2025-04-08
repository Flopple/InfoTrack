using System;
using System.Diagnostics.CodeAnalysis;

namespace InfoTrack.Domain.Responses;

[ExcludeFromCodeCoverage]
public record SearchCountDataPointResponse
{
    public DateTime SearchDate { get; set; }
    public int TotalCount { get; set; }
}