import { PaginationParams } from '@/common/params/pagination.params.ts';

export const createPaginatedSearchParams = (params: PaginationParams) => {
  const searchParams = new URLSearchParams();
  searchParams.append('pageNumber', params.pageNumber.toString());
  searchParams.append('pageSize', params.pageSize.toString());

  return searchParams;
};
