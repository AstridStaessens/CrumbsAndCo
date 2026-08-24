import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { OrderService, OrderResponse } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [CurrencyPipe, DatePipe],
  templateUrl: './account.component.html',
  styleUrl: './account.component.scss',
})
export class AccountComponent implements OnInit {
  orderService = inject(OrderService);
  authService = inject(AuthService);

  orders: OrderResponse[] = [];
  isLoading = true;
  cancellingOrderId: number | null = null;
  error = '';

  /**
   * Statussen waarin een klant zijn eigen bestelling nog kan annuleren.
   * Moet overeenkomen met CancellableStatuses in OrderService (backend).
   */
  private readonly cancellableStatuses = ['new', 'pending_payment'];

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.isLoading = true;
    this.orderService.getMyOrders().subscribe({
      next: (orders) => {
        this.orders = orders.sort((a, b) =>
          new Date(b.date).getTime() - new Date(a.date).getTime()
        );
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  canCancel(order: OrderResponse): boolean {
    return this.cancellableStatuses.includes(order.status);
  }

  cancelOrder(order: OrderResponse): void {
    if (!confirm(`Weet je zeker dat je bestelling #${order.id} wil annuleren?`)) {
      return;
    }

    this.error = '';
    this.cancellingOrderId = order.id;

    this.orderService.cancelOrder(order.id).subscribe({
      next: () => {
        this.cancellingOrderId = null;
        this.loadOrders();
      },
      error: (err) => {
        this.cancellingOrderId = null;
        this.error = err?.error?.message ?? 'Bestelling kon niet geannuleerd worden.';
      }
    });
  }

  statusLabel(status: string): string {
    const labels: Record<string, string> = {
      'new': 'Nieuw',
      'pending_payment': 'Wacht op betaling',
      'paid': 'Betaald',
      'in_production': 'In productie',
      'ready': 'Klaar voor afhaling',
      'completed': 'Afgerond',
      'cancelled': 'Geannuleerd',
      'refunded': 'Terugbetaald'
    };
    return labels[status] ?? status;
  }
}
