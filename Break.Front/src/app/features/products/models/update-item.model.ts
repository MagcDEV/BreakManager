/**
 * Represents the data required to update an existing product (Item).
 * Matches the backend Break.Contracts.Requests.UpdateItemRequest schema.
 * Note: The backend request includes DateAdded/LastUpdated, but these are usually
 * managed by the backend or handled differently in PUT requests.
 * We might only send fields the user can actually modify.
 */
export interface UpdateItem {
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
  // Consider if dateAdded/lastUpdated should be part of the update payload from the frontend
}