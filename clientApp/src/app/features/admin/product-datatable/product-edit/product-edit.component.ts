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
  private productId = inject(MAT_DIALOG_DATA);
  private productService = inject(ProductService);
  private authorService = inject(AuthorService);
  private categoryService = inject(CategoryService);
  private dialogRef = inject(MatDialogRef);
  productForm = new FormGroup({
    'id': new FormControl(0),
    'title': new FormControl(),
    'description': new FormControl(),
    'author': new FormControl(),
    'categories': new FormArray([]),
    'price': new FormControl()
  })
  imageUrl: string;
  imageFile: File;
  authorsFromDb: Author[] = [];
  categoriesFromDb: Category[] = [];

  ngOnInit(){
    if(this.productId){
      this.productService.getById(this.productId).subscribe({next: (res) => {
        this.initForm(res);
      }})
    }
  }

  initForm(product: Product)
  {
    let id = 0
    let title = '';
    let description = '';
    let author = '';
    let price = 0;
    if(product != null)
    {
      id = product.id;
      title = product.title;
      description = product.description;
      author = product.author;
      price = product.price;
    }

    this.productForm = new FormGroup({
      'id': new FormControl({value: id, disabled: id === 0}),
      'title': new FormControl(title),
      'description': new FormControl(description),
      'author': new FormControl(author),
      'categories': new FormArray([]),
      'price': new FormControl(price)
    })

    this.addCategoriesToFormArray(product.categories);
    this.imageUrl = product.imageUrl;
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
      price: this.productForm.value.price,
      imageUrl: this.imageUrl
    }
    if(this.productId){
      this.productService.update(product, this.imageFile).subscribe(res => {
          console.log(res);
          this.initForm(res);
        }
      );
    }else{
      this.productService.add(product, this.imageFile).subscribe(res => {
        console.log(res)
        this.dialogRef.close(true);
      });
    }
  }

  onCancel()
  {
    this.dialogRef.close(false);
  }

  onUploadImage(event: Event)
  {
    var file = (event.target as HTMLInputElement).files[0];
    this.imageFile = file;
    const reader = new FileReader();
    reader.onload = () => {
      this.imageUrl = reader.result as string;
    }
    reader.readAsDataURL(file);

  }
}
