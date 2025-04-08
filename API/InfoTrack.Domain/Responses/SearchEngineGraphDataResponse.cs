using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using InfoTrack.Domain.Enums;

namespace InfoTrack.Domain.Responses;

[ExcludeFromCodeCoverage]
public record SearchEngineGraphDataResponse
{
    public Dictionary<SearchEngine, SearchEnginePlotDataResponse> SearchEnginePlotData { get; init; }

    public SearchEngineGraphDataResponse(Dictionary<SearchEngine, SearchEnginePlotDataResponse> searchEnginePlotData)
    {
        SearchEnginePlotData = searchEnginePlotData;
    }
}