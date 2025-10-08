import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TimedateRoutingModule } from './timedate-routing.module';
import { TimedateComponent } from './timedate.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    TimedateComponent
  ],
  imports: [
    CommonModule,
    TimedateRoutingModule,
    sharedModule
  ]
})
export class CoachTimedateModule { }
