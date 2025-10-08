import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OtherSettingsRoutingModule } from './other-settings-routing.module';
import { OtherSettingsComponent } from './other-settings.component';


@NgModule({
  declarations: [
    OtherSettingsComponent
  ],
  imports: [
    CommonModule,
    OtherSettingsRoutingModule
  ]
})
export class OtherSettingsModule { }
