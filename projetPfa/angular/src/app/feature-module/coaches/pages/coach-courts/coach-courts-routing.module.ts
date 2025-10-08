import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoachCourtsComponent } from './coach-courts.component';

const routes: Routes = [
  {
    path: '',
    component: CoachCourtsComponent,
    children: [
      {
        path: 'all-courts',
        loadChildren: () =>
          import('./all-courts/all-courts.module').then(
            (m) => m.AllCourtsModule
          ),
      },
     
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CoachCourtsRoutingModule {}
