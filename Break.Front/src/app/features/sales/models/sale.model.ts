import { SaleItem } from './sale-item.model';

/**
 * Represents a sale transaction in the frontend.
 * Corresponds to the SaleResponse from the API.
 */
export interface Sale {
  saleId: number;
  saleDate: string; // Use string for ISO date format from API
  status: string; // Consider using an enum if statuses are well-defined
  subTotal: number;
  discountAmount: number;
  total: number;
  saleItems: SaleItem[];
}