import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CourtsRoutingModule } from './courts-routing.module';
import { CourtsComponent } from './courts.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CourtsComponent
  ],
  imports: [
    CommonModule,
    CourtsRoutingModule,
    sharedModule
  ]
})
export class CourtsModule { }
