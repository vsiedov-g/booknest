import { Component, inject, input } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { UserDto } from '../../../../core/models/userDto.model';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from '../../../../core/services/user.service';

@Component({
  selector: 'app-user-info',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.css'
})
export class UserInfoComponent {
  private userService = inject(UserService);
  user = input<UserDto>();
  userForm: FormGroup;
  editingMode = false;
  ngOnInit()
  {
    this.initForm(this.user());
  }
  initForm(user: UserDto)
  {
    this.userForm = new FormGroup({
      'email': new FormControl(user.email),
      'name': new FormControl(user.name),
      'mobilePhone': new FormControl(user.mobilePhone)
    })
    this.userForm.disable();
  }
  onSubmit(){
    let updatedUser: UserDto = 
    {
      id: this.user().id,
      email: this.userForm.value.email,
      name: this.userForm.value.name,
      mobilePhone: this.userForm.value.mobilePhone,
      role: this.user().role
    }
    this.userService.update(updatedUser).subscribe(() => {
      this.userForm.disable();
      this.editingMode = false;
    });
  }
  onChangeUserForm()
  {
    this.userForm.enable();
    this.editingMode = true;
  }
}
