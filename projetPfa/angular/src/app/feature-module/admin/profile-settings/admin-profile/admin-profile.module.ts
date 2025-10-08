import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminProfileRoutingModule } from './admin-profile-routing.module';
import { AdminProfileComponent } from './admin-profile.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    AdminProfileComponent
  ],
  imports: [
    CommonModule,
    AdminProfileRoutingModule,
    ReactiveFormsModule
  ]
})
export class AdminProfileModule { }
