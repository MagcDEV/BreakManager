// filepath: src/app/features/sales/services/sale.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { Sale } from '../models/sale.model';
import { CreateSale } from '../models/create-sale.model';
// Consider adding a PaginatedResult type if GetAllSales needs pagination later
// import { PaginatedResult } from '../../../core/models/pagination.model';

/**
 * Service responsible for interacting with the Sale API endpoints.
 */
@Injectable({
  providedIn: 'root' // Singleton service
})
export class SaleService {
  private readonly http = inject(HttpClient);
  // Base URL for the sale API endpoint
  private readonly apiUrl = `${environment.apiUrl}/api/sale`;

  /**
   * Creates a new sale transaction.
   * Corresponds to FR-SALE-01.
   * @param saleData Data for the new sale.
   * @returns Observable containing the ID and Total of the created sale.
   */
  createSale(saleData: CreateSale): Observable<{ saleId: number; total: number }> {
    return this.http.post<{ saleId: number; total: number }>(this.apiUrl, saleData).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Retrieves a list of all sales.
   * Corresponds to FR-SALE-07.
   * TODO: Implement pagination, sorting, filtering based on API capabilities if needed.
   * @returns Observable array of Sale objects.
   */
  getAllSales(): Observable<Sale[]> {
    // Assuming the API returns SaleResponse[] which maps to Sale[]
    return this.http.get<Sale[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Retrieves details for a specific sale by its ID.
   * Corresponds to FR-SALE-08.
   * @param id The unique identifier of the sale.
   * @returns Observable Sale object.
   */
  getSaleById(id: number): Observable<Sale> {
    return this.http.get<Sale>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Confirms a sale transaction.
   * Corresponds to FR-SALE-05.
   * @param id The unique identifier of the sale to confirm.
   * @returns Observable of the confirmed Sale object.
   */
  confirmSale(id: number): Observable<Sale> {
    return this.http.post<Sale>(`${this.apiUrl}/${id}/confirm`, {}).pipe( // POST with empty body
      catchError(this.handleError)
    );
  }

  /**
   * Cancels a sale transaction.
   * Corresponds to FR-SALE-06.
   * @param id The unique identifier of the sale to cancel.
   * @returns Observable of the cancelled Sale object.
   */
  cancelSale(id: number): Observable<Sale> {
    return this.http.post<Sale>(`${this.apiUrl}/${id}/cancel`, {}).pipe( // POST with empty body
      catchError(this.handleError)
    );
  }

  // TODO: Add CalculateDiscount method if needed based on FR-SALE-03 note
  // calculateDiscount(items: CreateSaleItem[], couponCode?: string): Observable<number> { ... }

  /**
   * Basic error handler for HTTP requests.
   * Logs the error and returns a user-friendly message.
   * Consider enhancing this or using a global error interceptor.
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('API Error:', error);
    let errorMessage = 'An unknown error occurred during the sale operation!';
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code
      if (error.status === 404) {
        errorMessage = 'Sale not found.';
      } else if (error.status === 400 && error.error?.error) {
         // Handle specific backend errors like stock issues from ConfirmSale
         errorMessage = error.error.error;
      } else if (error.status === 400 && error.error?.errors) {
         // Handle validation errors
         try {
            const validationErrors = error.error.errors;
            errorMessage = Object.values(validationErrors).flat().join(' ');
         } catch (e) {
            errorMessage = `Server error: ${error.status}`;
         }
      } else {
        errorMessage = `Server returned code: ${error.status}, error message is: ${error.message}`;
      }
    }
    // Return an observable with a user-facing error message.
    return throwError(() => new Error(errorMessage));
  }
}