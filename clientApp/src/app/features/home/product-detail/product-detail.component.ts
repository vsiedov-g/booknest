import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Product } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [MatButtonModule, MatCardModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})
export class ProductDetailComponent {
  product: Product;
  private route = inject(ActivatedRoute);
  ngOnInit(){
   this.route.data.subscribe({
    next: (data) => {
      this.product = data['product'];
      console.log(this.product);
    }
   })
  }
}
