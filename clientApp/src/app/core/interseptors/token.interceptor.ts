import {  HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { AuthService } from "../services/auth.service";
import { catchError, of, tap } from "rxjs";
import { Router } from "@angular/router";
import { NavigationService } from "../services/navigation.service";

 export function tokenInterceptor(request: HttpRequest<unknown>, next: HttpHandlerFn)
{
    const authService = inject(AuthService);
    const navigationService = inject(NavigationService);
    const router = inject(Router);
    const user = authService.user.getValue();
    const accessToken = user?.accessToken;
    if (!accessToken)
    {
        return next(request);   
    }
    var req = request.clone(
        {
            setHeaders: {
                Authorization: `Bearer ${accessToken}`
            }
        }
    )
    return next(req).pipe(
        catchError(err => {
            if (err.status === 401){
                return authService.refreshToken(accessToken).pipe(tap({ 
                next: res => {
                    console.log(res);
                    authService.setNewAccessToken(res.message);
                    router.navigateByUrl(navigationService.savedUrl);
                }
            }))}
            if (err.error === 'REFRESH_TOKEN_NULL')
            {
                console.log(err);
                //need some error message displayed to user
                authService.logout();
                return of(err);
            }
            console.log(err);
            return of(err);
        }))
}
