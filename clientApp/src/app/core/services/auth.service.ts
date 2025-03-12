import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { BehaviorSubject, Subject, tap } from "rxjs";
import { Router } from "@angular/router";
import { AuthUser } from "../models/authUser.model";
import { API_ENDPOINTS } from "../constants/api-endpoints";


@Injectable({providedIn: 'root'})
export class AuthService {
    user = new BehaviorSubject<AuthUser>(null);
    private httpClient = inject(HttpClient)
    private router = inject(Router);


    login(email: string, password: string)
    {
        return this.httpClient.post<AuthUser>(API_ENDPOINTS.AUTH.LOGIN, {Email:email, Password:password}, {withCredentials: true}).pipe(tap(resData => {
           this.handleAuthentication(resData.id, resData.email, resData.role, resData.accessToken);
           console.log(resData)
        }));
    }

    httpLogout()
    {
        const id: number = this.user.value.id;
        console.log(id)
        this.httpClient.post(API_ENDPOINTS.AUTH.LOGOUT, id).subscribe(() => 
            {
                this.logout();
            }
        )
    }
    logout()
    {
        this.user.next(null);
            localStorage.clear();
            window.location.reload();
    }

    signup(email: string, password: string)
    {
        return this.httpClient.post<AuthUser>(API_ENDPOINTS.AUTH.SIGNUP, {Email: email, Password: password}).pipe(tap(resData => {
            this.handleAuthentication(resData.id, resData.email, resData.role, resData.accessToken);
         }));
    }

    refreshToken(accessToken: string)
    {
        //415 status code solution
        var formData = new FormData();
        formData.append('accessToken', accessToken);

        return this.httpClient.post<{message: string}>(API_ENDPOINTS.AUTH.REFRESH_TOKEN, formData, {withCredentials: true});
    }

    autoLogin() {
        const userData: {
            id: number,
            email: string,
            role: string,
            accessToken: string
        } = JSON.parse(localStorage.getItem('userData'));
        if (!userData){
            return;
        }

        const loadedUser = new AuthUser(userData.id,userData.email, userData.role, userData.accessToken)
        this.user.next(loadedUser);
    }
    setNewAccessToken(accessToken: string){
        const user = this.user.value;
        user.accessToken = accessToken;
        this.user.next(user);
        localStorage.setItem('userData', JSON.stringify(user));
    }

    private handleAuthentication(id: number, email: string, role: string, token: string)
    {
        const user = new AuthUser(id, email, role, token);
        this.user.next(user);
        console.log(this.user)
        localStorage.setItem('userData', JSON.stringify(user));
    }
}