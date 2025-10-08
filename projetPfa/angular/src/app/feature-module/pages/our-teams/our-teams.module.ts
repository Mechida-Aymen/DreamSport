import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OurTeamsRoutingModule } from './our-teams-routing.module';
import { OurTeamsComponent } from './our-teams.component';


@NgModule({
  declarations: [
    OurTeamsComponent
  ],
  imports: [
    CommonModule,
    OurTeamsRoutingModule
  ]
})
export class OurTeamsModule { }
