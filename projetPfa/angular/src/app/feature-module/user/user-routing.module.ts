import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user.component';
import { TenantGuard } from 'src/app/core/guard/tenant/tenant.guard';

const routes: Routes = [
  {
    path: '',
    component: UserComponent,
    data: { breadcrumbLabel: 'Home' },
    children: [
      {
        path: 'chat',
        loadChildren: () =>
          import('./chat/chat.module').then((m) => m.ChatModule),
        canActivate: [TenantGuard]
      },
      {
        path: 'invitation',
        loadChildren: () =>
          import('./Invitation/Invitation.module').then((m) => m.InvitationModule),
        canActivate: [TenantGuard]
      },
      {
        path: 'team-invitation',
        loadChildren: () =>
          import('./team-invitation/team.module').then((m) => m.TeamModule),
        canActivate: [TenantGuard]
      },
      
      {
        path: 'user-profile',
        loadChildren: () =>
          import('./profile/profile.module').then((m) => m.ProfileModule),
        canActivate: [TenantGuard]
      },
      {
        path: 'change-password',
        loadChildren: () =>
          import('./change-password/change-password.module').then(
            (m) => m.ChangePasswordModule
          ),
          canActivate: [TenantGuard]
      },
      {
        path: 'user-profile-setting',
        loadChildren: () =>
          import('./other-settings/other-settings.module').then(
            (m) => m.OtherSettingsModule
          ),
          canActivate: [TenantGuard]
      },
      {
        path: 'user-bookings',
        loadChildren: () =>
          import('./user-bookings/user-bookings.module').then(
            (m) => m.UserBookingsModule
          ),
          canActivate: [TenantGuard]
      },
      {
        path: 'send-invitation',
        loadChildren: () =>
          import('./send-invitation/send-invitation.module').then(
            (m) => m.SendInvitationModule
          ),
          canActivate: [TenantGuard]
      },
      { path: 'team', loadChildren: () =>
        import('./team/team.module').then(
         (m) => m.TeamModule
       ) },
    ],
  },
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
