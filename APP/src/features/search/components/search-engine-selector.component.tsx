import React from 'react';
import { SearchEngine } from '@/common/enums/SearchEngine.ts';

export interface SearchEngineSelectorProps {
  selectedEngine: SearchEngine;
  onEngineChange: (engine: SearchEngine) => void;
}

export const SearchEngineSelector: React.FC<SearchEngineSelectorProps> = ({
  selectedEngine,
  onEngineChange,
}) => {
  return (
    <div className="mb-4">
      <label className="block font-bold mb-2" htmlFor="searchEngine">
        Search Engine:
      </label>
      <select
        id="searchEngine"
        className="w-full p-2 border border-gray-300 rounded"
        value={selectedEngine}
        onChange={e => onEngineChange(parseInt(e.target.value) as SearchEngine)}
      >
        <option value={SearchEngine.Google.toString()}>Google</option>
        <option value={SearchEngine.Yahoo.toString()}>Yahoo</option>
      </select>
    </div>
  );
};
