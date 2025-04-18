// filepath: c:\Users\manuel.guevara\Documents\workspaces\BreakManager\Break.Front\src\app\features\auth\auth.routes.ts
import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';

export const AUTH_ROUTES: Routes = [
  {
    path: 'login', // Full path will be /auth/login
    component: LoginComponent
  },
  {
    path: 'register', // Full path will be /auth/register
    component: RegisterComponent
  },
  {
    // Default route within the auth feature (e.g., if navigating just to /auth)
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];