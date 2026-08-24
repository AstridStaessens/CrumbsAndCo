import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe, NgClass, NgFor, NgIf } from '@angular/common';
import { ProductService, ProductResponse, CategoryResponse } from '../../services/product.service';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [CurrencyPipe, NgClass, NgFor, NgIf],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  cartService = inject(CartService);
  productService = inject(ProductService);

  categories: CategoryResponse[] = [];
  products: ProductResponse[] = [];
  filteredProducts: ProductResponse[] = [];
  selectedCategoryId: number | null = null;
  isLoading = true;

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  loadCategories(): void {
    this.productService.getCategories().subscribe((categories) => {
      this.categories = categories;
    });
  }

  loadProducts(): void {
    this.isLoading = true;
    this.productService.getAll().subscribe({
      next: (products) => {
        this.products = products;
        this.filteredProducts = products;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  selectCategory(categoryId: number | null): void {
    this.selectedCategoryId = categoryId;
    if (categoryId === null) {
      this.filteredProducts = this.products;
    } else {
      this.productService.getByCategory(categoryId).subscribe((products) => {
        this.filteredProducts = products;
      });
    }
  }

  addToCart(product: ProductResponse): void {
    this.cartService.addToCart({
      id: product.id,
      name: product.name,
      category: 'bread',
      price: product.price,
      image: product.imageUrl ?? '',
      description: product.description
    });
  }
}