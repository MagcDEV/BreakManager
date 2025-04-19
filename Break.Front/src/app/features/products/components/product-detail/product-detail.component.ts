import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AsyncPipe, CurrencyPipe, DatePipe, NgIf } from '@angular/common';
import { ProductService } from '../../../../core/services/product.service'; // Adjust the import path as necessary
import { Item } from '../../../../core/models/item.model'; // Adjust the import path as necessary
import { Observable, EMPTY } from 'rxjs';
import { switchMap, catchError, tap } from 'rxjs/operators';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [
    AsyncPipe,
    DatePipe, // To format dates
    CurrencyPipe, // To format currency
    RouterLink // For back/edit links
  ],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly productService = inject(ProductService);

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  product$: Observable<Item | null> = EMPTY; // Initialize with EMPTY

  ngOnInit(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.product$ = this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('id');
        if (!id) {
          this.errorMessage.set('Product ID not found in route.');
          this.isLoading.set(false);
          // Optionally navigate back or to an error page
          // this.router.navigate(['/products']);
          return EMPTY;
        }
        // Fetch product details based on the ID from the route
        // FR-PROD-04
        return this.productService.getItemById(+id).pipe(
          tap(() => this.isLoading.set(false)), // Stop loading on success
          catchError((err: Error) => {
            this.errorMessage.set(err.message || 'Failed to load product details.');
            this.isLoading.set(false);
            return EMPTY; // Return empty observable on error
          })
        );
      })
    );
  }

  goBack(): void {
    this.router.navigate(['/products']); // Navigate back to the product list
  }

  // Optional: Delete action directly from detail view
  deleteProduct(item: Item): void {
     if (confirm(`Are you sure you want to delete "${item.productName}"?`)) {
      this.isLoading.set(true);
      this.productService.deleteItem(item.itemId).subscribe({
        next: () => {
          // Optionally show success notification
          this.router.navigate(['/products']); // Navigate back to list after delete
        },
        error: (err: Error) => {
          this.isLoading.set(false);
          this.errorMessage.set(`Failed to delete product: ${err.message}`);
          // Optionally show error notification
        }
      });
    }
  }
}