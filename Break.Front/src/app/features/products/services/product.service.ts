import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams, HttpResponse } from '@angular/common/http'; // Import HttpResponse
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Item } from '../../../features/products/models/item.model';
import { CreateItem } from '../../../features/products/models/create-item.model';
import { UpdateItem } from '../../../features/products/models/update-item.model';
import { PaginationMetadata } from '../models/pagination.model'; // Import the new interface

// Define a type for the combined result
export interface PaginatedResult<T> {
  items: T[];
  metadata: PaginationMetadata;
}

// Define a default empty metadata object conforming to PaginationMetadata
const defaultPaginationMetadata: PaginationMetadata = {
    currentPage: 1, pageSize: 0, totalCount: 0, totalPages: 0, hasPreviousPage: false, hasNextPage: false
};

@Injectable({
  providedIn: 'root', // Singleton service available application-wide
})
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/item`; // Base URL for item endpoints

  /**
   * Fetches a paginated list of products.
   * FR-PROD-01
   * @param pageNumber The page number to retrieve (1-based).
   * @param pageSize The number of items per page.
   * @param searchTerm Optional filtering parameters (used for name/barcode search).
   * @returns Observable of PaginatedResult containing items and metadata.
   */
  getAllItems(
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm?: string // Use searchTerm for filtering
  ): Observable<PaginatedResult<Item>> { // Update return type
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    // Use searchTerm for filtering if provided
    if (searchTerm) {
      // Assuming the backend API uses a 'filter' query parameter for searching
      params = params.set('filter', searchTerm);
    }

    // Request the full HttpResponse to access headers
    return this.http.get<Item[]>(this.apiUrl, { params, observe: 'response' }).pipe(
      map((response: HttpResponse<Item[]>) => {
        // Extract pagination metadata from headers
        const paginationHeader = response.headers.get('X-Pagination');
        let metadata: PaginationMetadata = { ...defaultPaginationMetadata }; // Start with default

        if (paginationHeader) {
          try {
            metadata = JSON.parse(paginationHeader);
          } catch (e) {
            console.error('Could not parse X-Pagination header:', e);
            // Keep default metadata on parse error
          }
        } else {
          console.warn('X-Pagination header not found. Using default metadata.');
          // Adjust default metadata based on response if header is missing
          const itemCount = response.body?.length ?? 0;
          metadata = {
            ...defaultPaginationMetadata, // Spread defaults first
            currentPage: 1,
            pageSize: itemCount, // Assume page size is the number of items returned
            totalCount: itemCount, // Assume total count is the number returned if no header
            totalPages: 1, // Assume only one page if no header
          };
        }
        // Ensure body is not null, default to empty array if it is
        const items = response.body ?? [];
        return { items, metadata };
      }),
      catchError(this.handleError) // Centralized error handling
    );
  }

  /**
   * Fetches details for a single product by its ID.
   * FR-PROD-04
   * @param id The unique identifier of the product.
   * @returns Observable Item.
   */
  getItemById(id: number): Observable<Item> {
    return this.http.get<Item>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Creates a new product.
   * FR-PROD-03
   * @param itemData Data for the new product.
   * @returns Observable of the created Item (assuming API returns the created object).
   */
  createItem(itemData: CreateItem): Observable<Item> {
    // Adjust the return type <Item> if the API returns something else (e.g., void or just the ID)
    return this.http.post<Item>(this.apiUrl, itemData).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Updates an existing product.
   * FR-PROD-05
   * @param id The ID of the product to update.
   * @param itemData Updated data for the product.
   * @returns Observable of the updated Item (assuming API returns the updated object).
   */
  updateItem(id: number, itemData: UpdateItem): Observable<Item> {
    // Adjust the return type <Item> if the API returns something else (e.g., void)
    return this.http.put<Item>(`${this.apiUrl}/${id}`, itemData).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Deletes a product.
   * FR-PROD-06
   * @param id The ID of the product to delete.
   * @returns Observable<void> indicating success.
   */
  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Fetches a single product by its barcode.
   * @param barcode The barcode string to search for.
   * @returns Observable of the found Item or null/error if not found.
   */
  getItemByBarcode(barcode: string): Observable<Item> {
    // Ensure barcode is properly URL-encoded if it contains special characters
    const encodedBarcode = encodeURIComponent(barcode);
    return this.http.get<Item>(`${this.apiUrl}/barcode/${encodedBarcode}`).pipe(
      catchError(this.handleError) // Reuse existing error handler
    );
  }
  /**
   * Centralized error handler for HTTP requests.
   * Logs the error and returns an Observable that emits a user-friendly error message.
   * @param error The HttpErrorResponse received.
   * @returns Observable<never>
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('API Error:', error);
    let errorMessage = 'An unknown error occurred!';

    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Network error: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code
      if (error.status === 404) {
        // Use a more generic message for 404 as this service might fetch different resources
        errorMessage = 'Resource not found.';
      } else if (error.status === 400 && error.error) {
         // Handle validation errors or specific messages from backend
         try {
            // Attempt to parse ASP.NET Core validation problem details
            const validationErrors = error.error.errors; // ASP.NET Core validation format
            if (validationErrors) {
              // Flatten validation messages
              errorMessage = Object.values(validationErrors).flat().join(' ');
            } else {
              // Try to get a general error title or detail
              errorMessage = error.error.detail || error.error.title || (typeof error.error === 'string' ? error.error : `Bad Request`);
            }
         } catch (e) {
            errorMessage = typeof error.error === 'string' ? error.error : `Bad Request`;
         }
      } else if (error.status === 0) {
        // Likely a CORS issue or network connectivity problem
        errorMessage = 'Cannot connect to the server. Please check your network connection or CORS configuration.';
      }
       else {
        // Generic server error message
        const backendError = error.error?.message || error.error?.title || error.message;
        errorMessage = `Server error (${error.status}): ${backendError}`;
      }
    }
    // Return an observable that emits the error message wrapped in an Error object
    return throwError(() => new Error(errorMessage));
  }
}