import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { sharedModule } from 'src/app/shared/shared.module';



@NgModule({
  declarations: [
    UserComponent,
    
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    sharedModule,
    

  ]
})
export class UserModule { }
