import { AfterViewInit, Component, inject, OnInit, ViewChild } from '@angular/core';
import { MatTableModule, MatTable } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { UserDatatableDataSource } from './user-datatable-datasource';
import { UserDto } from '../../../core/models/userDto.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ROLES } from '../../../core/constants/roles';
import { UserService } from '../../../core/services/user.service';



@Component({
  selector: 'app-user-datatable',
  templateUrl: './user-datatable.component.html',
  styleUrl: './user-datatable.component.css',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule, CommonModule]
})
export class UserDatatableComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<UserDto>;
  dataSource = new UserDatatableDataSource();
  private route = inject(ActivatedRoute);
  private userService = inject(UserService);
  roles = ['admin', 'customer'];
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'name', 'email', 'mobilePhone', 'role'];
  ngOnInit(){
    this.route.data.subscribe({next: (res) => {
      this.dataSource.setUsers(res['users']); 
  }});
  }
  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }

  changeRole(id: number,role: string){
    let user = this.dataSource.data.find(i => i.id == id);
    user.role = role;
    this.userService.update(user);
  }
}
