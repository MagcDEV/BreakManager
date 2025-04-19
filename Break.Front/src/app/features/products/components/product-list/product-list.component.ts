import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductService } from '../../../../core/services/product.service'; // Adjust the import path as necessary
import { Item } from '../../../../core/models/item.model'; // Adjust the import path as necessary
import { ProductListItemComponent } from '../product-list-item/product-list-item.component'; // Import the dumb component
import { Observable, EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    AsyncPipe, // Use AsyncPipe to handle observables in the template
    RouterLink, // For navigation links (e.g., Add New)
    ProductListItemComponent // Import the child component
  ],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush // Use OnPush for performance
})
export class ProductListComponent implements OnInit {
  private readonly productService = inject(ProductService);

  // Signal-based state management for loading and errors
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  // Observable stream for products
  products$: Observable<Item[]> = EMPTY; // Initialize with EMPTY

  // FR-PROD-02: Placeholder signals for search/filter/sort criteria
  readonly searchTerm = signal('');
  readonly filterCategory = signal('');
  readonly sortOrder = signal('productName'); // Default sort

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    // TODO: Pass sort/filter signals to service when API supports it
    // For now, just fetch all
    this.products$ = this.productService.getAllItems().pipe(
      catchError((err: Error) => {
        this.errorMessage.set(err.message || 'Failed to load products.');
        this.isLoading.set(false);
        return EMPTY; // Return an empty observable on error to prevent breaking the async pipe
      }),
      // Use tap or finalize eventually to set isLoading(false) on success/completion
      // For simplicity now, we rely on async pipe handling completion.
      // Consider adding finalize(() => this.isLoading.set(false)) if needed
    );
    // A simple way to turn off loading: subscribe briefly (not ideal for async pipe)
    // Or use a more complex pattern involving tap/finalize with the async pipe.
    // For now, we'll assume loading stops when data arrives or error occurs.
    // A better approach might involve mapping the stream to include loading state.
    this.products$.subscribe({
        complete: () => this.isLoading.set(false),
        error: () => this.isLoading.set(false) // Already handled in catchError, but good practice
    });
  }

  // FR-PROD-02: Methods to update search/filter/sort signals (will trigger reload if implemented)
  onSearchTermChange(term: string): void {
    this.searchTerm.set(term);
    // this.loadProducts(); // Reload when API supports search
  }

  onFilterChange(category: string): void {
    this.filterCategory.set(category);
    // this.loadProducts(); // Reload when API supports filtering
  }

  onSortChange(order: string): void {
    this.sortOrder.set(order);
    // this.loadProducts(); // Reload when API supports sorting
  }

  // FR-PROD-06: Handle deletion triggered from list item
  handleDeleteProduct(item: Item): void {
    // Example: Add confirmation dialog here using a shared service/component
    if (confirm(`Are you sure you want to delete "${item.productName}"?`)) {
      this.isLoading.set(true); // Indicate activity
      this.productService.deleteItem(item.itemId).subscribe({
        next: () => {
          this.isLoading.set(false);
          // Optionally show success notification
          this.loadProducts(); // Refresh the list
        },
        error: (err: Error) => {
          this.isLoading.set(false);
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