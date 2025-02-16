import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormField, MatInputModule } from '@angular/material/input';
import {MatAutocompleteModule, MatAutocompleteSelectedEvent} from '@angular/material/autocomplete';
import { MatChipsModule} from '@angular/material/chips';
import {MatIconModule} from '@angular/material/icon';
import { ProductService } from '../../../../core/services/product.service';
import { Product } from '../../../../core/models/product.model';
import { AuthorService } from '../../../../core/services/author.service';
import { CategoryService } from '../../../../core/services/category.service';
import { Author } from '../../../../core/models/author.model';
import { Category } from '../../../../core/models/category.model';

@Component({
  selector: 'app-product-edit',
  standalone: true,
  imports: [MatFormField, MatDialogModule, MatInputModule, ReactiveFormsModule, MatAutocompleteModule, MatChipsModule, MatIconModule],
  templateUrl: './product-edit.component.html',
  styleUrl: './product-edit.component.css'
})
export class ProductEditComponent implements OnInit{
  private data = inject(MAT_DIALOG_DATA);
  private productService = inject(ProductService);
  private authorService = inject(AuthorService);
  private categoryService = inject(CategoryService);
  private dialogRef = inject(MatDialogRef);
  productForm: FormGroup;
  authorsFromDb: Author[] = [];
  categoriesFromDb: Category[] = [];

  ngOnInit(){
    this.initForm();
  }

  initForm()
  {
    let id = 0
    let title = '';
    let description = '';
    let author = '';
    let price = '';
    if(this.data != null)
    {
      id = this.data.id;
      title = this.data.title;
      description = this.data.description;
      author = this.data.author;
      price = this.data.price;
    }

    this.productForm = new FormGroup({
      'id': new FormControl({value: id, disabled: id === 0}),
      'title': new FormControl(title),
      'description': new FormControl(description),
      'author': new FormControl(author),
      'categories': new FormArray([]),
      'price': new FormControl(price)
    })

    this.addCategoriesToFormArray(this.data.categories);
  }

  get categories(): FormArray {
    return this.productForm.get('categories') as FormArray;
  }

  addCategoriesToFormArray(categories: string[])
  {
    if(categories)
    {
      categories.forEach(category =>
      {
        this.categories.push(new FormControl(category));
      }
      )
    }
  }

  onRemoveCategory(index: number)
  {
    this.categories.removeAt(index);
  }

  onAddCategory(event: MatAutocompleteSelectedEvent)
  {
    this.categories.push(new FormControl(event.option.value))
    console.log(this.productForm.value)
  }

  onFetchCategories(event: Event)
  {
    if((event.target as HTMLInputElement).value !=null)
    {
      this.categoryService.getAll((event.target as HTMLInputElement).value)
      .subscribe(res => {
        this.categoriesFromDb = res;
        console.log(res);
      }); 
    }
  }

  onFetchAuthors(event: Event)
  {
    if((event.target as HTMLInputElement).value)
    {
      this.authorService.getAll((event.target as HTMLInputElement).value)
      .subscribe(res => {
        this.authorsFromDb = res;
        console.log(res);
      }); 
    }
  }

  onSubmit(){
    let product: Product = { 
      id: this.productForm.value.id, 
      title: this.productForm.value.title,
      description: this.productForm.value.description,
      author: this.productForm.value.author,
      categories: this.productForm.value.categories,
      price: this.productForm.value.price
    }
    if(this.data)
    {
      console.log(product)
      this.productService.update(product).subscribe(res => 
        {
          console.log(res)
          this.productService.getAll(true).subscribe();
        }
      );
      this.dialogRef.close(true);
    }else{
      console.log(product)
      this.productService.add(product).subscribe(res => 
      {
        console.log(res)
        this.productService.getAll(true).subscribe();
      }
    );
      this.dialogRef.close(true);
    }

  }

  onCancel()
  {
    this.dialogRef.close(false);
  }
}
