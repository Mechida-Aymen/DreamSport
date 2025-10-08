import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TeamComponent } from './team.component';

const routes: Routes = [{ path: '', component: TeamComponent }, 
  { path: 'capitaine', loadChildren: () => import('./capitaine/capitaine.module').then(m => m.CapitaineModule) }, 
  { path: 'user', loadChildren: () => import('./user/user.module').then(m => m.UserModule) },
  { path: 'send-invitation', loadChildren: () => import('./send-invitation/send-invitation.module').then(m => m.SendInvitationModule) }, 
  { path: 'setting', loadChildren: () => import('./setting/setting.module').then(m => m.SettingModule) }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TeamRoutingModule { }
