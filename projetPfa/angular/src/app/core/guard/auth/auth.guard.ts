import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../service/auth/authservice';
import { routes } from '../../core.index';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    public routes = routes;
  
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // Check if the user is authenticated
    if (this.authService.isAuthenticated()) {
      return true; // Allow access if authenticated
    } else {
      // Redirect to the login page if the user is not authenticated
      this.router.navigate([this.routes.login]);
      return false; // Block access
    }
  }
}
