using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrack.Domain.Enums;
using InfoTrack.Domain.Models;
using InfoTrack.Domain.Requests;

namespace InfoTrack.Services.Interfaces;

public interface ISearchStrategyResolver
{
    Dictionary<SearchEngine, Func<CreateSearchResultRequest, Task<SearchResult>>> GetStrategies();
}