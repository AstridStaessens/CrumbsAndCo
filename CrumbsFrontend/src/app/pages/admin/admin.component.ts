import { Component, inject, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService, ProductResponse, CategoryResponse } from '../../services/product.service';
import { OrderService, OrderResponse } from '../../services/order.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CurrencyPipe, DatePipe, FormsModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss',
})
export class AdminComponent implements OnInit {
  productService = inject(ProductService);
  orderService = inject(OrderService);
  http = inject(HttpClient);
  apiUrl = environment.apiUrl;

  activeTab = signal<'products' | 'categories' | 'orders'>('products');

  products: ProductResponse[] = [];
  categories: CategoryResponse[] = [];
  orders: OrderResponse[] = [];

  // Product form
  productForm = {
    name: '',
    description: '',
    price: 0,
    stock: 0,
    imageUrl: '',
    categoryId: 0
  };
  editingProductId: number | null = null;

  // Category form
  categoryForm = { name: '' };
  editingCategoryId: number | null = null;

  orderStatuses = ['new', 'pending_payment', 'paid', 'in_production', 'ready', 'completed', 'cancelled', 'refunded'];

  ngOnInit(): void {
    this.loadProducts();
    this.loadCategories();
    this.loadOrders();
  }

  setTab(tab: 'products' | 'categories' | 'orders'): void {
    this.activeTab.set(tab);
  }

  loadProducts(): void {
    this.productService.getAll().subscribe((products) => (this.products = products));
  }

  loadCategories(): void {
    this.productService.getCategories().subscribe((categories) => (this.categories = categories));
  }

  loadOrders(): void {
    this.http.get<OrderResponse[]>(`${this.apiUrl}/orders`).subscribe((orders) => {
      this.orders = orders.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
    });
  }

  // Products
  editProduct(product: ProductResponse): void {
    this.editingProductId = product.id;
    this.productForm = {
      name: product.name,
      description: product.description,
      price: product.price,
      stock: product.stock,
      imageUrl: product.imageUrl ?? '',
      categoryId: product.categoryId
    };
  }

  resetProductForm(): void {
    this.editingProductId = null;
    this.productForm = { name: '', description: '', price: 0, stock: 0, imageUrl: '', categoryId: this.categories[0]?.id ?? 0 };
  }

  saveProduct(): void {
    if (this.editingProductId) {
      this.http.put(`${this.apiUrl}/products/${this.editingProductId}`, this.productForm).subscribe(() => {
        this.loadProducts();
        this.resetProductForm();
      });
    } else {
      this.http.post(`${this.apiUrl}/products`, this.productForm).subscribe(() => {
        this.loadProducts();
        this.resetProductForm();
      });
    }
  }

  deleteProduct(id: number): void {
    if (!confirm('Product deactiveren?')) return;
    this.http.delete(`${this.apiUrl}/products/${id}`).subscribe(() => this.loadProducts());
  }

  // Categories
  editCategory(category: CategoryResponse): void {
    this.editingCategoryId = category.id;
    this.categoryForm = { name: category.name };
  }

  resetCategoryForm(): void {
    this.editingCategoryId = null;
    this.categoryForm = { name: '' };
  }

  saveCategory(): void {
    if (this.editingCategoryId) {
      this.http.put(`${this.apiUrl}/categories/${this.editingCategoryId}`, this.categoryForm).subscribe(() => {
        this.loadCategories();
        this.resetCategoryForm();
      });
    } else {
      this.http.post(`${this.apiUrl}/categories`, this.categoryForm).subscribe(() => {
        this.loadCategories();
        this.resetCategoryForm();
      });
    }
  }

  deleteCategory(id: number): void {
    if (!confirm('Categorie verwijderen?')) return;
    this.http.delete(`${this.apiUrl}/categories/${id}`).subscribe(() => this.loadCategories());
  }

  // Orders
  orderActionError = '';
  refundingOrderId: number | null = null;

  updateOrderStatus(orderId: number, status: string): void {
    this.orderActionError = '';
    this.http.put(`${this.apiUrl}/orders/${orderId}/status`, JSON.stringify(status), {
      headers: { 'Content-Type': 'application/json' }
    }).subscribe({
      next: () => this.loadOrders(),
      error: (err) => {
        this.orderActionError = err?.error?.message ?? 'Status kon niet aangepast worden.';
        this.loadOrders();
      }
    });
  }

  canRefund(order: OrderResponse): boolean {
    return ['paid', 'in_production', 'ready'].includes(order.status);
  }

  refundOrder(order: OrderResponse): void {
    if (!confirm(`Bestelling #${order.id} markeren als terugbetaald? Dit moet je zelf nog effectief via Mollie terugstorten.`)) {
      return;
    }

    this.orderActionError = '';
    this.refundingOrderId = order.id;

    this.http.post(`${this.apiUrl}/payments/refund/${order.id}`, {}).subscribe({
      next: () => {
        this.refundingOrderId = null;
        this.loadOrders();
      },
      error: (err) => {
        this.refundingOrderId = null;
        this.orderActionError = err?.error?.message ?? 'Bestelling kon niet als terugbetaald gemarkeerd worden.';
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