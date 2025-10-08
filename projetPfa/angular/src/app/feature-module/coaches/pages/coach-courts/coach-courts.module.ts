import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoachCourtsRoutingModule } from './coach-courts-routing.module';
import { CoachCourtsComponent } from './coach-courts.component';


@NgModule({
  declarations: [
    CoachCourtsComponent
  ],
  imports: [
    CommonModule,
    CoachCourtsRoutingModule
  ]
})
export class CoachCourtsModule { }
