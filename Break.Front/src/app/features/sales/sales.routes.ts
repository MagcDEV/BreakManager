import { Routes } from '@angular/router';

// Import the components that will be routed to within this feature
// (Assume these components will be created later)
/* import { SaleListComponent } from './components/sale-list/sale-list.component';
import { SaleDetailComponent } from './components/sale-detail/sale-detail.component'; */

// Import necessary guards
// import { authGuard } from '../../core/guards/auth.guard';

export const SALES_ROUTES: Routes = [
/*   {
    path: '', // Default path for the sales feature (/sales) - shows sales history list
    component: SaleListComponent,
    title: 'Sales History',
    // canActivate: [authGuard], // Protect this route
  },
  {
    path: ':id', // Route for viewing details of a specific past sale (/sales/456)
    component: SaleDetailComponent,
    title: 'Sale Details',
    // canActivate: [authGuard],
     // You might add a resolver here later to pre-fetch sale data
  },
  // Note: The main POS interface route ('/pos') is likely defined directly in app.routes.ts */
  // for easier top-level access, rather than being nested under '/sales/pos'.
];