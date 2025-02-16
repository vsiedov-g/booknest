import { AfterViewInit, Component, inject, ViewChild } from '@angular/core';
import { MatTableModule, MatTable } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { AuthorDatatableDataSource } from './author-datatable-datasource';
import { Author } from '../../../core/models/author.model';
import { ActivatedRoute } from '@angular/router';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { AuthorEditComponent } from './author-edit/author-edit.component';
import { AuthorService } from '../../../core/services/author.service';

@Component({
  selector: 'app-author-datatable',
  templateUrl: './author-datatable.component.html',
  styleUrl: './author-datatable.component.css',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule, MatDialogModule]
})
export class AuthorDatatableComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<Author>;
  dataSource = new AuthorDatatableDataSource();
  private route = inject(ActivatedRoute);
  private dialog = inject(MatDialog);
  private authorService = inject(AuthorService);
  
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'name', 'edit'];
  ngOnInit(){
    this.route.data.subscribe({next: (res) => {
      this.dataSource.setAuthors(res['authors']);
  }});
  }
  ngAfterViewInit(): void {
    this.loadData();
  }

  openEditDialog(data?: any){
    const dialogRef = this.dialog.open(AuthorEditComponent, {
      data
    });
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if(val) {
          this.authorService.authors.subscribe({
            next: x => {
              console.log(x);
              window.location.reload();
            }
          })
        }
      }
    })
  }

  onDeleteAuthor(authorId: number)
  {
    this.authorService.delete(authorId).subscribe( { 
      next: () => {
        window.location.reload();
      }
    });
  }

  loadData()
  {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }

}
