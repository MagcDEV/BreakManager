/**
 * Represents the data needed to create a new sale item via the API.
 * Corresponds to SaleItemRequest in the API.
 */
export interface CreateSaleItem {
  itemId: number;
  quantity: number;
}

/**
 * Represents the data needed to create a new sale via the API.
 * Corresponds to CreateSaleRequest in the API.
 */
export interface CreateSale {
  items: CreateSaleItem[];
  couponCode?: string | null; // Allow null or undefined
}