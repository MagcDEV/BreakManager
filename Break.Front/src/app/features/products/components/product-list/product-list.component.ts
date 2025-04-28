import { Component, OnInit, inject, signal, ChangeDetectionStrategy, computed } from '@angular/core'; // Import computed
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductService, PaginatedResult } from '../../../../core/services/product.service'; // Adjust path, import PaginatedResult
import { Item } from '../../../../core/models/item.model';
import { PaginationMetadata } from '../../../../core/models/pagination.model'; // Import PaginationMetadata
import { ProductListItemComponent } from '../product-list-item/product-list-item.component';
import { Observable, EMPTY, Subject, switchMap, tap, startWith } from 'rxjs'; // Import Subject, switchMap, tap, startWith
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    AsyncPipe,
    RouterLink,
    ProductListItemComponent
  ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductListComponent implements OnInit {
  private readonly productService = inject(ProductService);

  // --- State Signals ---
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly currentPage = signal(1);
  readonly pageSize = signal(10); // Default page size
  readonly paginationMetadata = signal<PaginationMetadata | null>(null);

  // --- Signals for triggering data refresh ---
  private readonly refreshTrigger = new Subject<void>();

  // --- Derived Observable for Products ---
  // Use switchMap to react to refresh triggers (including page changes)
  readonly productsResult$: Observable<PaginatedResult<Item>> = this.refreshTrigger.pipe(
    startWith(undefined), // Trigger initial load
    tap(() => { // Set loading state when starting fetch
      this.isLoading.set(true);
      this.errorMessage.set(null);
    }),
    switchMap(() => this.productService.getAllItems(this.currentPage(), this.pageSize()) // Pass current page/size
      .pipe(
        tap((result) => { // Update metadata and loading state on success
          this.paginationMetadata.set(result.metadata);
          this.isLoading.set(false);
        }),
        catchError((err: Error) => { // Handle errors
          this.errorMessage.set(err.message || 'Failed to load products.');
          this.paginationMetadata.set(null); // Clear metadata on error
          this.isLoading.set(false);
          return EMPTY; // Return empty observable to prevent breaking the stream
        })
      )
    )
  );

  // Computed signal for easier access in template (optional)
  readonly hasPreviousPage = computed(() => this.paginationMetadata()?.hasPreviousPage ?? false);
  readonly hasNextPage = computed(() => this.paginationMetadata()?.hasNextPage ?? false);

  ngOnInit(): void {
    // Initial load is handled by startWith in the observable pipeline
    // this.loadProducts(); // No longer needed here
  }

  // --- Pagination Methods ---
  goToPreviousPage(): void {
    if (this.hasPreviousPage()) {
      this.currentPage.update(page => page - 1);
      this.triggerRefresh();
    }
  }

  goToNextPage(): void {
    if (this.hasNextPage()) {
      this.currentPage.update(page => page + 1);
      this.triggerRefresh();
    }
  }

  // Method to trigger a data refresh
  private triggerRefresh(): void {
    this.refreshTrigger.next();
  }

  // --- Other Methods ---
  // FR-PROD-02: Methods to update search/filter/sort signals (will trigger reload if implemented)
  onSearchTermChange(term: string): void {
    // TODO: Implement search logic (update a search signal and triggerRefresh)
    console.log('Search:', term);
    // this.searchTerm.set(term);
    // this.currentPage.set(1); // Reset to first page on search
    // this.triggerRefresh();
  }

  onFilterChange(category: string): void {
    // TODO: Implement filter logic
    console.log('Filter:', category);
    // this.filterCategory.set(category);
    // this.currentPage.set(1); // Reset to first page on filter
    // this.triggerRefresh();
  }

  onSortChange(order: string): void {
    // TODO: Implement sort logic
    console.log('Sort:', order);
    // this.sortOrder.set(order);
    // this.currentPage.set(1); // Reset to first page on sort
    // this.triggerRefresh();
  }

  // FR-PROD-06: Handle deletion triggered from list item
  handleDeleteProduct(item: Item): void {
    if (confirm(`Are you sure you want to delete "${item.productName}"?`)) {
      this.isLoading.set(true); // Indicate loading during delete
      this.productService.deleteItem(item.itemId).subscribe({
        next: () => {
          this.errorMessage.set(null); // Clear any previous error
          // Optionally show success notification
          // Refresh the current page after deletion
          // Consider edge case: deleting last item on a page > 1
          if (this.paginationMetadata()?.totalCount && this.paginationMetadata()!.totalCount % this.pageSize() === 1 && this.currentPage() > 1) {
             this.currentPage.update(p => p - 1); // Go to previous page if last item deleted
          }
          this.triggerRefresh();
        },
        error: (err: Error) => {
          this.isLoading.set(false); // Stop loading on error
          this.errorMessage.set(`Failed to delete product: ${err.message}`);
          // Optionally show error notification
        }
      });
    }
  }

  // Required for *ngFor trackBy
  trackByProductId(index: number, item: Item): number {
    return item.itemId;
  }
}