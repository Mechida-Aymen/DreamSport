import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProfileSettingsComponent } from './profile-settings.component';

const routes: Routes = [
  {
    path: '',
    component: ProfileSettingsComponent,
    children: [
      {
        path: 'coach-profile',
        loadChildren: () =>
          import('./coach-profile/coach-profile.module').then(
            (m) => m.CoachProfileModule
          ),
      },
      {
        path: 'setting-password',
        loadChildren: () =>
          import('./setting-password/setting-password.module').then(
            (m) => m.SettingPasswordModule
          ),
      },     
      {
        path: 'myprofile',
        loadChildren: () =>
          import('./myprofile/myprofile.module').then((m) => m.MyprofileModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileSettingsRoutingModule {}
