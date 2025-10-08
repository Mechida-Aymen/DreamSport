import { createFeatureSelector, createSelector } from '@ngrx/store';
import { TenantState } from './tenant.state';

export const selectTenantState = createFeatureSelector<TenantState>('tenant');

export const selectSiteInfo = createSelector(selectTenantState, (state) => state.siteInfo);
export const selectFaq = createSelector(selectTenantState, (state) => state.faq);
export const selectAnnonces = createSelector(selectTenantState, (state) => state.annonces);
export const selectError = createSelector(selectTenantState, (state) => state.error);
export const selectTenantId = createSelector(selectTenantState, (state) => state.tenantId);
export const selectSiteMail = createSelector(selectTenantState, (state) => state.siteInfo.email);

export const selectTenantData = createSelector(
    selectTenantState,
    (state) => {
      console.log('📊 Sélecteur: État actuel du store:', state);
      return state;
    }
  );

