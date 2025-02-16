import { inject} from "@angular/core";
import { UserDto } from "../models/userDto.model";
import { ResolveFn } from "@angular/router";
import { UserService } from "../services/user.service";
import { Author } from "../models/author.model";
import { AuthorService } from "../services/author.service";
import { Category } from "../models/category.model";
import { CategoryService } from "../services/category.service";
import { Product } from "../models/product.model";
import { ProductService } from "../services/product.service";
import { AuthService } from "../services/auth.service";

export const UserResolver : ResolveFn<UserDto[]> = (route, state) => {
   const userService = inject(UserService);
   if(userService.users.value == null)
   {
      return userService.getAll();
   }
   return userService.users;
}

export const authUserResolver : ResolveFn<UserDto> = (route, state) => {
   const userService = inject(UserService);
   const authService = inject(AuthService);
   return userService.getById(authService.user.getValue().id);
}

export const AuthorResolver : ResolveFn<Author[]> = (route, state) => {
   const authorService = inject(AuthorService);
   if(authorService.authors.value == null)
   {
      return authorService.getAll();
   }
   return authorService.authors;
}

export const CategoryResolver : ResolveFn<Category[]> = (route, state) => {
   const categoryService = inject(CategoryService);
   if(categoryService.categories.value == null)
   {
      return categoryService.getAll();
   }
   return categoryService.categories;
}

export const manageProductResolver : ResolveFn<Product[]> = (route, state) => {
   const productService = inject(ProductService);
   if(productService.manageProducts.value == null)
   {
      return productService.getAll(true);
   }
   return productService.manageProducts;
}

export const homePageProductResolver : ResolveFn<Product[]> = (route, state) => {
   const productService = inject(ProductService);
   if(productService.homePageProducts.value == null)
   {
      return productService.getAll(false);
   }
   return productService.homePageProducts;
}

export const ProductDetailResolver : ResolveFn<Product> = (route, state) => {
   const productService = inject(ProductService);
   
   return productService.getById(+route.params['id']);
}

