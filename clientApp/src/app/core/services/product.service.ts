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
    getUserProducts(userId: number)
    {
        return this.http.get<Product[]>(API_ENDPOINTS.PRODUCT.GET_ALL, {params: new HttpParams().set('userId', userId)});
    }
    getById(productId: number){
        return this.http.get<Product>(API_ENDPOINTS.PRODUCT.GET_BY_ID, {params: new HttpParams().set('productId', productId)});
    }
    add(product: Product, imageFile?: File)
    {
        const formData: FormData = new FormData();
        if(imageFile != null){
            formData.append('imageFile', imageFile, imageFile.name);
        }
        formData.append('productDto', JSON.stringify(product));
        return this.http.put(API_ENDPOINTS.PRODUCT.ADD, formData);
    }
    update(product: Product, imageFile?: File){
        const formData: FormData = new FormData();
        if(imageFile != null){
            formData.append('imageFile', imageFile, imageFile.name);
        }
        formData.append('productDto', JSON.stringify(product));
        return this.http.put<Product>(API_ENDPOINTS.PRODUCT.UPDATE, formData);
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
    uploadProductFile(productId: number, file: File)
    {
        var formData = new FormData();
        formData.append('file', file, file.name);
        return this.http.put<string>(API_ENDPOINTS.PRODUCT.UPLOAD_FILE, formData, {params: new HttpParams().set('productId', productId)});
    }
    dowloadProductFile(productId: number)
    {
        return this.http.get(API_ENDPOINTS.PRODUCT.DOWNLOAD_FILE,  {params: new HttpParams().set('productId', productId), responseType: 'blob'}, );
    }
    createPayment(productId: number)
    {
        const redirectUrl = window.location.origin + '/home';
        var formData = new FormData();
        formData.append('redirectUrl', redirectUrl);
        return this.http.post<{pageUrl: string}>(API_ENDPOINTS.PRODUCT.CREATE_PAYMENT, formData, {params: new HttpParams().set('productId', productId)});
    }
}