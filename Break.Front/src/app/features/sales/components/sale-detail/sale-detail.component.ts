import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { SaleService } from '../../services/sale.service';
import { Sale } from '../../models/sale.model';
import { SaleItem } from '../../models/sale-item.model'; // Assuming SaleItem model exists
import { EMPTY } from 'rxjs';
import { switchMap, catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-sale-detail',
  standalone: true,
  imports: [
    CommonModule,
    CurrencyPipe,
    DatePipe
  ],
  templateUrl: './sale-detail.component.html',
  styleUrls: ['./sale-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SaleDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly saleService = inject(SaleService);

  // --- State Signals ---
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);
  readonly sale = signal<Sale | null>(null); // Signal for the detailed sale object

  ngOnInit(): void {
    this.loadSaleDetails();
  }

  /**
   * Loads sale details based on the ID from the route parameter.
   * Implements FR-SALE-08.
   */
  loadSaleDetails(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.sale.set(null);

    this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('id');
        if (!id) {
          this.errorMessage.set('Sale ID not found in route.');
          this.isLoading.set(false);
          return EMPTY;
        }
        return this.saleService.getSaleById(+id).pipe(
          tap((saleData) => {
            if (!saleData) {
              this.errorMessage.set('Sale not found.');
              this.isLoading.set(false);
            } else {
              this.sale.set(saleData); // Update signal with fetched data
              this.isLoading.set(false);
            }
          }),
          catchError((err: Error) => {
            this.errorMessage.set(err.message || 'Failed to load sale details.');
            this.isLoading.set(false);
            return EMPTY;
          })
        );
      })
    ).subscribe();
  }

  /**
   * Navigates back to the sales list view.
   */
  goBack(): void {
    this.router.navigate(['/sales']); // Adjust if your list route is different
  }

  /**
   * Required for @for trackBy on sale items.
   */
  trackBySaleItemId(index: number, item: SaleItem): number {
    // Use SaleItemId if available and unique, otherwise fallback to ItemId or index
    return item.saleItemId ?? item.itemId ?? index;
  }
}
