import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoachesRoutingModule } from './coaches-routing.module';
import { CoachesComponent } from './coaches.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CoachesComponent,
  ],
  imports: [
    CommonModule,
    CoachesRoutingModule,
    sharedModule
  ]
})
export class CoachesModule { }
