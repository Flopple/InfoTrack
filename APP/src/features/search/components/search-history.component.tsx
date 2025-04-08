import React from 'react';
import { SearchResultResponse } from '../responses/search-result.response';
import { Pagination } from '@/components/layout/pagination.component.tsx';

interface SearchHistoryProps {
  history: SearchResultResponse[];
  isLoading: boolean;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  onPageChange: (page: number) => void;
}

export const SearchHistory: React.FC<SearchHistoryProps> = ({
  history,
  isLoading,
  pageNumber,
  totalPages,
  totalCount,
  onPageChange,
}) => {
  if (isLoading) {
    return (
      <div className="p-6 bg-gray-100 rounded-lg">
        <h2 className="text-xl font-bold mb-4">Search History</h2>
        <div className="flex justify-center py-8">
          <div className="animate-spin rounded-full h-10 w-10 border-b-2 border-gray-800"></div>
        </div>
      </div>
    );
  }

  return (
    <div className="p-6 bg-gray-100 rounded-lg">
      <h2 className="text-xl font-bold mb-4">Search History</h2>

      {history.length === 0 ? (
        <p className="text-gray-500 italic">No search history available.</p>
      ) : (
        <>
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="bg-gray-200">
                  <th className="p-2 text-left">Search Date</th>
                  <th className="p-2 text-left">Keywords</th>
                  <th className="p-2 text-left">Search Target</th>
                  <th className="p-2 text-left">Positions</th>
                  <th className="p-2 text-left">Total</th>
                </tr>
              </thead>
              <tbody>
                {history.map((item, index) => (
                  <tr key={index} className="border-b border-gray-200">
                    <td className="p-2">{new Date(item.searchDate).toLocaleString()}</td>
                    <td className="p-2">{item.keywords}</td>
                    <td className="p-2">{item.searchTarget}</td>
                    <td className="p-2">{item.positions.join(', ')}</td>
                    <td className="p-2">{item.positions.length}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <Pagination
            currentPage={pageNumber}
            totalPages={totalPages}
            onPageChange={onPageChange}
          />

          <div className="mt-2 text-sm text-gray-500 text-right">
            Showing {history.length} of {totalCount} results
          </div>
        </>
      )}
    </div>
  );
};
