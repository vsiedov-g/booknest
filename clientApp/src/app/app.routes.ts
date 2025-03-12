import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { SignupComponent } from './features/auth/signup/signup.component';
import { LoginComponent } from './features/auth/login/login.component';
import { AdminGuard } from './core/resolvers/roleGuard';
import { authUserResolver, homePageProductResolver, ProductDetailResolver } from './core/resolvers/resolver';
import { ProductDetailComponent } from './features/home/product-detail/product-detail.component';
import { AccountComponent } from './features/auth/account/account.component';



export const routes: Routes = [
    {
        path: 'signup',
        component: SignupComponent
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'account',
        component: AccountComponent,
        resolve: {user: authUserResolver}
    },
    {
        path: 'home',
        component: HomeComponent,
        resolve: {products: homePageProductResolver}
    },
    {
        path: 'product/:id',
        component: ProductDetailComponent,
        resolve: {product: ProductDetailResolver}
    },
    {
        path: 'manage',
        canActivate: [AdminGuard],
        loadChildren: () => import('./features/admin/admin.routes').then(x => x.adminRoutes),
    }
];
