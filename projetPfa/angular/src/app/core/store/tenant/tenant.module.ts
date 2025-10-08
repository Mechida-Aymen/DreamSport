import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { tenantReducer } from './tenant.reducer';
import { TenantEffects } from './tenant.effects';

@NgModule({
  imports: [
    StoreModule.forFeature('tenant', tenantReducer),
    EffectsModule.forFeature([TenantEffects])
  ]
})
export class TenantModule {}
