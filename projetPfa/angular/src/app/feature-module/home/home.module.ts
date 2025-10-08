import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { sharedModule} from 'src/app/shared/shared.index';
import { FeatherIconModule } from 'src/app/shared/model/feather.module';
import { TenantModule } from 'src/app/core/store/tenant/tenant.module';  // Ajouter le store Tenant


@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    sharedModule,
    FeatherIconModule,
    TenantModule
  ]
})
export class HomeModule { }
