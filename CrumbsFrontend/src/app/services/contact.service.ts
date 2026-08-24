import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ContactRequest {
  naam: string;
  email: string;
  bericht: string;
}

export interface CustomOrderRequest {
  naam: string;
  email: string;
  telefoon: string;
  type: string;
  datum: string;
  wensen: string;
  fileName: string;
}

@Injectable({ providedIn: 'root' })
export class ContactService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  sendContact(request: ContactRequest): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/contact`, request);
  }

  sendCustomOrder(request: CustomOrderRequest): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/contact/op-maat`, request);
  }
}
