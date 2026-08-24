import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss',
})
export class ResetPasswordComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  authService = inject(AuthService);

  email = '';
  token = '';
  newPassword = '';
  confirmPassword = '';

  isLoading = false;
  submitted = false;
  error = '';
  linkMissing = false;

  showPassword = false;
  showConfirmPassword = false;

  togglePasswordVisibility(field: 'new' | 'confirm'): void {
    if (field === 'new') {
      this.showPassword = !this.showPassword;
    } else {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  ngOnInit(): void {
    const queryParams = this.route.snapshot.queryParamMap;
    this.email = queryParams.get('email') ?? '';
    this.token = queryParams.get('token') ?? '';

    if (!this.email || !this.token) {
      this.linkMissing = true;
    }
  }

  onSubmit(): void {
    this.error = '';

    if (this.newPassword !== this.confirmPassword) {
      this.error = 'De wachtwoorden komen niet overeen.';
      return;
    }

    this.isLoading = true;

    this.authService.resetPassword({
      email: this.email,
      token: this.token,
      newPassword: this.newPassword,
    }).subscribe({
      next: () => {
        this.submitted = true;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'De link is ongeldig of verlopen. Vraag een nieuwe resetlink aan.';
        this.isLoading = false;
      },
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}
