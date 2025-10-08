import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AllCourtsRoutingModule } from './all-courts-routing.module';
import { AllCourtsComponent } from './all-courts.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    AllCourtsComponent
  ],
  imports: [
    CommonModule,
    AllCourtsRoutingModule,
    sharedModule
  ]
})
export class AllCourtsModule { }
