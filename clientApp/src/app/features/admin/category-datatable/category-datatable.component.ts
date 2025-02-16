import { AfterViewInit, Component, inject, ViewChild } from '@angular/core';
import { MatTableModule, MatTable } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { CategoryDatatableDataSource } from './category-datatable-datasource';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Category } from '../../../core/models/category.model';
import { MatDialog } from '@angular/material/dialog';
import { CategoryService } from '../../../core/services/category.service';
import { CategoryEditComponent } from './category-edit/category-edit.component';

@Component({
  selector: 'app-category-datatable',
  templateUrl: './category-datatable.component.html',
  styleUrl: './category-datatable.component.css',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule]
})
export class CategoryDatatableComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<Category>;
  dataSource = new CategoryDatatableDataSource();
  private route = inject(ActivatedRoute);
  private dialog = inject(MatDialog);
  private categoryService = inject(CategoryService);

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'name', 'edit'];
  ngOnInit(){
    this.route.data.subscribe({next: (res) => {
      this.dataSource.setCategories(res['categories']);
  }});
  }
  openEditDialog(data?: any){
      const dialogRef = this.dialog.open(CategoryEditComponent, {
        data
      });
      dialogRef.afterClosed().subscribe({
        next: (val) => {
          if(val) {
            this.categoryService.categories.subscribe({
              next: x => {
                console.log(x);
                //change that 
                window.location.reload();
              }
            })
          }
        }
      })
    }
  
    onDeleteCategory(categoryId: number)
    {
      this.categoryService.delete(categoryId).subscribe( { 
        next: () => {
          window.location.reload();
        }
      });
    }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }
}
