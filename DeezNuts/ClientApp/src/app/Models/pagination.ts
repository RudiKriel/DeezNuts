export interface Pagination {
  currentPage: Number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class PaginatedResult<T> {
  result!: T;
  pagination!: Pagination;
}
