import { CreateSearchResultRequest } from '../requests/create-search-result.request.ts';
import {
  createSearchResult,
  getSearchEngineGraphData,
  getSearchResultById,
  getSearchResults,
} from '../services/search.service.ts';
import { useMutation, useQuery } from '@tanstack/react-query';
import { ApiResponse } from '@/common/responses/api.response.ts';
import { SearchResultResponse } from '../responses/search-result.response.ts';
import { PaginatedResponse } from '@/common/responses/paginated.response.ts';
import { PaginationParams } from '@/common/params/pagination.params.ts';
import { SearchEngineGraphDataResponse } from '@/features/search/responses/search-engine-graph-data.response.ts';

export const useGetSearchResultById = (id: number) =>
  useQuery({
    queryKey: ['getSearchResultById', id],
    queryFn: (): Promise<ApiResponse<SearchResultResponse>> => getSearchResultById(id),
    enabled: !!id,
  });

export const useGetSearchResults = (params: PaginationParams) =>
  useQuery({
    queryKey: ['getSearchResults', params.pageNumber, params.pageSize],
    queryFn: (): Promise<ApiResponse<PaginatedResponse<SearchResultResponse>>> =>
      getSearchResults(params),
  });

export const useCreateSearchResult = () =>
  useMutation({
    mutationFn: (request: CreateSearchResultRequest) => createSearchResult(request),
  });

export const useSearchEngineGraphData = (days: number) => {
  return useQuery({
    queryKey: ['getSearchEngineGraphData', days],
    queryFn: (): Promise<ApiResponse<SearchEngineGraphDataResponse>> =>
      getSearchEngineGraphData(days),
    select: response => response.data,
  });
};
