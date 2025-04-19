import { Injectable, inject, signal, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
// Import HttpErrorResponse for more specific error handling
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
// Import map operator if needed for complex error parsing, otherwise just tap, catchError, throwError
import { Observable, tap, catchError, throwError } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { AuthResponse } from '../models/auth-response.model';
import { environment } from '../../../environments/environment';
// Import the RegisterUserRequest model
import { RegisterUserRequest } from '../models/login-request.model';

@Injectable({
  providedIn: 'root', // Provided in root for a singleton, tree-shakable service
})
export class AuthService {
  // Using inject() for dependency injection
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly tokenKey = 'authToken'; // Key for localStorage

  // Constructing URLs from environment variables
  private readonly loginUrl = `${environment.apiUrl}/api/auth/login`;
  // Define the registration endpoint URL based on openapi.json
  private readonly registerUrl = `${environment.apiUrl}/api/auth/register`;

  // Signal for authentication state, initialized safely for SSR/prerendering
  readonly isAuthenticated = signal<boolean>(this.checkInitialAuthStatus());

  /**
   * Checks the initial authentication status safely, avoiding localStorage access on the server.
   */
  private checkInitialAuthStatus(): boolean {
    // isPlatformBrowser ensures localStorage is only accessed in the browser
    if (isPlatformBrowser(this.platformId)) {
      return !!this.getToken(); // Check if token exists
    }
    return false; // Default to not authenticated on the server
  }

  /**
   * Attempts to log in the user.
   * @param credentials The user's login credentials.
   * @returns Observable<AuthResponse> containing the auth token.
   */
  login(credentials: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.loginUrl, credentials).pipe(
      tap((response) => {
        // Side effect: Store token and update auth state on successful login
        this.storeToken(response.token);
        this.isAuthenticated.set(true);
        // Navigate after successful login (only in browser)
        if (isPlatformBrowser(this.platformId)) {
            console.log('Login successful, navigating to dashboard');
            this.router.navigate(['/products']); // Or desired post-login route
        }
      }),
      catchError((error: HttpErrorResponse) => {
        // Handle login errors
        console.error('Login failed:', error);
        this.isAuthenticated.set(false); // Ensure state is false on error
        // Provide a user-friendly error message
        let errorMessage = 'Login failed. Please check your credentials.';
        if (error.status === 401 || error.status === 400) { // Unauthorized or Bad Request often mean bad credentials
            errorMessage = 'Invalid username or password.';
        }
        // Re-throw a new error for the component to handle
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  /**
   * Registers a new user.
   * @param registrationData The user's registration details matching RegisterUserRequest.
   * @returns Observable<void> indicating success (adjust if API returns data).
   */
  register(registrationData: RegisterUserRequest): Observable<void> {
    // Assuming the backend returns 2xx status with no body on success.
    // Change <void> if the backend returns user data or a token upon registration.
    return this.http.post<void>(this.registerUrl, registrationData).pipe(
      tap(() => {
        // Side effect: Log success and navigate to login page
        console.log('Registration successful');
        // Navigate only in the browser context
        if (isPlatformBrowser(this.platformId)) {
            // Navigate to login, optionally with a query param to show a success message
            this.router.navigate(['/auth/login'], { queryParams: { registered: 'true' } });
        }
      }),
      catchError((error: HttpErrorResponse) => {
        // Handle registration errors
        console.error('Registration failed:', error);
        let errorMessage = 'Registration failed. Please try again.';
        // Check for specific backend error messages or statuses
        if (error.status === 409) { // HTTP 409 Conflict often means username/email exists
            errorMessage = 'Username or email already exists.';
        } else if (error.error && typeof error.error === 'string') {
           // If the backend sends a plain text error message in the body
           errorMessage = error.error;
        } else if (error.error?.errors && typeof error.error.errors === 'object') {
            // Handle ASP.NET Core validation problem details
             try {
                errorMessage = Object.values(error.error.errors).flat().join(' ');
             } catch (e) { /* Fallback if parsing fails */ }
        }
        // Re-throw a new error for the component's error handler
        return throwError(() => new Error(errorMessage));
      })
    );
  }


  /**
   * Logs out the current user.
   */
  logout(): void {
    this.removeToken(); // Remove token from storage
    this.isAuthenticated.set(false); // Update auth state
    // Ensure navigation only happens in the browser
    if (isPlatformBrowser(this.platformId)) {
        this.router.navigate(['/auth/login']); // Redirect to login
    }
  }

  /**
   * Retrieves the stored authentication token safely (browser only).
   * @returns The token string or null.
   */
  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.tokenKey);
    }
    return null; // Return null if not in browser
  }

  /**
   * Stores the authentication token in localStorage safely (browser only).
   * @param token The JWT token string.
   */
  private storeToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.tokenKey, token);
    }
  }

  /**
   * Removes the authentication token from localStorage safely (browser only).
   */
  private removeToken(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.tokenKey);
    }
  }
}