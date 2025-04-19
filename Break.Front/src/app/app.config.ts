import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http'; // Import provideHttpClient and interceptor function

import { routes } from './app.routes';
// Import your interceptors when created
// import { authInterceptor } from './core/interceptors/auth.interceptor';
// import { errorInterceptor } from './core/interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    // Set up routing using the defined routes
    provideRouter(routes),

    // Set up HttpClient for making API requests
    provideHttpClient(
      withFetch()
      // Register interceptors here once created
      // withInterceptors([authInterceptor, errorInterceptor])
    ),

    // Other providers can be added here (e.g., for animations, state management)
  ]
};