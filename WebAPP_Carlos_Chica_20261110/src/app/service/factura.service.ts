import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Factura {
  id: number;
  idCliente: number;
  nombreCliente: string;
  numeroFactura: string;
  fechaEmision: string;
  fechaVencimiento: string;
  monto: number;
  saldoPendiente: number;
  estadoPago: string;
}

@Injectable({ providedIn: 'root' })
export class FacturaService {
  private http = inject(HttpClient);
  private url = 'http://localhost:5279/api/factura';

  getFacturas(): Observable<Factura[]> {
    return this.http.get<Factura[]>(this.url);
  }
}
