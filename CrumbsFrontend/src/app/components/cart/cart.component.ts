import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { CartItem } from '../../models/product.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [AsyncPipe, CurrencyPipe],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent {
  @Input() isOpen = false;
  @Output() close = new EventEmitter<void>();

  cartService = inject(CartService);
  router = inject(Router);
  items$ = this.cartService.items$;
  subtotal$ = this.cartService.subtotal$;

  updateQuantity(item: CartItem, delta: number): void {
    const newQty = item.quantity + delta;
    if (newQty < 1) {
      this.cartService.removeItem(item.id);
    } else {
      this.cartService.updateQuantity(item.id, newQty);
    }
  }

  remove(id: number): void {
    this.cartService.removeItem(id);
  }

  checkout(): void {
    this.close.emit();
    this.router.navigate(['/checkout']);
  }
}