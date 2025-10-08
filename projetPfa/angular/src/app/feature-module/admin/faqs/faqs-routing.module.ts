import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FAQsComponent } from './faqs.component';

const routes: Routes = [{ path: '', component: FAQsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FAQsRoutingModule { }
