import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SendInvitationComponent } from './send-invitation.component';

const routes: Routes = [{ path: '', component: SendInvitationComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SendInvitationRoutingModule { }
