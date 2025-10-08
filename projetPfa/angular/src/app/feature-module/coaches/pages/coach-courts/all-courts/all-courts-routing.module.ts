import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllCourtsComponent } from './all-courts.component';

const routes: Routes = [{ path: '', component: AllCourtsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AllCourtsRoutingModule { }
