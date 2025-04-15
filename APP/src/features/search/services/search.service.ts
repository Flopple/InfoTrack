import { httpClient } from '@/api/http-client.ts';
import { CreateSearchResultRequest } from '../requests/create-search-result.request.ts';
import { SearchResultResponse } from '../responses/search-result.response.ts';
import { ApiResponse } from '@/common/responses/api.response.ts';
import { PaginatedResponse } from '@/common/responses/paginated.response.ts';
import { PaginationParams } from '@/common/params/pagination.params.ts';
import { createPaginatedSearchParams } from '@/common/params/paginated-search.params.ts';
import { SearchEngineGraphDataResponse } from '@/features/search/responses/search-engine-graph-data.response.ts';

const controller = 'searchresults';

export const getSearchResultById = async (
  id: number
): Promise<ApiResponse<SearchResultResponse>> => {
  return await httpClient.get<ApiResponse<SearchResultResponse>>(`${controller}/${id}`).json();
};

export const getSearchResults = async (
  params: PaginationParams
): Promise<ApiResponse<PaginatedResponse<SearchResultResponse>>> => {
  const searchParams = createPaginatedSearchParams(params);

  return await httpClient
    .get<ApiResponse<PaginatedResponse<SearchResultResponse>>>(`${controller}`, {
      searchParams,
    })
    .json();
};

export const createSearchResult = async (
  request: CreateSearchResultRequest
): Promise<ApiResponse<number>> => {
  return await httpClient
    .post(`${controller}/create`, {
      json: request,
    })
    .json();
};

export const getSearchEngineGraphData = async (
  days: number
): Promise<ApiResponse<SearchEngineGraphDataResponse>> => {
  return await httpClient
    .get<ApiResponse<SearchEngineGraphDataResponse>>(`${controller}/search-engine-graph-data`, {
      searchParams: { days },
    })
    .json();
};
