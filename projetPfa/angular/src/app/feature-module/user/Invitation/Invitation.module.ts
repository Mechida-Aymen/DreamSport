import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InvitationRoutingModule } from './Invitation-routing.module';
import { InvitationComponent } from './Invitation.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    InvitationComponent
  ],
  imports: [
    CommonModule,
    InvitationRoutingModule,
    sharedModule
  ]
})
export class InvitationModule { }
