import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { ProductFormComponent } from './components/product-form/product-form.component';
// Import guards if needed, e.g., to protect routes based on roles (Stock Manager, Admin)
// import { roleGuard } from '../../core/guards/role.guard'; // Example guard

export const PRODUCT_ROUTES: Routes = [
  {
    path: '', // Default route for '/products' -> Product List
    component: ProductListComponent,
    title: 'Product Management',
    // canActivate: [authGuard, roleGuard(['Stock Manager', 'Administrator'])] // Example protection
  },
  {
    path: 'new', // Route for '/products/new' -> Create Product Form
    component: ProductFormComponent,
    title: 'Add New Product',
    // canActivate: [authGuard, roleGuard(['Stock Manager', 'Administrator'])]
  },
  {
    path: ':id', // Route for '/products/:id' -> Product Details
    component: ProductDetailComponent,
    title: 'Product Details',
    // canActivate: [authGuard] // All authenticated users might view details
    // Consider adding a resolver here to pre-fetch product data
  },
  {
    path: ':id/edit', // Route for '/products/:id/edit' -> Edit Product Form
    component: ProductFormComponent,
    title: 'Edit Product',
    // canActivate: [authGuard, roleGuard(['Stock Manager', 'Administrator'])]
    // Consider adding a resolver here to pre-fetch product data for the form
  },
];