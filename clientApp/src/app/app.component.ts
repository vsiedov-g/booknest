import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './shared/header/header.component';
import { AuthService } from './core/services/auth.service';
import { NavigationService } from './core/services/navigation.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  private authService = inject(AuthService);
  private navigationService = inject(NavigationService);
  title = 'clientApp';

  ngOnInit() {
    this.navigationService.subscribe();
    this.authService.autoLogin();
  }
  
  ngOnDestroy(){
    this.navigationService.unsubscribe();
  }
}
