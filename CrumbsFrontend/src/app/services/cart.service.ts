import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Product, CartItem } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class CartService {
  private itemsSubject = new BehaviorSubject<CartItem[]>([]);

  items$ = this.itemsSubject.asObservable();

  totalCount$ = this.items$.pipe(
    map((items) => items.reduce((sum, item) => sum + item.quantity, 0))
  );

  subtotal$ = this.items$.pipe(
    map((items) => items.reduce((sum, item) => sum + item.price * item.quantity, 0))
  );

  addToCart(product: Product): void {
    const current = this.itemsSubject.getValue();
    const existing = current.find((i) => i.id === product.id);
    if (existing) {
      this.itemsSubject.next(
        current.map((i) =>
          i.id === product.id ? { ...i, quantity: i.quantity + 1 } : i
        )
      );
    } else {
      this.itemsSubject.next([...current, { ...product, quantity: 1 }]);
    }
  }

  updateQuantity(productId: number, quantity: number): void {
    const current = this.itemsSubject.getValue();
    this.itemsSubject.next(
      current.map((i) => (i.id === productId ? { ...i, quantity } : i))
    );
  }

  removeItem(productId: number): void {
    const current = this.itemsSubject.getValue();
    this.itemsSubject.next(current.filter((i) => i.id !== productId));
  }

  clearCart(): void {
    this.itemsSubject.next([]);
  }

  get items(): CartItem[] {
    return this.itemsSubject.getValue();
  }
}
