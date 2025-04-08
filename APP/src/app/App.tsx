import '../styles/App.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { SearchPage } from '../features/search/pages/search.page.tsx';
import { ToastProvider } from '@/components/shared/toast-provider.component.tsx';

const queryClient = new QueryClient();

function App() {
  return (
    <>
      <QueryClientProvider client={queryClient}>
        <ToastProvider>
          <SearchPage />
        </ToastProvider>
      </QueryClientProvider>
    </>
  );
}

export default App;
