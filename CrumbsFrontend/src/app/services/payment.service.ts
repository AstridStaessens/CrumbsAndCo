import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface PaymentResponse {
  paymentUrl: string;
  molliePaymentId: string;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class PaymentService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  createPayment(orderId: number): Observable<PaymentResponse> {
    return this.http.post<PaymentResponse>(`${this.apiUrl}/payments/create/${orderId}`, {});
  }

  getStatus(orderId: number): Observable<{ status: string }> {
    return this.http.get<{ status: string }>(`${this.apiUrl}/payments/status/${orderId}`);
  }
}