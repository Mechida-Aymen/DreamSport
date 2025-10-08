import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PagesComponent } from './pages.component';

const routes: Routes = [
  {
    path: '',
    component: PagesComponent,
    children: [

      {
        path: 'requests',
        loadChildren: () =>
          import('./coach-request/coach-request.module').then(
            (m) => m.CoachRequestModule
          ),
      },
      {
        path: 'courts',
        loadChildren: () =>
          import('./coach-courts/coach-courts.module').then(
            (m) => m.CoachCourtsModule
          ),
      },
      
      {
        path: 'profile-settings',
        loadChildren: () =>
          import('./profile-settings/profile-settings.module').then(
            (m) => m.ProfileSettingsModule
          ),
      },
      {
        path: 'users',
        loadChildren: () =>
          import('./users/users.module').then(
            (m) => m.UsersModule
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {}
