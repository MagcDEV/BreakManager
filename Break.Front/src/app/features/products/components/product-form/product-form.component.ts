import { Component, OnInit, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgIf } from '@angular/common';
import { ProductService } from '../../../../core/services/product.service'; // Adjust the import path as necessary
import { Item } from '../../../../core/models/item.model';
import { CreateItem } from '../../../../core/models/create-item.model';
import { UpdateItem } from '../../../../core/models/update-item.model';
import { catchError, switchMap, tap, filter } from 'rxjs/operators';
import { EMPTY, Observable } from 'rxjs';
// Optional: Import a notification service for user feedback
// import { NotificationService } from '../../../../core/services/notification.service';

// Interface for the strongly-typed form structure
interface ProductForm {
  productCode: FormControl<string>;
  barcode: FormControl<string>;
  productName: FormControl<string>;
  productDescription: FormControl<string>;
  productCategory: FormControl<string>;
  reorderQuantity: FormControl<number>;
  unitPrice: FormControl<number>;
  quantityInStock: FormControl<number>;
  minimumStockLevel: FormControl<number>;
  maximumStockLevel: FormControl<number>;
}

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    ReactiveFormsModule, // Essential for reactive forms
    RouterLink
  ],
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly productService = inject(ProductService);
  // private readonly notificationService = inject(NotificationService); // Optional

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditMode = signal(false);
  private productId = signal<number | null>(null);

  // Strongly-typed form group
  productForm: FormGroup<ProductForm> = this.fb.nonNullable.group({
    productCode: ['', [Validators.required, Validators.maxLength(100)]],
    barcode: ['', [Validators.required, Validators.maxLength(100)]], // Assuming barcode is required
    productName: ['', [Validators.required, Validators.maxLength(100)]],
    productDescription: ['', [Validators.maxLength(2000)]],
    productCategory: ['', [Validators.required, Validators.maxLength(100)]],
    // Use Validators.min(0) for non-negative numbers
    unitPrice: [0, [Validators.required, Validators.min(0)]],
    quantityInStock: [0, [Validators.required, Validators.min(0), Validators.pattern('^[0-9]*$')]], // Integer validation
    reorderQuantity: [0, [Validators.required, Validators.min(0), Validators.pattern('^[0-9]*$')]],
    minimumStockLevel: [0, [Validators.required, Validators.min(0), Validators.pattern('^[0-9]*$')]],
    maximumStockLevel: [0, [Validators.required, Validators.min(0), Validators.pattern('^[0-9]*$')]],
  });

  ngOnInit(): void {
    this.route.paramMap.pipe(
      tap(params => {
        const id = params.get('id');
        if (id) {
          this.isEditMode.set(true);
          this.productId.set(+id);
          this.loadProductData(+id); // Load data for editing
        } else {
          this.isEditMode.set(false);
        }
      })
    ).subscribe(); // Subscribe to trigger the logic
  }

  loadProductData(id: number): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.productService.getItemById(id).pipe(
      tap(product => {
        // Patch the form with the fetched product data
        this.productForm.patchValue(product);
        this.isLoading.set(false);
      }),
      catchError((err: Error) => {
        this.errorMessage.set(`Failed to load product data: ${err.message}`);
        this.isLoading.set(false);
        // Optionally navigate back if product can't be loaded for edit
        // this.router.navigate(['/products']);
        return EMPTY;
      })
    ).subscribe();
  }

  onSubmit(): void {
    if (this.productForm.invalid) {
      this.errorMessage.set('Please correct the errors in the form.');
      this.productForm.markAllAsTouched(); // Show validation errors
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const formData = this.productForm.getRawValue();
    let saveObservable: Observable<Item>;

    if (this.isEditMode() && this.productId()) {
      // Update existing product (FR-PROD-05)
      const updateData: UpdateItem = formData; // Assuming UpdateItem matches the form structure
      saveObservable = this.productService.updateItem(this.productId()!, updateData);
    } else {
      // Create new product (FR-PROD-03)
      const createData: CreateItem = formData; // Assuming CreateItem matches the form structure
      saveObservable = this.productService.createItem(createData);
    }

    saveObservable.pipe(
      catchError((err: Error) => {
        this.errorMessage.set(`Failed to save product: ${err.message}`);
        this.isLoading.set(false);
        // this.notificationService.showError(`Failed to save product: ${err.message}`); // Optional
        return EMPTY;
      })
    ).subscribe((savedProduct) => {
      this.isLoading.set(false);
      // this.notificationService.showSuccess(`Product "${savedProduct.productName}" saved successfully!`); // Optional
      // Navigate to detail view after save
      this.router.navigate(['/products', savedProduct.itemId]);
    });
  }

  // Helper getters for template validation access
  get productCode() { return this.productForm.controls.productCode; }
  get barcode() { return this.productForm.controls.barcode; }
  get productName() { return this.productForm.controls.productName; }
  get productDescription() { return this.productForm.controls.productDescription; }
  get productCategory() { return this.productForm.controls.productCategory; }
  get unitPrice() { return this.productForm.controls.unitPrice; }
  get quantityInStock() { return this.productForm.controls.quantityInStock; }
  get reorderQuantity() { return this.productForm.controls.reorderQuantity; }
  get minimumStockLevel() { return this.productForm.controls.minimumStockLevel; }
  get maximumStockLevel() { return this.productForm.controls.maximumStockLevel; }
}