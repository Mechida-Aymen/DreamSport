import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { filter, take, BehaviorSubject, Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../service/auth/authservice';
import { Router } from '@angular/router';
import { Store} from '@ngrx/store';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);


  Tenant: string | number | null = null; // Raw value for tenantId
  private excludedRoutes: string[] = [
    '/auth/register',
    '/auth/forgot-password',
    '/auth/login',
    '/auth/refresh',
  ];

  constructor(private authService: AuthService,private router: Router,private store: Store) {
     this.Tenant=localStorage.getItem("Tenant-ID");
     

  } 

  private isExcluded(url: string): boolean {
    return this.excludedRoutes.some(route =>
      url.includes(route) || url.includes('/'+this.Tenant+route)
    );
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.isExcluded(req.url)) {
      return next.handle(req);
    }

    const token = this.authService.getAccessToken();
    let headers = req.headers;

    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }

    if (this.Tenant) {
      headers = headers.set('Tenant-ID', this.Tenant.toString());
    }

    const modifiedReq = req.clone({ headers });

    return next.handle(modifiedReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error(req, next);
        }
        return throwError(() => error);
      })
    );
  }

  private handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      console.log('[Interceptor] Starting token refresh');

      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((response: any) => {
          this.isRefreshing = false;
          const newToken = response.token;
          console.log('[Interceptor] Received new token:', newToken);

          if (newToken) {
            this.refreshTokenSubject.next(newToken);

            const updatedReq = req.clone({
              headers: req.headers
                .set('Authorization', `Bearer ${newToken}`)
                .set('Tenant-ID', this.Tenant?.toString() || '')
            });

            return next.handle(updatedReq);
          }
          console.error('[Interceptor] Token refresh failed (no token in response)');

          return throwError(() => new Error('Token refresh failed'));
        }),
        catchError((err ) => {
          if (err.status === 401) {
            console.error('[Interceptor] Refresh token request failed:', err);
        
            this.isRefreshing = false;
            this.authService.logout();
            this.router.navigate(['/' + this.Tenant + '/auth/login']);
          }
        
          return throwError(() => new Error('Session expired'));
        })
      );
    } else {
      console.log('[Interceptor] Token refresh already in progress, waiting...');

      // Wait for the new token
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap((token) => {
          console.log('[Interceptor] Retrying request with new token');

          const updatedReq = req.clone({
            headers: req.headers
              .set('Authorization', `Bearer ${token}`)
              .set('Tenant-ID', this.Tenant?.toString() || '')
          });
          return next.handle(updatedReq);
        })
      );
    }
  }
  
}