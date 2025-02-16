import { Component, inject } from '@angular/core';
import { MatFormField } from '@angular/material/form-field';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Author } from '../../../../core/models/author.model';
import { AuthorService } from '../../../../core/services/author.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-author-edit',
  standalone: true,
  imports: [MatFormField, MatDialogModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './author-edit.component.html',
  styleUrl: './author-edit.component.css'
})
export class AuthorEditComponent {
  private data = inject(MAT_DIALOG_DATA);
  private authorService = inject(AuthorService);
  private dialogRef = inject(MatDialogRef);
  private router = inject(Router);
  authorForm: FormGroup;

  ngOnInit(){
    this.initForm();
  }

  initForm()
  {
    let id = '0'
    let name = '';
    if(this.data != null)
    {
      id = this.data.id;
      name = this.data.name;
    }

    this.authorForm = new FormGroup({
      'id': new FormControl(id),
      'name': new FormControl(name)
    })
  }

  onSubmit(){
    let author: Author = { 
      id: this.authorForm.value.id,
      name: this.authorForm.value.name
    }
    if(this.data)
    {
      console.log(author)
      this.authorService.update(author).subscribe(res => 
        {
          console.log(res)
          this.authorService.getAll().subscribe();
        }
      );
      this.dialogRef.close(true);
    }else{
      console.log(author)
      this.authorService.add(author).subscribe(res => 
      {
        console.log(res)
        this.authorService.getAll().subscribe();
      }
    );
      this.dialogRef.close(true);
    }

  }

  onCancel()
  {
    this.dialogRef.close;
  }
}
