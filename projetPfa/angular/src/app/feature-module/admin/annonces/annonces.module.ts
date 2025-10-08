import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnnoncesRoutingModule } from './annonces-routing.module';
import { AnnoncesComponent } from './annonces.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    AnnoncesComponent
  ],
  imports: [
    CommonModule,
    AnnoncesRoutingModule,
    sharedModule,
  ]
})
export class AnnoncesModule { }
