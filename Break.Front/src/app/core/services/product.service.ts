import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams, HttpResponse } from '@angular/common/http'; // Import HttpResponse
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Item } from '../models/item.model';
import { CreateItem } from '../models/create-item.model';
import { UpdateItem } from '../models/update-item.model';
import { PaginationMetadata } from '../models/pagination.model'; // Import the new interface

// Define a type for the combined result
export interface PaginatedResult<T> {
  items: T[];
  metadata: PaginationMetadata;
}

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
   * @param sort Optional sorting parameters.
   * @param filter Optional filtering parameters.
   * @returns Observable of PaginatedResult containing items and metadata.
   */
  getAllItems(
    pageNumber: number = 1,
    pageSize: number = 10,
    sort?: string,
    filter?: string
  ): Observable<PaginatedResult<Item>> { // Update return type
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (sort) {
      params = params.set('sort', sort);
    }
    if (filter) {
      params = params.set('filter', filter);
    }

    // Request the full HttpResponse to access headers
    return this.http.get<Item[]>(this.apiUrl, { params, observe: 'response' }).pipe(
      map((response: HttpResponse<Item[]>) => {
        // Extract pagination header
        const paginationHeader = response.headers.get('X-Pagination');
        let metadata: PaginationMetadata | null = null;

        if (paginationHeader) {
          try {
            metadata = JSON.parse(paginationHeader);
          } catch (e) {
            console.error('Could not parse X-Pagination header:', e);
            // Provide default metadata or handle error appropriately
            metadata = { currentPage: 1, pageSize: pageSize, totalCount: 0, totalPages: 0, hasPreviousPage: false, hasNextPage: false };
          }
        } else {
           console.warn('X-Pagination header not found.');
           // Provide default metadata if header is missing
           metadata = { currentPage: 1, pageSize: pageSize, totalCount: response.body?.length ?? 0, totalPages: 1, hasPreviousPage: false, hasNextPage: false };
        }

        // Ensure metadata is not null before returning
        if (!metadata) {
             throw new Error("Pagination metadata could not be determined.");
        }


        // Return the combined result: items from body, metadata from header
        return {
          items: response.body || [], // Use empty array if body is null
          metadata: metadata
        };
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
   * Basic error handler for HTTP requests.
   * Logs the error and returns a user-friendly message.
   */
  private handleError(error: HttpErrorResponse) {
    console.error('API Error:', error);
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code
      // Try to extract a meaningful message
      if (error.status === 404) {
        errorMessage = 'Product not found.';
      } else if (error.status === 400 && error.error) {
         // Handle validation errors or specific messages from backend
         try {
            // Attempt to parse ASP.NET Core validation problem details
            const errors = error.error.errors || { message: [error.error.detail || error.error] };
            errorMessage = Object.values(errors).flat().join(' ');
         } catch (e) {
            errorMessage = typeof error.error === 'string' ? error.error : `Server error: ${error.status}`;
         }
      } else {
        errorMessage = `Server returned code: ${error.status}, error message is: ${error.message}`;
      }
    }
    return throwError(() => new Error(errorMessage));
  }
}