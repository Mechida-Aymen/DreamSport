import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SettingPasswordComponent } from './setting-password.component';

const routes: Routes = [{ path: '', component: SettingPasswordComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingPasswordRoutingModule { }
