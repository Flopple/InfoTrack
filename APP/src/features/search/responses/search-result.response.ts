import { SearchEngine } from '@/common/enums/SearchEngine.ts';

export interface SearchResultResponse {
  id: number;
  keywords: string;
  searchTarget: string;
  searchEngine: SearchEngine;
  positions: number[];
  searchDate: Date;
}
