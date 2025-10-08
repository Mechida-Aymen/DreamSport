import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileSettingsRoutingModule } from './profile-settings-routing.module';
import { ProfileSettingsComponent } from './profile-settings.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    ProfileSettingsComponent
  ],
  imports: [
    CommonModule,
    ProfileSettingsRoutingModule,
    sharedModule
  ]
})
export class ProfileSettingsModule { }
