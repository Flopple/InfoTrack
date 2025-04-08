import React, { useState } from 'react';
import { SearchEngine } from '@/common/enums/SearchEngine.ts';
import { SearchEngineSelector } from './search-engine-selector.component';

export interface SearchFormProps {
  onSearch: (keywords: string, searchTarget: string, searchEngine: SearchEngine) => void;
  loading: boolean;
}

export const SearchForm: React.FC<SearchFormProps> = ({ onSearch, loading }) => {
  const [keywords, setKeywords] = useState('land registry search');
  const [searchTarget, setSearchTarget] = useState('www.infotrack.co.uk');
  const [searchEngine, setSearchEngine] = useState<SearchEngine>(SearchEngine.Google);

  return (
    <div className="p-6 bg-gray-100 rounded-lg">
      <h2 className="text-xl font-bold mb-4">Search Engine Rankings</h2>
      <p className="mb-4">Enter keywords and search target to find positions in search results.</p>

      <form
        onSubmit={e => {
          e.preventDefault();
          onSearch(keywords, searchTarget, searchEngine);
        }}
      >
        <div className="mb-4">
          <label className="block font-bold mb-2" htmlFor="keywords">
            Keywords:
          </label>
          <input
            className="w-full p-2 border border-gray-300 rounded"
            type="text"
            id="keywords"
            value={keywords}
            onChange={e => setKeywords(e.target.value)}
            required
          />
        </div>

        <div className="mb-4">
          <label className="block font-bold mb-2" htmlFor="searchTarget">
            Search Target:
          </label>
          <input
            className="w-full p-2 border border-gray-300 rounded"
            type="text"
            id="searchTarget"
            value={searchTarget}
            onChange={e => setSearchTarget(e.target.value)}
            required
          />
        </div>

        <SearchEngineSelector selectedEngine={searchEngine} onEngineChange={setSearchEngine} />

        <button
          className="bg-green-500 hover:bg-green-600 text-white py-2 px-4 rounded"
          type="submit"
          disabled={loading}
        >
          {loading ? 'Searching...' : 'Search'}
        </button>
      </form>
    </div>
  );
};
