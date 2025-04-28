import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from './core/services/auth.service'; // Import AuthService
import { CommonModule } from '@angular/common'; // Import CommonModule for NgIf/NgFor etc. (or just NgIf)

@Component({
  selector: 'app-root',
  standalone: true,
  // Import RouterLink, RouterLinkActive for navigation links
  // Import CommonModule or just NgIf for conditional rendering
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'] // Corrected property name
})
export class AppComponent {
  // Inject AuthService to access authentication state
  private readonly authService = inject(AuthService);

  // Expose the signal directly to the template
  readonly isAuthenticated = this.authService.isAuthenticated;

  readonly title = 'Break Manager'; // Example title
  readonly currentYear = new Date().getFullYear();

  /**
   * Logs the user out by calling the AuthService.
   */
  logout(): void {
    this.authService.logout();
  }
}