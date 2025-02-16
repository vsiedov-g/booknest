import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import { Author } from "../models/author.model";
import { BehaviorSubject, tap } from "rxjs";



@Injectable({providedIn: 'root'})
export class AuthorService {
    private http = inject(HttpClient)
    authors = new BehaviorSubject<Author[]>(null);
    getAll(authorName?: string){
        if(authorName)
        {
            return this.http.get<Author[]>(API_ENDPOINTS.AUTHOR.GET_ALL, {params: new HttpParams().set('authorName', authorName) });
        }
        return this.http.get<Author[]>(API_ENDPOINTS.AUTHOR.GET_ALL).pipe(tap({next: res => this.storageAuthors(res)}));
    }
    getById(authorId: number){
        return this.http.get(API_ENDPOINTS.AUTHOR.GET_BY_ID, {params: new HttpParams().set('authorId', authorId) });
    }
    update(author: Author){
        return this.http.put(API_ENDPOINTS.AUTHOR.UPDATE, author);
    }
    add(author: Author){
        return this.http.put(API_ENDPOINTS.AUTHOR.ADD, author);
    }
    delete(authorId: number){
        return this.http.delete(API_ENDPOINTS.AUTHOR.DELETE, {params: new HttpParams().set('authorId', authorId) });
    }
    storageAuthors(authors: Author[])
    {
        this.authors.next(authors);
        console.log(this.authors);
    }
}