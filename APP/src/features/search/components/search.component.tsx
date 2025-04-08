import React, { useState } from 'react';
import { SearchForm } from './search-form.component';
import { SearchResults } from './search-results.component';
import { SearchHistory } from './search-history.component';
import { SearchEngineVisualisation } from './search-engine-visualisation.component';
import { CreateSearchResultRequest } from '../requests/create-search-result.request';
import {
  useCreateSearchResult,
  useGetSearchResultById,
  useGetSearchResults,
} from '@/features/search/hooks/search-result.hook.ts';
import { Header } from '@/components/layout/header.component.tsx';
import { Tabs } from '@/components/layout/tabs.component.tsx';
import { SearchResultResponse } from '@/features/search/responses/search-result.response.ts';
import { ApiResponse } from '@/common/responses/api.response.ts';
import { PaginationParams } from '@/common/params/pagination.params.ts';
import { SearchEngine } from '@/common/enums/SearchEngine.ts';
import { useToast } from '@/components/shared/toast-provider.component.tsx';

export const SearchComponent: React.FC = () => {
  const [activeTab, setActiveTab] = useState('search');
  const [currentResult, setCurrentResult] = useState<ApiResponse<SearchResultResponse> | null>(
    null
  );
  const [lastSearchId, setLastSearchId] = useState<number | null>(null);

  const [pagination, setPagination] = useState<PaginationParams>({
    pageNumber: 1,
    pageSize: 10,
  });

  const createSearchResultMutation = useCreateSearchResult();
  const searchResultsQuery = useGetSearchResults(pagination);
  const searchResultByIdQuery = useGetSearchResultById(lastSearchId || 0);

  const searchResultsQueryData = searchResultsQuery.data;

  const { showToast } = useToast();

  const handlePageChange = (page: number) => {
    setPagination(prev => ({
      ...prev,
      pageNumber: page,
    }));
  };

  const handleSearch = (keywords: string, targetUrl: string, searchEngine: SearchEngine) => {
    const request = new CreateSearchResultRequest(keywords, targetUrl, searchEngine);

    createSearchResultMutation.mutate(request, {
      onSuccess: (data: ApiResponse<number>) => {
        setLastSearchId(data.data);

        if (searchResultByIdQuery.data) {
          setCurrentResult(searchResultByIdQuery.data);
        }

        setActiveTab('results');

        searchResultsQuery.refetch();

        if (data.message) {
          showToast(data.message, 'success');
        }
      },
      onError: async (error: any) => {
        const response = error.response;

        if (response?.json) {
          const data = await response.json();

          if (data.errors) {
            const errorValues = Object.values(data.errors) as string[][];
            errorValues.flat().forEach(msg => showToast(msg, 'error'));
            return;
          }

          if (data.message) {
            showToast(data.message, 'error');
            return;
          }
        }

        showToast('An error occurred', 'error');
      },
    });
  };

  React.useEffect(() => {
    if (searchResultByIdQuery.data && lastSearchId) {
      setCurrentResult(searchResultByIdQuery.data);
    }
  }, [searchResultByIdQuery.data, lastSearchId]);

  const searchHistory = searchResultsQueryData?.data?.items || [];
  const totalPages = searchResultsQueryData?.data?.totalPages || 0;
  const totalCount = searchResultsQueryData?.data?.totalCount || 0;

  const hasTrendsData = searchHistory.length > 0;

  return (
    <div className="max-w-4xl mx-auto p-4">
      <Header />

      <Tabs activeTab={activeTab} setActiveTab={setActiveTab} hasResults={!!currentResult} />

      {activeTab === 'search' && (
        <SearchForm onSearch={handleSearch} loading={createSearchResultMutation.isPending} />
      )}

      {activeTab === 'results' && <SearchResults result={currentResult} />}

      {activeTab === 'history' && (
        <SearchHistory
          history={searchHistory}
          isLoading={searchResultsQuery.isLoading}
          pageNumber={pagination.pageNumber}
          pageSize={pagination.pageSize}
          totalPages={totalPages}
          totalCount={totalCount}
          onPageChange={handlePageChange}
        />
      )}

      {activeTab === 'trends' && hasTrendsData && (
        <div className="p-6 bg-gray-100 rounded-lg">
          <h2 className="text-xl font-bold mb-4">Search Trends</h2>
          <SearchEngineVisualisation />
        </div>
      )}

      {activeTab === 'trends' && !hasTrendsData && (
        <div className="p-6 bg-gray-100 rounded-lg">
          <h2 className="text-xl font-bold mb-4">Search Trends</h2>
          <p className="text-gray-500 italic">No search history available for trends analysis.</p>
        </div>
      )}
    </div>
  );
};
