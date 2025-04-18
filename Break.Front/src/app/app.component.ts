import { Component } from '@angular/core';
// Import Router directives
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
// Import CommonModule if you use NgIf/NgFor etc. in app.component.html, otherwise not needed for just links
// import { CommonModule } from '@angular/common';
import { AuthService } from './core/services/auth.service'; // Assuming you'll use this later for conditional links
import { inject } from '@angular/core'; // If using inject

@Component({
  selector: 'app-root',
  standalone: true,
  // Add RouterLink and RouterLinkActive here
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
    // CommonModule // Add if needed for *ngIf/*ngFor etc.
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'BreakManager';
  currentYear = new Date().getFullYear();
  // Inject AuthService if you plan to show/hide links based on auth state
  // authService = inject(AuthService);
}