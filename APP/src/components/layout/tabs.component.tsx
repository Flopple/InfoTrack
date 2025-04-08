import React from 'react';

export interface TabProps {
  activeTab: string;
  setActiveTab: (tab: string) => void;
  hasResults: boolean;
}

export const Tabs: React.FC<TabProps> = ({ activeTab, setActiveTab, hasResults }) => {
  return (
    <div className="flex mb-4">
      <button
        className={`flex-1 py-2 ${activeTab === 'search' ? 'bg-gray-300 font-bold' : 'bg-gray-200'}`}
        onClick={() => setActiveTab('search')}
      >
        Search
      </button>

      <button
        className={`flex-1 py-2 ${
          activeTab === 'results'
            ? 'bg-gray-300 font-bold'
            : hasResults
              ? 'bg-gray-200'
              : 'bg-gray-100 text-gray-400 cursor-not-allowed'
        }`}
        onClick={() => hasResults && setActiveTab('results')}
        disabled={!hasResults}
        aria-disabled={!hasResults}
        title={!hasResults ? 'Please make a search' : ''}
      >
        Results
      </button>

      <button
        className={`flex-1 py-2 ${activeTab === 'history' ? 'bg-gray-300 font-bold' : 'bg-gray-200'}`}
        onClick={() => setActiveTab('history')}
      >
        History
      </button>

      <button
        className={`flex-1 py-2 ${activeTab === 'trends' ? 'bg-gray-300 font-bold' : 'bg-gray-200'}`}
        onClick={() => setActiveTab('trends')}
      >
        Trends
      </button>
    </div>
  );
};
