import { Component } from '@angular/core';
import { FacturaComponent } from './component/factura-component/factura-component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FacturaComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {}
