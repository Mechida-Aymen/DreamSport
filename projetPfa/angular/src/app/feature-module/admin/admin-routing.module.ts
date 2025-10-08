import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { TenantGuard } from 'src/app/core/guard/tenant/tenant.guard';
import { AuthGuard } from 'src/app/core/core.index';
import { RoleGuard } from 'src/app/core/guard/auth/role.guard';

const routes: Routes = [
  { path: '', 
    component: AdminComponent,
    children: [
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./admin-dashboard/admin-dashboard.module').then(
            (m) => m.AdminDashboardModule,
          ),
      },
      {
        path: 'employees',
        loadChildren: () =>
          import('./employees/employees.module').then(
            (m) => m.EmployeesModule
          ),
        canActivate: [TenantGuard, AuthGuard, RoleGuard],
        data: { roles: ['Admin'] }
      },
      {
        path: 'site',
        loadChildren: () =>
          import('./site/site.module').then(
            (m) => m.SiteModule
          ),
      },  
      {
        path: 'annonces',
        loadChildren: () =>
          import('./annonces/annonces.module').then(
            (m) => m.AnnoncesModule
          ),
      },  
      {
        path: 'faqs',
        loadChildren: () =>
          import('./faqs/faqs.module').then(
            (m) => m.FAQsModule
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
        path: 'courts',
        loadChildren: () =>
          import('./courts/courts.module').then(
            (m) => m.CourtsModule
          ),
      },
    ]
  },
  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
