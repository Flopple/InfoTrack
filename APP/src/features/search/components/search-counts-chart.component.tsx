import React from 'react';
import { format } from 'date-fns';
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from 'recharts';
import { SearchEngine } from '@/common/enums/SearchEngine.ts';
import { SearchEnginePlotDataResponse } from '@/features/search/responses/search-engine-plot-data.response.ts';

interface SearchCountsChartProps {
  data: Record<SearchEngine, SearchEnginePlotDataResponse>;
  selectedEngines: SearchEngine[];
}

const ENGINE_COLORS = {
  [SearchEngine.Google]: '#4285F4',
  [SearchEngine.Yahoo]: '#720E9E',
};

export const SearchCountsChart: React.FC<SearchCountsChartProps> = ({ data, selectedEngines }) => {
  const prepareChartData = () => {
    const dateMap = new Map<string, { date: string; [key: string]: any }>();

    selectedEngines.forEach(searchEngine => {
      const searchEngineEnum = SearchEngine[searchEngine as unknown as keyof typeof SearchEngine];
      if (data[searchEngineEnum]) {
        data[searchEngineEnum].searchHistory.forEach(point => {
          const formattedDate = format(new Date(point.searchDate), 'yyyy-MM-dd');

          if (!dateMap.has(formattedDate)) {
            dateMap.set(formattedDate, { date: formattedDate });
          }

          const engineKey = SearchEngine[searchEngine];
          dateMap.get(formattedDate)![engineKey] = point.totalCount;
        });
      }
    });

    return Array.from(dateMap.values()).sort(
      (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
    );
  };

  const chartData = prepareChartData();

  if (chartData.length === 0) {
    return (
      <div className="flex justify-center items-center h-64 bg-gray-50 rounded-md">
        <p className="text-gray-500">No data available for the selected search engines</p>
      </div>
    );
  }

  return (
    <div className="h-96 w-full">
      <ResponsiveContainer width="100%" height="100%">
        <LineChart data={chartData} margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="date" tickFormatter={date => format(new Date(date), 'MMM dd')} />
          <YAxis />
          <Tooltip
            labelFormatter={label => format(new Date(label), 'MMMM dd, yyyy')}
            formatter={(value, name) => [value, name]}
          />
          <Legend />

          {selectedEngines.map(searchEngine => {
            const searchEngineEnum =
              SearchEngine[searchEngine as unknown as keyof typeof SearchEngine];

            if (!data[searchEngineEnum]) return null;

            return (
              <Line
                key={SearchEngine[searchEngine]}
                type="monotone"
                dataKey={SearchEngine[searchEngine]}
                stroke={ENGINE_COLORS[searchEngine]}
                activeDot={{ r: 8 }}
              />
            );
          })}
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
};
