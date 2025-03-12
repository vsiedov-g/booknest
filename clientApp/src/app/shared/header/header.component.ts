import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, MatIconModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit, OnDestroy{
  isAuthenticated = false;
  isAdmin = false;
  private userSub: Subscription;
  private authService = inject(AuthService)
  ngOnInit() {
    this.userSub = this.authService.user.subscribe(user => 
      {
        this.isAuthenticated = !!user.role;
        if(!!user?.role && user.role === 'admin')
        {
          this.isAdmin = true;
        }
      }
    );
  }

  onLogout(){
    this.authService.httpLogout();
    this.isAuthenticated = false;
  }

  ngOnDestroy(){
    this.userSub.unsubscribe();
  }
}
