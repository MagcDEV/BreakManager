/**
 * Represents the data required to create a new product (Item).
 * Matches the backend Break.Contracts.Requests.CreateItemRequest schema.
 */
export interface CreateItem {
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
  // dateAdded and lastUpdated are typically set by the backend on creation
}