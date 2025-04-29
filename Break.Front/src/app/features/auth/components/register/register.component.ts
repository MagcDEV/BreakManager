// filepath: c:\Users\manuel.guevara\Documents\workspaces\BreakManager\Break.Front\src\app\features\auth\components\register\register.component.ts
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators, ValidationErrors, AbstractControl } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // Import RouterLink
import { AuthService } from '../../../../core/services/auth.service';
import { RegisterUserRequest } from '../../models/login-request.model';

// Custom validator for matching passwords
// Placed outside the component class for better reusability and testability
export function passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    // Don't validate if controls aren't present or haven't been touched yet
    if (!password || !confirmPassword || !password.value || !confirmPassword.value) {
        return null;
    }

    return password.value === confirmPassword.value ? null : { passwordsMismatch: true };
}

// Define an interface for the registration form structure for strong typing
interface RegisterForm {
  username: FormControl<string>;
  email: FormControl<string>;
  password: FormControl<string>;
  confirmPassword: FormControl<string>;
}

@Component({
  selector: 'app-register',
  standalone: true, // Using Standalone Component API as per instructions
  imports: [
    ReactiveFormsModule, // Needed for reactive forms
    RouterLink // Needed for routerLink directive (e.g., link back to login)
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush // Using OnPush for performance optimization
})
export class RegisterComponent {
  // Using inject() for dependency injection (modern approach)
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // Using signals for managing component state (modern approach)
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly successMessage = signal<string | null>(null); // Optional: for success feedback

  // Strongly typed reactive form group using FormBuilder and FormControl<T>
  readonly registerForm: FormGroup<RegisterForm> = this.fb.group({
    // Using nonNullable: true for required fields
    username: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3)] }),
    email: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.email] }),
    password: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(6)] }),
    confirmPassword: new FormControl('', { nonNullable: true, validators: [Validators.required] })
  }, { validators: passwordsMatchValidator }); // Add group-level validator

  /**
   * Handles the form submission for registration.
   * Kept concise as per instructions (< 75 lines).
   */
  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.errorMessage.set('Please correct the errors in the form.');
      this.registerForm.markAllAsTouched(); // Mark all fields to show validation errors visually
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.successMessage.set(null);

    const { username, email, password } = this.registerForm.getRawValue();
    // Construct the request payload according to the RegisterUserRequest interface
    const registrationData: RegisterUserRequest = {
        username,
        email,
        password,
        roles: [] // Sending empty array for roles based on openapi.json and self-registration context
    };

    // Subscribe to the service method
    this.authService.register(registrationData).subscribe({
      next: () => {
        // Success navigation is handled within the AuthService's register method's tap operator.
        // Optionally, set a success message signal here if needed before navigation.
        // this.successMessage.set('Registration successful! Redirecting...');
      },
      error: (err: Error) => {
        // Update error message signal for display in the template
        this.errorMessage.set(err.message || 'An unknown registration error occurred.');
        this.isLoading.set(false); // Ensure loading state is turned off on error
      },
      complete: () => {
        // Ensure loading state is turned off when the observable completes (even if error occurred)
        this.isLoading.set(false);
      }
    });
  }
}