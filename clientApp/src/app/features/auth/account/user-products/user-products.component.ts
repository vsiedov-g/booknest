import { Component, inject } from '@angular/core';
import { ProductService } from '../../../../core/services/product.service';
import { Product } from '../../../../core/models/product.model';
import { AuthService } from '../../../../core/services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-user-products',
  standalone: true,
  imports: [MatCardModule, MatIconModule, MatGridListModule, MatButtonModule],
  templateUrl: './user-products.component.html',
  styleUrl: './user-products.component.css'
})
export class UserProductsComponent {
  private productService = inject(ProductService);
  private authService = inject(AuthService);
  products: Product[];
  ngOnInit()
  {
    this.productService.getUserProducts(this.authService.user.value.id).subscribe({next: (res) => {
      this.products = res;
    }})
  }
  
  onDownload(productId: number)
  {
    this.productService.dowloadProductFile(productId).subscribe({next: (res) => {
      const url = window.URL.createObjectURL(res);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${this.products.find( p => p.id === productId).title}.epub`;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    }})
  }
}
