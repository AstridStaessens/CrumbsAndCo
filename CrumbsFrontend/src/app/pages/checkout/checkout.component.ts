import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { PaymentService } from '../../services/payment.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CurrencyPipe, AsyncPipe, RouterLink],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit {
  cartService = inject(CartService);
  orderService = inject(OrderService);
  paymentService = inject(PaymentService);
  authService = inject(AuthService);
  router = inject(Router);

  isLoading = false;
  error = '';

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
    }
    if (this.cartService.items.length === 0) {
      this.router.navigate(['/shop']);
    }
  }

  placeOrder(): void {
    this.isLoading = true;
    this.error = '';

    const orderLines = this.cartService.items.map((item) => ({
      productId: item.id,
      quantity: item.quantity,
    }));

    this.orderService.createOrder({ orderLines }).subscribe({
      next: (response) => {
        this.paymentService.createPayment(response.id).subscribe({
          next: (payment) => {
            this.cartService.clearCart();
            window.location.href = payment.paymentUrl;
          },
          error: (err) => {
            this.error = err?.error?.message ?? 'Betaling kon niet worden aangemaakt.';
            this.isLoading = false;
          }
        });
      },
      error: (err) => {
        // De backend geeft bij onvoldoende voorraad of een ongeldig product
        // een duidelijke foutmelding terug (via ErrorResponseContract.message).
        this.error = err?.error?.message ?? 'Bestelling kon niet worden geplaatst.';
        this.isLoading = false;
      }
    });
  }
}
