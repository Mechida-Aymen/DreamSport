import { createReducer, on } from '@ngrx/store';
import * as TenantActions from './tenant.actions';

export interface TenantState {
  tenantId: number | null;
  siteInfo: any;
  faq: any;
  annonces: any;
  mainColor: string | null;
  SecandColor: string | null; 
  error: string | null;
}

const initialState: TenantState = {
  tenantId: null,
  siteInfo: null,
  faq: null,
  annonces: null,
  mainColor: null,
  SecandColor: null,
  error: null,
};

export const tenantReducer = createReducer(
  initialState,

  // Lorsqu'on charge un nouveau tenant, on réinitialise les données et met à jour le localStorage
  on(TenantActions.loadTenantData, (state, { tenantId }) => {
    console.log('🔄 Reducer: Changement de Tenant ID détecté:', tenantId);
    localStorage.setItem("Tenant-ID", tenantId.toString());
    return { ...initialState, tenantId };
  }),

  // Si la requête réussit, on met à jour le store avec les données
  on(TenantActions.loadTenantDataSuccess, (state, { siteInfo, faq, annonces, tenantId }) => {
    console.log('✅ Reducer: Données mises à jour pour Tenant ID:', tenantId);
    return { 
      ...state,
      tenantId, 
      siteInfo, 
      faq, 
      annonces, 
      mainColor: siteInfo?.mainColor || '#097E52',
      SecandColor: siteInfo?.SecandColor || '#192335',
      error: null 
    };
  }),

  // Si la requête échoue, on sauvegarde l'erreur
  on(TenantActions.loadTenantDataFailure, (state, { error }) => {
    console.error('🚨 Reducer: Erreur lors du chargement des données:', error);
    return { ...state, error };
  }),

  // Réinitialisation complète des données et suppression du localStorage
  on(TenantActions.resetTenantData, () => {
    console.log('🔄 Reducer: Réinitialisation des données');
    localStorage.removeItem("Tenant-ID");
    return { ...initialState };
  })
);