import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common'; // Import necessary pipes
import { RouterLink } from '@angular/router';
import { SaleService } from '../../services/sale.service';
import { Sale } from '../../models/sale.model'; // Assuming Sale model exists
import { Observable, EMPTY } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-sale-list',
  standalone: true,
  imports: [
    CommonModule, // Provides NgIf, NgFor etc.
    RouterLink,   // For linking to sale details
    CurrencyPipe, // To format currency
    DatePipe      // To format dates
  ],
  templateUrl: './sale-list.component.html',
  styleUrls: ['./sale-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush // OnPush for performance
})
export class SaleListComponent implements OnInit {
  private readonly saleService = inject(SaleService);

  // --- State Signals ---
  readonly isLoading = signal(true); // Start in loading state
  readonly errorMessage = signal<string | null>(null);
  readonly sales = signal<Sale[]>([]); // Signal to hold the list of sales

  ngOnInit(): void {
    this.loadSales();
  }

  /**
   * Fetches the list of sales from the service.
   * Implements FR-SALE-07.
   */
  loadSales(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.sales.set([]); // Clear previous sales

    this.saleService.getAllSales().pipe(
      tap((salesData) => {
        this.sales.set(salesData); // Update the signal with fetched data
        this.isLoading.set(false);
      }),
      catchError((err: Error) => {
        this.errorMessage.set(err.message || 'Failed to load sales history.');
        this.isLoading.set(false);
        return EMPTY; // Prevent breaking the stream
      })
    ).subscribe(); // Subscribe to trigger the fetch
  }

  /**
   * Required for @for trackBy to optimize rendering.
   */
  trackBySaleId(index: number, sale: Sale): number {
    return sale.saleId;
  }
}
