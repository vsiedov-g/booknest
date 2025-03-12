import { AfterViewInit, Component, inject, ViewChild } from '@angular/core';
import { MatTableModule, MatTable } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { ProductDatatableDataSource } from './product-datatable-datasource';
import { Product } from '../../../core/models/product.model';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ProductService } from '../../../core/services/product.service';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { MatIconModule } from '@angular/material/icon';
import { ProductUploadFileComponent } from './product-upload-file/product-upload-file.component';

@Component({
  selector: 'app-product-datatable',
  templateUrl: './product-datatable.component.html',
  styleUrl: './product-datatable.component.css',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule, MatIconModule]
})
export class ProductDatatableComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<Product>;
  dataSource = new ProductDatatableDataSource();
  private route = inject(ActivatedRoute);
  private dialog = inject(MatDialog);
  private productService = inject(ProductService);

  displayedColumns = ['id', 'title', 'author', 'categories', 'price', 'edit', 'upload'];
  ngOnInit(){
    this.route.data.subscribe({next: (res) => {
      this.dataSource.setProducts(res['products']);
  }});

  }
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }

  openEditDialog(data?: any){
    const dialogRef = this.dialog.open(ProductEditComponent, {
      data,
      width: '1500px'
    });
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if(val) {
          this.productService.manageProducts.subscribe({
            next: x => {
              console.log(x);
              //change that 
              // window.location.reload();
            }})
        }
      }})
  }
    
  onDeleteProduct(productId: number){
    this.productService.delete(productId).subscribe( { 
      next: () => {
        window.location.reload();
      }
    });
  }

  onOpenUploadFileDialog(data: number){
    const dialogRef = this.dialog.open(ProductUploadFileComponent, {
      data
    });
  }
}
