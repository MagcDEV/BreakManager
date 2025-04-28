/**
 * Represents the pagination metadata received from the API header.
 * Matches the structure of Break.Contracts.Common.PaginationMetadata
 */
export interface PaginationMetadata {
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}