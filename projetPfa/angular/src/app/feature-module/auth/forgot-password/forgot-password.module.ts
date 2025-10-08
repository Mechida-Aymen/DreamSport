import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ForgotPasswordRoutingModule } from './forgot-password-routing.module';
import { ForgotPasswordComponent } from './forgot-password.component';
import { sharedModule } from 'src/app/shared/shared.module';
import { FeatherIconModule } from 'src/app/shared/model/feather.module';


@NgModule({
  declarations: [
    ForgotPasswordComponent
  ],
  imports: [
    CommonModule,
    ForgotPasswordRoutingModule,
    sharedModule,
    FeatherIconModule
  ]
})
export class ForgotPasswordModule { }
