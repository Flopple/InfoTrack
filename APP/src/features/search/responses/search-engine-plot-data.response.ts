import { PositionDataPointResponse } from '@/features/search/responses/position-data-point.response.ts';
import { SearchCountDataPointResponse } from '@/features/search/responses/search-count-data-point.response.ts';

export interface SearchEnginePlotDataResponse {
  searchHistory: SearchCountDataPointResponse[];
  positionHistory: PositionDataPointResponse[];
}
