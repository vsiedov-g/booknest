import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {MatSidenavModule} from '@angular/material/sidenav';
import { UserInfoComponent } from './user-info/user-info.component';
import { BehaviorSubject } from 'rxjs';
import { UserDto } from '../../../core/models/userDto.model';
import { UserProductsComponent } from './user-products/user-products.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [MatSidenavModule, MatButtonModule, UserInfoComponent, UserProductsComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent {
  private route = inject(ActivatedRoute);
  user: UserDto;
  isPersonalInfo = true;
  ngOnInit(){
    this.route.data.subscribe({
      next: (data) =>
      {
        this.user = data['user'];
        console.log(this.user);
      }
    })
  }
  onToggleProfileInfo()
  {
    this.isPersonalInfo = true;
  }
  onTogglePurchasedProducts()
  {
    this.isPersonalInfo = false;
  }
}
