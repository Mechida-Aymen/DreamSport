import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap, take } from 'rxjs/operators';
import * as TenantActions from '../../store/tenant/tenant.actions';
import { routes } from '../../core.index';

@Injectable({
  providedIn: 'root'
})
export class TenantGuard implements CanActivate {
  constructor(
    private store: Store<any>,
    private router: Router,
    private http: HttpClient
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    const fullPath = state.url; // Récupère le chemin complet (/28/auth/login)
    const tenantId = this.getTenantIdFromUrl(fullPath);

    if (!tenantId) {
      this.router.navigate(['error/error404']);
      return of(false);
    }

    // Correction du problème de double tenant
    const expectedPath = `/${tenantId}`;
    if (fullPath === expectedPath || fullPath.startsWith(expectedPath + '/')) {
      // Chemin correct, on continue
      return this.processTenant(tenantId);
    } else {
      // Redirection propre avec un seul tenantId
      this.router.navigate([`/${tenantId}${fullPath}`]);
      return of(false);
    }
  }
  public routes = routes;

  private processTenant(tenantId: number): Observable<boolean> {
    this.changeTenant(tenantId);
    return this.checkTenantExistence(tenantId).pipe(
      tap((exists) => {
        if (!exists) {
          this.router.navigate([this.routes.error404]);
        }
      }),
      map((exists) => exists),
      take(1) // Important pour éviter les boucles
    );
  }
  private getTenantIdFromUrl(path: string): number | null {
    const match = path.match(/^\/(\d+)(\/|$)/);
    
    if (match) {
      return parseInt(match[1], 10);
    }

    const tenantSlug = path.split('/')[1];
    const tenantMap: { [key: string]: number } = {
      'club1': 1,
      'club2': 2,
      'club3': 3
    };
    return tenantMap[tenantSlug] || null;
  }

  private changeTenant(tenantId: number) {
    this.store.dispatch(TenantActions.loadTenantData({ tenantId }));
  
    this.store.subscribe((state: any) => {
      if (state?.tenant?.siteInfo?.[0]) {
        const site = state.tenant.siteInfo[0];
        if (site.couleurPrincipale && site.couleurSecondaire) {
          this.applyMainColor(site.couleurPrincipale, site.couleurSecondaire);
        }
      }
    });
  }

  private applyMainColor(color: string, color2: string) {
    document.documentElement.style.setProperty('--main-color', color);
    document.documentElement.style.setProperty('--sec-color', color2);
    const lightColor = this.primarylight(color);
    document.documentElement.style.setProperty('--main-light-color', lightColor);
    const successColor = this.sucessColor(lightColor);
    document.documentElement.style.setProperty('--success-color', successColor);
  }

  private primarylight(baseColor: string): string {
    const r = parseInt(baseColor.slice(1, 3), 16);
    const g = parseInt(baseColor.slice(3, 5), 16);
    const b = parseInt(baseColor.slice(5, 7), 16);
    
    const newR = Math.min(255, Math.max(0, r + 26));
    const newG = Math.min(255, Math.max(0, g + 53));
    const newB = Math.min(255, Math.max(0, b + 24));
    
    return `#${newR.toString(16).padStart(2, '0')}${newG.toString(16).padStart(2, '0')}${newB.toString(16).padStart(2, '0')}`;
  }

  private sucessColor(baseColor: string): string {
    const r = parseInt(baseColor.slice(1, 3), 16);
    const g = parseInt(baseColor.slice(3, 5), 16);
    const b = parseInt(baseColor.slice(5, 7), 16);
    
    const newR = Math.min(255, Math.max(0, r + 2));
    const newG = Math.min(255, Math.max(0, g + 6));
    const newB = Math.min(255, Math.max(0, b + 3));
    
    return `#${newR.toString(16).padStart(2, '0')}${newG.toString(16).padStart(2, '0')}${newB.toString(16).padStart(2, '0')}`;
  }

  private checkTenantExistence(tenantId: number): Observable<boolean> {
    const headers = new HttpHeaders().set('Tenant-ID', tenantId.toString());
  
    return this.http.get<any>('/gateway/Admin/validate', { headers }).pipe(
      catchError((error) => {
        if (error.status === 404) {
          return of(false);
        }
        return of(false);
      }),
      map((response) => {
        return response ? true : false;
      })
    );
  }
}