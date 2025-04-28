import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map, take } from 'rxjs/operators'; // Import map and take if using observable approach

/**
 * Functional route guard to check if the user is authenticated.
 * Redirects to the login page if the user is not authenticated.
 *
 * @returns A boolean indicating if the route can be activated,
 *          or a UrlTree to redirect the user.
 */
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Check the authentication status using the signal
  if (authService.isAuthenticated()) {
    return true; // User is authenticated, allow access
  } else {
    // User is not authenticated, redirect to login page
    console.log('AuthGuard: User not authenticated, redirecting to login.');
    // Create a UrlTree to redirect
    return router.createUrlTree(['/auth/login'], { queryParams: { returnUrl: state.url } });
    // Returning a UrlTree automatically cancels the current navigation and redirects.
    // We pass the attempted URL (`state.url`) as a query parameter
    // so the login component can redirect back after successful login.
  }

  /*
  // --- Alternative using Observable (if isAuthenticated were an Observable) ---
  // return authService.isAuthenticated$.pipe( // Assuming isAuthenticated$ is an Observable<boolean>
  //   take(1), // Take the first emission and complete
  //   map(isAuthenticated => {
  //     if (isAuthenticated) {
  //       return true;
  //     } else {
  //       console.log('AuthGuard: User not authenticated, redirecting to login.');
  //       return router.createUrlTree(['/auth/login'], { queryParams: { returnUrl: state.url } });
  //     }
  //   })
  // );
  */
};