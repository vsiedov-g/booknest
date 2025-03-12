import { Component, inject } from '@angular/core';
import { ProductService } from '../../core/services/product.service';
import { Product } from '../../core/models/product.model';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';
import {MatGridListModule} from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { apiBaseUrl } from '../../../environments/development';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatButtonModule, MatCardModule, MatGridListModule, MatIconModule,RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private productService = inject(ProductService);
  private route = inject(ActivatedRoute);
  products: Product[] = [];
  ngOnInit()
  {
    this.route.data.subscribe({
      next: (data) =>
      {
        this.products = data['products'];
        console.log(this.products);
      }
    })
  }
}
