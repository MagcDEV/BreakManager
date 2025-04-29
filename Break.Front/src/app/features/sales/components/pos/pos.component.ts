import { Component, ChangeDetectionStrategy, inject, signal, computed, DestroyRef } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { SaleService } from '../../services/sale.service';
// Import PaginationMetadata along with PaginatedResult and ProductService
import { ProductService, PaginatedResult } from '../../../products/services/product.service';
// Import PaginationMetadata from its correct location
import { PaginationMetadata } from '../../../products/models/pagination.model';
import { Item } from '../../../products/models/item.model';
import { CreateSale, CreateSaleItem } from '../../models/create-sale.model';
import { Sale } from '../../models/sale.model';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, switchMap, catchError, EMPTY, tap, Observable, of } from 'rxjs';

// Interface to hold cart item details including product info
interface CartItemDetail extends CreateSaleItem {
  productName: string;
  unitPrice: number;
  quantityInStock: number; // Keep track of stock for checks
  lineTotal: number;
}

// Define a default empty metadata object conforming to PaginationMetadata
// Ensure all properties from the interface are included
const defaultPaginationMetadata: PaginationMetadata = {
    currentPage: 1,
    pageSize: 0,
    totalCount: 0,
    totalPages: 0,
    hasPreviousPage: false, // Add missing property
    hasNextPage: false      // Add missing property
};

@Component({
  selector: 'app-pos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CurrencyPipe
  ],
  templateUrl: './pos.component.html',
  styleUrls: ['./pos.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PosComponent {
  private readonly saleService = inject(SaleService);
  private readonly productService = inject(ProductService);
  private readonly destroyRef = inject(DestroyRef); // For takeUntilDestroyed

  // --- Component State using Signals ---
  readonly couponCode = signal<string>('');
  readonly isLoading = signal(false); // Loading state for sale creation/confirmation
  readonly errorMessage = signal<string | null>(null);
  readonly lastCreatedSaleId = signal<number | null>(null);
  readonly searchIsLoading = signal(false);
  readonly searchError = signal<string | null>(null);

  // --- Product Search State ---
  private readonly searchTermSubject = new Subject<string>();
  readonly searchResults = signal<Item[]>([]);

  // --- Cart State ---
  readonly detailedCartItems = signal<CartItemDetail[]>([]);

  // --- Computed Signals for Totals (FR-SALE-03) ---
  readonly subTotal = computed(() => {
    return this.detailedCartItems().reduce((sum, item) => sum + item.lineTotal, 0);
  });

  // FR-SALE-04: Discount Calculation
  readonly discountAmount = computed(() => {
    const code = this.couponCode();
    const currentSubTotal = this.subTotal();
    if (code.toUpperCase() === 'SAVE10' && currentSubTotal > 0) {
        return currentSubTotal * 0.10;
    }
    return 0;
  });

  readonly total = computed(() => {
    const currentSubTotal = this.subTotal();
    const currentDiscount = this.discountAmount();
    const finalTotal = currentSubTotal - currentDiscount;
    return finalTotal < 0 ? 0 : finalTotal;
  });

  constructor() {
    // --- Product Search Logic (FR-SALE-02) ---
    this.searchTermSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      tap(() => {
        this.searchIsLoading.set(true);
        this.searchError.set(null);
        this.searchResults.set([]);
      }),
      switchMap(term => {
        if (!term || term.length < 2) {
          this.searchIsLoading.set(false);
          // Use the correctly defined defaultPaginationMetadata
          return of({ items: [], metadata: defaultPaginationMetadata } as PaginatedResult<Item>);
        }
        return this.productService.getAllItems(1, 10, term).pipe(
          tap(() => this.searchIsLoading.set(false)),
          catchError(err => {
            this.searchError.set(err.message || 'Failed to search products.');
            this.searchIsLoading.set(false);
            // Use the correctly defined defaultPaginationMetadata
            return of({ items: [], metadata: defaultPaginationMetadata } as PaginatedResult<Item>);
          })
        );
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(result => {
      // Ensure result and result.items are not null/undefined before setting
      this.searchResults.set(result?.items ?? []);
    });
  }

  // --- Methods ---

  // Trigger search when input changes
  onSearchTermChange(term: string): void {
    this.searchTermSubject.next(term);
  }

  // FR-SALE-02: Add item to sale
  addItemToSale(itemToAdd: Item, quantity: number = 1): void {
    // ... existing code ...
    if (quantity <= 0) {
      this.errorMessage.set('Quantity must be positive.');
      return;
    }

    // Check available stock from the search result item
    if (itemToAdd.quantityInStock < quantity) {
       this.errorMessage.set(`Not enough stock for ${itemToAdd.productName}. Available: ${itemToAdd.quantityInStock}`);
       return;
    }

    this.detailedCartItems.update(currentCart => {
      const existingItemIndex = currentCart.findIndex(i => i.itemId === itemToAdd.itemId);
      let updatedCart = [...currentCart];

      if (existingItemIndex > -1) {
        // Item exists, just update quantity
        const existingItem = updatedCart[existingItemIndex];
        const newQuantity = existingItem.quantity + quantity;

        // Check stock again for cumulative quantity
        if (existingItem.quantityInStock < newQuantity) {
           this.errorMessage.set(`Not enough stock for ${existingItem.productName}. Available: ${existingItem.quantityInStock}, In Cart: ${existingItem.quantity}`);
           return currentCart; // Return original cart if stock check fails
        }

        updatedCart[existingItemIndex] = {
          ...existingItem,
          quantity: newQuantity,
          lineTotal: existingItem.unitPrice * newQuantity // Recalculate line total
        };
      } else {
        // New item, add with details
        updatedCart.push({
          itemId: itemToAdd.itemId,
          quantity: quantity,
          productName: itemToAdd.productName,
          unitPrice: itemToAdd.unitPrice,
          quantityInStock: itemToAdd.quantityInStock, // Store stock info
          lineTotal: itemToAdd.unitPrice * quantity
        });
      }

      this.errorMessage.set(null); // Clear error on successful add/update
      this.searchResults.set([]); // Clear search results after adding
      // Optionally clear search input value here if needed
      return updatedCart; // Return the updated cart
    });
    // ... rest of the method
  }

  // FR-SALE-02: Remove item
  removeItemFromSale(itemId: number): void {
    this.detailedCartItems.update(items => items.filter(i => i.itemId !== itemId));
  }

  // FR-SALE-02: Update quantity
  updateItemQuantity(itemId: number, newQuantityInput: string | number): void {
    // ... existing code ...
    const newQuantity = Number(newQuantityInput);

    if (isNaN(newQuantity) || newQuantity < 0) {
      this.errorMessage.set('Invalid quantity entered.');
      // Consider resetting the input value visually if possible/needed
      return;
    }

    if (newQuantity === 0) {
      this.removeItemFromSale(itemId);
      return;
    }

    this.detailedCartItems.update(items => {
      const itemIndex = items.findIndex(i => i.itemId === itemId);
      if (itemIndex === -1) return items; // Item not found, should not happen

      const itemToUpdate = items[itemIndex];

      // Check stock using the stored quantityInStock
      if (itemToUpdate.quantityInStock < newQuantity) {
         this.errorMessage.set(`Not enough stock for ${itemToUpdate.productName}. Available: ${itemToUpdate.quantityInStock}`);
         // Optionally reset the input value to max stock here
         return items; // Return original items without updating quantity
      }

      // Create a new array with the updated item
      const updatedItems = [...items];
      updatedItems[itemIndex] = {
        ...itemToUpdate,
        quantity: newQuantity,
        lineTotal: itemToUpdate.unitPrice * newQuantity // Recalculate line total
      };
      this.errorMessage.set(null); // Clear error on successful update
      return updatedItems;
    });
    // ... rest of the method
  }

  // FR-SALE-01, FR-SALE-04: Submit sale
  submitSale(): void {
    // ... existing code ...
    const currentCart = this.detailedCartItems(); // Get current cart state
    if (currentCart.length === 0) {
      this.errorMessage.set('Cannot create an empty sale.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.lastCreatedSaleId.set(null);

    // Map detailed items back to the basic structure needed for the API
    const saleData: CreateSale = {
      items: currentCart.map(item => ({ itemId: item.itemId, quantity: item.quantity })),
      couponCode: this.couponCode() || null // Use null if couponCode is empty string
    };

    this.saleService.createSale(saleData).pipe(
        takeUntilDestroyed(this.destroyRef) // Auto-unsubscribe
    ).subscribe({
      next: (response) => {
        console.log('Sale created:', response);
        this.lastCreatedSaleId.set(response.saleId);
        this.resetSale(); // Clear the cart
        this.isLoading.set(false);
        // Optionally show success message via NotificationService
      },
      error: (err: Error) => {
        this.errorMessage.set(err.message);
        this.isLoading.set(false);
      }
    });
    // ... rest of the method
  }

  // FR-SALE-05: Confirm sale
  confirmSale(saleId: number): void {
    // ... existing code ...
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.saleService.confirmSale(saleId).pipe(
        takeUntilDestroyed(this.destroyRef) // Auto-unsubscribe
    ).subscribe({
       next: (confirmedSale) => {
          console.log('Sale confirmed:', confirmedSale);
          this.isLoading.set(false);
          this.lastCreatedSaleId.set(null); // Clear the confirmation message
          // Optionally show success message
       },
       error: (err: Error) => {
          // Display specific error from backend if available (e.g., stock issue on confirm)
          this.errorMessage.set(`Failed to confirm sale: ${err.message}`);
          this.isLoading.set(false);
       }
    });
    // ... rest of the method
  }

  // Reset sale state
  resetSale(): void {
    // ... existing code ...
    this.detailedCartItems.set([]); // Clear detailed items
    this.couponCode.set('');
    this.errorMessage.set(null);
    this.searchResults.set([]);
    // Clear search input visually if needed
    // this.lastCreatedSaleId.set(null); // Decide whether to clear confirmation ID
    // ... rest of the method
  }

  // Required for @for trackBy
  trackByItemId(index: number, item: Item | CartItemDetail): number {
    return item.itemId;
  }
}