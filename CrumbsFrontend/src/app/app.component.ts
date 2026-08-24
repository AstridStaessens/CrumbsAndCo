import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { CartComponent } from './components/cart/cart.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, CartComponent],
  template: `
    <app-header (cartClick)="cartOpen = true" />
    <main class="page-wrapper">
      <router-outlet />
    </main>
    <app-footer />
    <app-cart [isOpen]="cartOpen" (close)="cartOpen = false" />
  `,
})
export class AppComponent {
  cartOpen = false;
}
