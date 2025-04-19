/**
 * Represents the detailed information of a product (Item).
 * Matches the backend Break.Contracts.Responses.ItemResponse schema.
 */
export interface Item {
  itemId: number;
  productCode: string;
  barcode: string;
  productName: string;
  productDescription: string;
  productCategory: string;
  reorderQuantity: number;
  unitPrice: number;
  quantityInStock: number;
  minimumStockLevel: number;
  maximumStockLevel: number;
  dateAdded: string; // Use string for ISO date format from JSON
  lastUpdated: string; // Use string for ISO date format from JSON
}