import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface OrderLineRequest {
  productId: number;
  quantity: number;
}

export interface CreateOrderRequest {
  orderLines: OrderLineRequest[];
}

export interface OrderLineResponse {
  id: number;
  quantity: number;
  unitPrice: number;
  productId: number;
  productName: string;
}

export interface OrderResponse {
  id: number;
  date: string;
  status: string;
  total: number;
  userId: string;
  orderLines: OrderLineResponse[];
}

@Injectable({ providedIn: 'root' })
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  createOrder(request: CreateOrderRequest): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.apiUrl}/orders`, request);
  }

  getMyOrders(): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.apiUrl}/orders/my`);
  }

  getById(id: number): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(`${this.apiUrl}/orders/${id}`);
  }

  cancelOrder(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/orders/${id}/cancel`, {});
  }
}