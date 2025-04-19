import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Item } from '../models/item.model';
import { CreateItem } from '../models/create-item.model';
import { UpdateItem } from '../models/update-item.model';

@Injectable({
  providedIn: 'root', // Singleton service available application-wide
})
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/api/item`; // Base URL for item endpoints

  /**
   * Fetches a list of all products.
   * FR-PROD-01
   * @param sort Optional sorting parameters
   * @param filter Optional filtering parameters
   * @returns Observable array of Items.
   */
  getAllItems(sort?: string, filter?: string): Observable<Item[]> {
    // Basic example: Add query params if needed (API support required)
    let params = new HttpParams();
    if (sort) {
      params = params.set('sort', sort);
    }
    if (filter) {
      params = params.set('filter', filter);
    }

    return this.http.get<Item[]>(this.apiUrl, { params }).pipe(
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