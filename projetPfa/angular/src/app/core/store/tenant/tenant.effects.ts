import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Store } from '@ngrx/store';
import { of, forkJoin } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import * as TenantActions from './tenant.actions';
import { environment } from 'src/environments/environment';


@Injectable()
export class TenantEffects {
  
  constructor(private actions$: Actions, private http: HttpClient, private store: Store) {}

  loadTenantData$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TenantActions.loadTenantData),
      tap((action) => {
        console.log('🔄 Chargement des données pour Tenant ID:', action.tenantId);
        this.store.dispatch(TenantActions.resetTenantData());
      }),
      switchMap((action) => {
        const tenantId = action.tenantId;
        const headers = new HttpHeaders().set('Tenant-Id', tenantId.toString());
        console.log('hahah ',environment.apiUrl);
        return forkJoin({
          siteInfo: this.http.get<any>(environment.apiUrl+'/site', { headers }).pipe(
            catchError((error) => {
              console.warn('⚠️ Erreur Site Info:', error);
              return of(null); // Si l'API échoue, retourne `null` au lieu d'un échec total
            })
          ),
          faq: this.http.get<any>(environment.apiUrl+'/faq', { headers }).pipe(
            catchError((error) => {
              console.warn('⚠️ Erreur FAQ:', error);
              return of(null);
            })
          ),
          annonces: this.http.get<any>(environment.apiUrl+'/annonces', { headers }).pipe(
            catchError((error) => {
              console.warn('⚠️ Erreur Annonces:', error);
              return of(null);
            })
          ),
        }).pipe(
          tap(({ siteInfo, faq, annonces }) => {
            console.log('✅ Données reçues:', { siteInfo, faq, annonces });
          }),
          map(({ siteInfo, faq, annonces }) =>
            TenantActions.loadTenantDataSuccess({ siteInfo, faq, annonces, tenantId })
          ),
          catchError((error) => {
            console.error('🚨 Erreur globale:', error);
            return of(TenantActions.loadTenantDataFailure({ error: error.message }));
          })
        );
      })
    )
  );
}
