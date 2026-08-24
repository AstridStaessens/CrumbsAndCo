import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent {
  authService = inject(AuthService);

  email = '';
  isLoading = false;
  submitted = false;
  error = '';

  onSubmit(): void {
    this.isLoading = true;
    this.error = '';

    this.authService.forgotPassword({ email: this.email }).subscribe({
      next: () => {
        this.submitted = true;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Er ging iets mis. Probeer het later opnieuw.';
        this.isLoading = false;
      },
    });
  }
}
