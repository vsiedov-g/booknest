import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CategoryService } from '../../../../core/services/category.service';
import { Category } from '../../../../core/models/category.model';
import { MatFormField, MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-category-edit',
  standalone: true,
  imports: [MatFormField, MatDialogModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './category-edit.component.html',
  styleUrl: './category-edit.component.css'
})
export class CategoryEditComponent {
  private data = inject(MAT_DIALOG_DATA);
  private categoryService = inject(CategoryService);
  private dialogRef = inject(MatDialogRef);
  categoryForm: FormGroup;

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

    this.categoryForm = new FormGroup({
      'id': new FormControl(id),
      'name': new FormControl(name)
    })
  }

  onSubmit(){
    let category: Category = { 
      id: this.categoryForm.value.id,
      name: this.categoryForm.value.name
    }
    if(this.data)
    {
      console.log(category)
      this.categoryService.update(category).subscribe(res => 
        {
          console.log(res)
          this.categoryService.getAll().subscribe();
        }
      );
      this.dialogRef.close(true);
    }else{
      console.log(category)
      this.categoryService.add(category).subscribe(res => 
      {
        console.log(res)
        this.categoryService.getAll().subscribe();
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
