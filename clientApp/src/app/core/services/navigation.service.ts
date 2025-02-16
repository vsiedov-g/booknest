import { inject, Injectable } from "@angular/core";
import { NavigationCancel, NavigationEnd, NavigationStart, Router } from "@angular/router";
import { Subscription } from "rxjs";

@Injectable({providedIn: 'root'})

export class NavigationService{

    private router = inject(Router);
    private routerSub: Subscription; 
    public savedUrl: string;

    subscribe(){
        this.routerSub = this.router.events.subscribe(event => 
        {
            if (event instanceof NavigationStart)
            {
                this.savedUrl = event.url;
            } 
        }
        )
    }

    unsubscribe(){
        this.routerSub.unsubscribe();
    }
}