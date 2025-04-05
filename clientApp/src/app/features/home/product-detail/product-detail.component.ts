import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { Product } from '../../../core/models/product.model';
import { ProductService } from '../../../core/services/product.service';

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
  private router = inject(Router);
  private productService = inject(ProductService);
  ngOnInit(){
   this.route.data.subscribe({
    next: (data) => {
      this.product = data['product'];
    }
   })
  }

  onBuyProduct(){
    this.productService.createPayment(this.product.id).subscribe({next: (res) => {
      console.log(res.pageUrl);
      window.location.href = res.pageUrl;
    }});
  }
}
