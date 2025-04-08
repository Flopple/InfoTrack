import React from 'react';
import { format } from 'date-fns';
import {
  ScatterChart,
  Scatter,
  XAxis,
  YAxis,
  ZAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from 'recharts';
import { SearchEngine } from '@/common/enums/SearchEngine.ts';
import { SearchEnginePlotDataResponse } from '@/features/search/responses/search-engine-plot-data.response.ts';

interface PositionsScatterChartProps {
  data: Record<SearchEngine, SearchEnginePlotDataResponse>;
  selectedEngines: SearchEngine[];
}

interface ScatterPoint {
  x: number;
  y: number;
  searchEngine: string;
}

const ENGINE_COLOURS = {
  [SearchEngine.Google]: '#4285F4',
  [SearchEngine.Yahoo]: '#720E9E',
};

export const PositionsScatterChart: React.FC<PositionsScatterChartProps> = ({
  data,
  selectedEngines,
}) => {
  const prepareChartData = () => {
    const scatterData: ScatterPoint[] = [];

    selectedEngines.forEach(searchEngine => {
      const searchEngineEnum = SearchEngine[searchEngine as unknown as keyof typeof SearchEngine];

      if (data[searchEngineEnum]) {
        data[searchEngineEnum].positionHistory.forEach(point => {
          const searchDate = new Date(point.searchDate).getTime();

          point.positions.forEach(position => {
            scatterData.push({
              x: searchDate,
              y: position,
              searchEngine: SearchEngine[searchEngine],
            });
          });
        });
      }
    });

    return scatterData;
  };

  const groupDataByEngine = () => {
    const result: { [key: string]: ScatterPoint[] } = {};

    selectedEngines.forEach(searchEngine => {
      const engineEnum = SearchEngine[searchEngine as unknown as keyof typeof SearchEngine];

      if (data[engineEnum]) {
        const engineName = SearchEngine[searchEngine];
        result[engineName] = prepareChartData().filter(point => point.searchEngine === engineName);
      }
    });

    return result;
  };

  const groupedData = groupDataByEngine();

  const hasData = Object.values(groupedData).some(points => points.length > 0);

  if (!hasData) {
    return (
      <div className="flex justify-center items-center h-64 bg-gray-50 rounded-md">
        <p className="text-gray-500">No position data available for the selected search engines</p>
      </div>
    );
  }

  const allPoints = Object.values(groupedData).flat();
  const minDate = Math.min(...allPoints.map(p => p.x));
  const maxDate = Math.max(...allPoints.map(p => p.x));

  const PositionRankingTooltip = ({ active, payload }: any) => {
    if (active && payload && payload.length) {
      const data: ScatterPoint = payload[0].payload;
      return (
        <div className="p-2 bg-white border border-gray-200 shadow-sm rounded">
          <p className="font-medium">{format(data.x, 'MMMM dd, yyyy')}</p>
          <p className="text-sm">
            <span className="font-semibold">{data.searchEngine}:</span> Position {data.y}
          </p>
        </div>
      );
    }
    return null;
  };

  return (
    <div className="h-96 w-full">
      <ResponsiveContainer width="100%" height="100%">
        <ScatterChart margin={{ top: 20, right: 20, bottom: 20, left: 20 }}>
          <CartesianGrid />
          <XAxis
            type="number"
            dataKey="x"
            domain={[minDate, maxDate]}
            name="Date"
            tickFormatter={timestamp => format(timestamp, 'MMM dd')}
          />
          <YAxis
            type="number"
            dataKey="y"
            name="Position"
            domain={[1, 'dataMax']}
            label={{ value: 'Position', angle: -90, position: 'insideLeft' }}
          />
          <ZAxis range={[60, 60]} />
          <Tooltip content={<PositionRankingTooltip active payload />} />
          <Legend />

          {selectedEngines.map(searchEngine => {
            const searchEngineName = SearchEngine[searchEngine];
            if (!groupedData[searchEngineName] || groupedData[searchEngineName].length === 0)
              return null;

            return (
              <Scatter
                key={searchEngineName}
                name={searchEngineName}
                data={groupedData[searchEngineName]}
                fill={ENGINE_COLOURS[searchEngine]}
              />
            );
          })}
        </ScatterChart>
      </ResponsiveContainer>
    </div>
  );
};
