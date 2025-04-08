import { SearchEnginePlotDataResponse } from '@/features/search/responses/search-engine-plot-data.response.ts';
import { SearchEngine } from '@/common/enums/SearchEngine.ts';

export interface SearchEngineGraphDataResponse {
  searchEnginePlotData: Record<SearchEngine, SearchEnginePlotDataResponse>;
}
