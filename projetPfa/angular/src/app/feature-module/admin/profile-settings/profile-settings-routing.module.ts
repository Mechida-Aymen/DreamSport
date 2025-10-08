import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProfileSettingsComponent } from './profile-settings.component';

const routes: Routes = [
  { path: '', 
    component: ProfileSettingsComponent,
    children:[
      {
        path: 'admin-profile',
        loadChildren: () =>
          import('./admin-profile/admin-profile.module').then(
            (m) => m.AdminProfileModule
          ),
      },
      {
        path: 'myprofile',
        loadChildren: () =>
          import('./myprofile/myprofile.module').then(
            (m) => m.MyprofileModule
          ),
      },
      {
        path: 'setting-password',
        loadChildren: () =>
          import('./setting-password/setting-password.module').then(
            (m) => m.SettingPasswordModule
          ),
      },
    ]
  }, 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileSettingsRoutingModule { }
