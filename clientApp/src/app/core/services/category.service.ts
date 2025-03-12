import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import { Category } from "../models/category.model";
import { BehaviorSubject, tap } from "rxjs";


@Injectable({providedIn: 'root'})
export class CategoryService {
    private http = inject(HttpClient)
    categories = new BehaviorSubject<Category[]>(null); 
    getAll(categoryName?: string){
        if(categoryName)
        {
            return this.http.get<Category[]>(API_ENDPOINTS.CATEGORY.GET_ALL,{params: new HttpParams().set('categoryName', categoryName)});
        }
        return this.http.get<Category[]>(API_ENDPOINTS.CATEGORY.GET_ALL).pipe(tap({next: res => this.storageCategories(res)}));
    }
    getById(categoryId: number){
        return this.http.get(API_ENDPOINTS.CATEGORY.GET_BY_ID, {params: new HttpParams().set('categoryId', categoryId) });
    }
    update(category: Category){
        return this.http.put(API_ENDPOINTS.CATEGORY.UPDATE, category);
    }
    add(category: Category)
    {
        return this.http.put(API_ENDPOINTS.CATEGORY.ADD, category );
    }
    delete(categoryId: number){
        return this.http.delete(API_ENDPOINTS.CATEGORY.DELETE, {params: new HttpParams().set('categoryId', categoryId) })
    }
    storageCategories(categories: Category[]) 
    {
        this.categories.next(categories);
        console.log(this.categories);
    }
}