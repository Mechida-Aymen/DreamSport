import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OurTeamsComponent } from './our-teams.component';

const routes: Routes = [{ path: '', component: OurTeamsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OurTeamsRoutingModule { }
