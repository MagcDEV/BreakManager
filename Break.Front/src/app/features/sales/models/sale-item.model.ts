/**
 * Represents a single item within a sale transaction in the frontend.
 * Corresponds to the SaleItemResponse from the API.
 */
export interface SaleItem {
  saleItemId: number;
  itemId: number;
  productName?: string; // Optional, might be populated for display
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}