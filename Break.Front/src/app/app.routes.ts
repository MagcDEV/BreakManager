// filepath: c:\Users\manuel.guevara\Documents\workspaces\BreakManager\Break.Front\src\app\app.routes.ts
import { Routes } from '@angular/router';
// Import the functional AuthGuard
import { authGuard } from './core/guards/auth.guard'; // Correct path

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'pos',
    loadComponent: () => import('./features/sales/components/pos/pos.component').then(m => m.PosComponent),
    canActivate: [authGuard] // Apply the guard here
  },
  {
    path: 'products',
    loadChildren: () => import('./features/products/products.routes').then(m => m.PRODUCT_ROUTES),
    canActivate: [authGuard] // Apply the guard here
  },
  /*
  {
    path: 'sales',
    loadChildren: () => import('./features/sales/sales.routes').then(m => m.SALES_ROUTES),
    canActivate: [authGuard] // Apply the guard here if needed
  },
  */
  // Default route: Redirect to login or a main page like POS
  // The guard on '' might be complex depending on desired behavior.
  // Redirecting to login is often simplest if not authenticated.
  { path: '', redirectTo: '/pos', pathMatch: 'full' }, // Redirect logged-in users to POS by default? Or products?

  // Wildcard route for 404 Not Found (create a NotFoundComponent later)
  // { path: '**', loadComponent: () => import('./core/components/not-found/not-found.component').then(m => m.NotFoundComponent) }
];