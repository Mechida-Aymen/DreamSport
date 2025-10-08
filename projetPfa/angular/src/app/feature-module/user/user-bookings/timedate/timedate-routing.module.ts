import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TimedateComponent } from './timedate.component';

const routes: Routes = [{ path: '', component: TimedateComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TimedateRoutingModule { }
