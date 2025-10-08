import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoachRequestComponent } from './coach-request.component';

const routes: Routes = [{ path: '', component: CoachRequestComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoachRequestRoutingModule { }
