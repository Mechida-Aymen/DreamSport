import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../../service/auth/authservice';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const user = this.authService.getDecodedToken(); // Assuming you store decoded token

    // Get required roles from route data
    const requiredRoles = route.data['roles'] as Array<string>;

    // Check if the user is authenticated and has the required role
    if (user && requiredRoles && requiredRoles.includes(user.Role)) {
      return true; // Allow access if user has one of the required roles
    } else {
      this.router.navigate(['/auth/login']); // Redirect to login page if not authorized
      return false;
    }
  }
}