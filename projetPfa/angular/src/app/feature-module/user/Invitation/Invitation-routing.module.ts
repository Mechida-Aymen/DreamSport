import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvitationComponent } from './Invitation.component';

const routes: Routes = [{ path: '', component: InvitationComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvitationRoutingModule { }
