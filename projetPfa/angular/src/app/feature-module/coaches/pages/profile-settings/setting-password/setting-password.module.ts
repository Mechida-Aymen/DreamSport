import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingPasswordRoutingModule } from './setting-password-routing.module';
import { SettingPasswordComponent } from './setting-password.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    SettingPasswordComponent
  ],
  imports: [
    CommonModule,
    SettingPasswordRoutingModule,
    ReactiveFormsModule,
  ]
})
export class SettingPasswordModule { }
