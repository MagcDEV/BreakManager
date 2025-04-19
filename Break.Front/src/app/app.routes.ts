// filepath: c:\Users\manuel.guevara\Documents\workspaces\BreakManager\Break.Front\src\app\app.routes.ts
import { Routes } from '@angular/router';
// Import the functional AuthGuard you will create later
// import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'auth', // Group auth routes under /auth path
    // Lazy load the auth feature's routes file
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'pos',
    loadComponent: () => import('./features/sales/components/pos/pos.component').then(m => m.PosComponent),
    // canActivate: [authGuard] // Add guard later
  },
  {
    path: 'products',
    // Example assuming products feature has its own routes file
    loadChildren: () => import('./features/products/products.routes').then(m => m.PRODUCT_ROUTES),
    // canActivate: [authGuard] // Add guard later
  },
/*   {
    path: 'sales',
    // Example assuming sales feature has its own routes file
    loadChildren: () => import('./features/sales/sales.routes').then(m => m.SALES_ROUTES),
    // canActivate: [authGuard] // Add guard later
  }, */
  // Default route: Redirect to login or a main page like POS
  // If using auth guard, you might redirect based on auth status
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },

  // Wildcard route for 404 Not Found (create a NotFoundComponent later)
  // { path: '**', loadComponent: () => import('./core/components/not-found/not-found.component').then(m => m.NotFoundComponent) }
];