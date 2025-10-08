import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoachProfileRoutingModule } from './coach-profile-routing.module';
import { CoachProfileComponent } from './coach-profile.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CoachProfileComponent,
  ],
  imports: [
    CommonModule,
    CoachProfileRoutingModule,
    sharedModule
  ]
})
export class CoachProfileModule { }
