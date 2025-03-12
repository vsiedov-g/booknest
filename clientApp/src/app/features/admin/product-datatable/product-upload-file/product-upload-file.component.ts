import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { ProductService } from '../../../../core/services/product.service';

@Component({
  selector: 'app-product-upload-file',
  standalone: true,
  imports: [MatDialogModule],
  templateUrl: './product-upload-file.component.html',
  styleUrl: './product-upload-file.component.css'
})
export class ProductUploadFileComponent {
  private productService = inject(ProductService);
  private productId = inject(MAT_DIALOG_DATA);
  file: File;

  onUploadProductFile()
  {
    this.productService.uploadProductFile(this.productId, this.file).subscribe({next: (res) => {
      console.log(res);
    }});
  }

  onChangeFile(event: Event)
  {
    this.file = (event.target as HTMLInputElement).files[0];
    console.log(this.file.name);
  }
}
