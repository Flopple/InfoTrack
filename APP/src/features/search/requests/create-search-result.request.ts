import { SearchEngine } from '@/common/enums/SearchEngine.ts';

export class CreateSearchResultRequest {
  keywords: string;
  searchTarget: string;
  searchEngine: SearchEngine;

  constructor(keywords: string, searchTarget: string, searchEngine: SearchEngine) {
    this.keywords = keywords;
    this.searchTarget = searchTarget;
    this.searchEngine = searchEngine;
  }
}
