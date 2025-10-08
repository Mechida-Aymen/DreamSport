import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CapitaineComponent } from './capitaine.component';

const routes: Routes = [{ path: '', component: CapitaineComponent },];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CapitaineRoutingModule { }
