import { CanMatchFn } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { inject } from "@angular/core";
import { ROLES } from "../constants/roles";

export const AdminGuard: CanMatchFn = (route, segments) => {
    const authService = inject(AuthService);
    const user = authService.user.getValue();
    if (user.role == ROLES.ADMIN)
        return true;
    return false;
}