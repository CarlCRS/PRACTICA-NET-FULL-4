import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FacturaService, Factura } from '../../service/factura.service';

@Component({
  selector: 'app-factura-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './factura-component.html',
  styleUrl: './factura-component.css'
})
export class FacturaComponent implements OnInit {
  private facturaService = inject(FacturaService);

  facturas: Factura[] = [];
  cargando = true;
  error = '';

  ngOnInit(): void {
    this.facturaService.getFacturas().subscribe({
      next: (datos) => {
        this.facturas = datos;
        this.cargando = false;
      },
      error: () => {
        this.error = 'No se pudo conectar con la API en http://localhost:5279';
        this.cargando = false;
      }
    });
  }
}
