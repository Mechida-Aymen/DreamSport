import { createAction, props } from '@ngrx/store';

// Charger les données du tenant
export const loadTenantData = createAction(
  '[Tenant] Load Tenant Data',
  props<{ tenantId: number }>()
);

// Succès de la récupération des données
export const loadTenantDataSuccess = createAction(
    '[Tenant] Load Tenant Data Success',
    props<{ siteInfo: any; faq: any; annonces: any; tenantId: number }>()
  );
  

// Échec de la récupération des données
export const loadTenantDataFailure = createAction(
  '[Tenant] Load Tenant Data Failure',
  props<{ error: string }>()
);

export const resetTenantData = createAction('[Tenant] Reset Data'); // 🆕 Réinitialisation des données

