import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import { Product } from "../models/product.model";
import { BehaviorSubject, tap } from "rxjs";


@Injectable({providedIn: 'root'})
export class ProductService {
    private http = inject(HttpClient)
    manageProducts = new BehaviorSubject<Product[]>(null);
    homePageProducts = new BehaviorSubject<Product[]>(null);
    getAll(isAdmin: boolean){
        return this.http.get<Product[]>(API_ENDPOINTS.PRODUCT.GET_ALL).pipe(tap({next: res => this.storageProducts(res, isAdmin)}));
    }
    getById(productId: number){
        return this.http.get<Product>(API_ENDPOINTS.PRODUCT.GET_BY_ID, {params: new HttpParams().set('productId', productId) });
    }
    add(product: Product)
    {
        return this.http.put(API_ENDPOINTS.PRODUCT.ADD, product);
    }
    update(product: Product){
        return this.http.put(API_ENDPOINTS.PRODUCT.UPDATE, product);
    }
    delete(productId: number){
        return this.http.delete(API_ENDPOINTS.PRODUCT.DELETE, {params: new HttpParams().set('productId', productId) });
    }
    storageProducts(products: Product[], isAdmin: boolean) 
    {
        if(isAdmin)
        {
            this.manageProducts.next(products);
            console.log(this.manageProducts);
        }else 
        {
            this.homePageProducts.next(products);
            console.log(this.homePageProducts);

        }
    }
}