import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  userForm = new FormGroup({
    email: new FormControl(),
    password: new FormControl(),
    confirmPassword: new FormControl()
  });
   private authService = inject(AuthService);
   private router = inject(Router);

  onSubmit(){
    this.authService.signup(this.userForm.value.email, this.userForm.value.password)
        .subscribe({
          next: res => {
            console.log(res)
            this.router.navigate(['/home'])
          },
          error: err => console.log(err)
        });
  }


}
