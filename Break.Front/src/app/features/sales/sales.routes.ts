import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
// Import the new components
import { SaleListComponent } from './components/sale-list/sale-list.component';
import { SaleDetailComponent } from './components/sale-detail/sale-detail.component';

export const SALES_ROUTES: Routes = [
  // FR-SALE-07: Route for sales list
  {
    path: '', // Corresponds to '/sales'
    component: SaleListComponent, // Use direct component reference for standalone
    canActivate: [authGuard],
    title: 'Sales History' // Optional: Set browser title
  },
  // FR-SALE-08: Route for sale details
  {
    path: ':id', // Corresponds to '/sales/:id'
    component: SaleDetailComponent, // Use direct component reference for standalone
    canActivate: [authGuard],
    title: 'Sale Details' // Optional: Set browser title
  },
  // Note: The main POS interface route ('/pos') is defined directly in app.routes.ts
  // for easier top-level access, rather than being nested under '/sales/pos'.
];