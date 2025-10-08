import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SendInvitationRoutingModule } from './send-invitation-routing.module';
import { SendInvitationComponent } from './send-invitation.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    SendInvitationComponent
  ],
  imports: [
    CommonModule,
    SendInvitationRoutingModule,
    FormsModule
  ]
})
export class SendInvitationModule { }
