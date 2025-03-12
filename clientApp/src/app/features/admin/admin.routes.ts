import { Routes } from "@angular/router";
import { UserDatatableComponent } from "./user-datatable/user-datatable.component";
import { AuthorResolver, CategoryResolver, manageProductResolver, UserResolver } from "../../core/resolvers/resolver";
import { AuthorDatatableComponent } from "./author-datatable/author-datatable.component";
import { CategoryDatatableComponent } from "./category-datatable/category-datatable.component";
import { ProductDatatableComponent } from "./product-datatable/product-datatable.component";
import { AuthorEditComponent } from "./author-datatable/author-edit/author-edit.component";
import { UserEditComponent } from "./user-datatable/user-edit/user-edit.component";
import { CategoryEditComponent } from "./category-datatable/category-edit/category-edit.component";
import { ProductEditComponent } from "./product-datatable/product-edit/product-edit.component";


export const adminRoutes: Routes = [
    {
        path: '',
        children: [
            {
                path: 'users',
                component: UserDatatableComponent,
                resolve: {users: UserResolver},
                children: [
                    {
                        path: ':id',
                        component: UserEditComponent
                    }
                ]
                
            },
            {
                path: 'authors',
                component: AuthorDatatableComponent,
                resolve: {authors: AuthorResolver},
            },
            {
                path: 'authors/:id',
                component: AuthorEditComponent
            },
            {
                path: 'categories',
                component: CategoryDatatableComponent,
                resolve: {categories: CategoryResolver},
                children: [
                    {
                        path: ':id',
                        component: CategoryEditComponent
                    }
                ]
            },
            {
                path: 'products',
                component: ProductDatatableComponent,
                resolve: {products: manageProductResolver},
                children: [
                    {
                        path: ':id',
                        component: ProductEditComponent
                    }
                ]
            }
        ]
    }   
]