import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoachRequestRoutingModule } from './coach-request-routing.module';
import { CoachRequestComponent } from './coach-request.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CoachRequestComponent
  ],
  imports: [
    CommonModule,
    CoachRequestRoutingModule,
    sharedModule
  ]
})
export class CoachRequestModule { }
