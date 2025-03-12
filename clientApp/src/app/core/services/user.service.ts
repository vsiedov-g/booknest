import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../constants/api-endpoints";
import { UserDto } from "../models/userDto.model";
import { BehaviorSubject, tap } from "rxjs";


@Injectable({providedIn: 'root'})
export class UserService {
    private http = inject(HttpClient)
    users = new BehaviorSubject<UserDto[]>(null);;
    getAll(){
        return this.http.get<UserDto[]>(API_ENDPOINTS.USER.GET_ALL).pipe(tap({next: res => this.storageUsers(res)}));
    }
    getById(userId: number){
        return this.http.get<UserDto>(API_ENDPOINTS.USER.GET_BY_ID, {params: new HttpParams().set('userId', userId) });
    }
    update(user: UserDto){
        return this.http.put(API_ENDPOINTS.USER.UPDATE, user);
    }
    delete(){

    }
    storageUsers(users: UserDto[]) 
    {
        this.users.next(users);
        console.log(this.users);
    }
}