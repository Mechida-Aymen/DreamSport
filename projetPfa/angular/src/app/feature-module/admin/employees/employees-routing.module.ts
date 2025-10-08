import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeesComponent } from './employees.component';

const routes: Routes = [{ path: '', component: EmployeesComponent }, { path: 'update-employee', loadChildren: () => import('./update-employee/update-employee.module').then(m => m.UpdateEmployeeModule) }, { path: 'add-employee', loadChildren: () => import('./add-employee/add-employee.module').then(m => m.AddEmployeeModule) }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeesRoutingModule { }
