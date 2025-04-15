import React, { useState } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';
import { SearchEngine } from '@/common/enums/SearchEngine.ts';
import { useSearchEngineGraphData } from '@/features/search/hooks/search-result.hook.ts';
import { PositionsScatterChart } from '@/features/search/components/positions-scatter-chart.component.tsx';
import { SearchCountsChart } from '@/features/search/components/search-counts-chart.component.tsx';

const dayOptions = [7, 14, 30, 60, 90];

export const SearchEngineVisualisation: React.FC = () => {
  const [days, setDays] = useState<number>(30);
  const [selectedEngines, setSelectedEngines] = useState<SearchEngine[]>([
    SearchEngine.Google,
    SearchEngine.Yahoo,
  ]);

  const { data, isLoading, isError } = useSearchEngineGraphData(days);

  const toggleEngine = (searchEngine: SearchEngine) => {
    if (selectedEngines.includes(searchEngine)) {
      setSelectedEngines(selectedEngines.filter(engine => engine !== searchEngine));
    } else {
      setSelectedEngines([...selectedEngines, searchEngine]);
    }
  };

  const handleDaysChange = (value: string) => {
    setDays(parseInt(value));
  };

  if (isLoading) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Search Engine Trends</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="flex justify-center py-8">
            <div className="animate-spin rounded-full h-10 w-10 border-b-2 border-gray-800"></div>
          </div>
        </CardContent>
      </Card>
    );
  }

  if (isError || !data) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Search Engine Trends</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-red-500">Error loading data. Please try again later.</p>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Search Engine Trends</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="flex justify-between mb-6">
          <div>
            <Label htmlFor="timeRange">Time Range:</Label>
            <Select value={days.toString()} onValueChange={handleDaysChange}>
              <SelectTrigger className="w-[180px]" id="timeRange">
                <SelectValue placeholder="Select days" />
              </SelectTrigger>
              <SelectContent>
                {dayOptions.map(option => (
                  <SelectItem key={option} value={option.toString()}>
                    Last {option} days
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="flex gap-4 items-center">
            <div className="flex items-center space-x-2">
              <Checkbox
                id="google"
                checked={selectedEngines.includes(SearchEngine.Google)}
                onCheckedChange={() => toggleEngine(SearchEngine.Google)}
              />
              <Label htmlFor="google" className="text-blue-600 font-semibold">
                Google
              </Label>
            </div>

            <div className="flex items-center space-x-2">
              <Checkbox
                id="yahoo"
                checked={selectedEngines.includes(SearchEngine.Yahoo)}
                onCheckedChange={() => toggleEngine(SearchEngine.Yahoo)}
              />
              <Label htmlFor="yahoo" className="text-purple-600 font-semibold">
                Yahoo
              </Label>
            </div>
          </div>
        </div>

        <Tabs defaultValue="counts">
          <TabsList className="grid grid-cols-2 mb-6">
            <TabsTrigger value="counts">Search Counts</TabsTrigger>
            <TabsTrigger value="positions">Position Rankings</TabsTrigger>
          </TabsList>

          <TabsContent value="counts">
            <SearchCountsChart data={data.searchEnginePlotData} selectedEngines={selectedEngines} />
          </TabsContent>

          <TabsContent value="positions">
            <PositionsScatterChart
              data={data.searchEnginePlotData}
              selectedEngines={selectedEngines}
            />
          </TabsContent>
        </Tabs>
      </CardContent>
    </Card>
  );
};
