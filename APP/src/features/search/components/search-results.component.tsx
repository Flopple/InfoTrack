import React from 'react';
import { ApiResponse } from '@/common/responses/api.response.ts';
import { SearchResultResponse } from '@/features/search/responses/search-result.response.ts';

export interface SearchResultsProps {
  result: ApiResponse<SearchResultResponse> | null;
}

export const SearchResults: React.FC<SearchResultsProps> = ({ result }) => {
  if (!result) return null;

  return (
    <div className="p-6 bg-gray-100 rounded-lg">
      <h2 className="text-xl font-bold mb-4">Search Results</h2>

      <div className="bg-green-100 p-4 rounded-lg mb-4">
        <p className="mb-2">
          <span className="font-bold">Keywords:</span> {result.data.keywords}
        </p>
        <p className="mb-2">
          <span className="font-bold">Search Target:</span> {result.data.searchTarget}
        </p>
        <p className="mb-2">
          <span className="font-bold">Positions:</span> {result.data.positions.join(', ')}
        </p>
        <p>
          <span className="font-bold">Total:</span> {result.data.positions.length}
        </p>
      </div>

      {result.data.positions.length > 0 ? (
        <div>
          <h3 className="text-lg font-bold mb-2">Positions Detail</h3>
          <ul>
            {result.data.positions.map((position, index) => (
              <li key={index} className="bg-gray-200 p-2 rounded mb-2">
                Found at position {position} in search results
              </li>
            ))}
          </ul>
        </div>
      ) : (
        <p className="text-gray-500 italic">
          The search target was not found in the search results.
        </p>
      )}
    </div>
  );
};
