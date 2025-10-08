import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserBookingsComponent } from './user-bookings.component';
import { TenantGuard } from 'src/app/core/guard/tenant/tenant.guard';

const routes: Routes = [{ path: '', component: UserBookingsComponent,
children: [
  {
    path: 'details',
    loadChildren: () =>
      import('./details/details.module').then(
        (m) => (m).DetailsModule
      ),
      canActivate: [TenantGuard]
      
  },
  {
    path: 'timedate/:id',
    loadChildren: () =>
      import('./timedate/timedate.module').then(
        (m) => (m).CoachTimedateModule
      ),
      canActivate: [TenantGuard]
  }

]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserBookingsRoutingModule { }
