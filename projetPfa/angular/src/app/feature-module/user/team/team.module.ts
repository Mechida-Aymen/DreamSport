import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TeamRoutingModule } from './team-routing.module';
import { TeamComponent } from './team.component';
import { CapitaineComponent } from './capitaine/capitaine.component';
import { UserComponent } from './user/user.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    TeamComponent,
    CapitaineComponent, 
    UserComponent  
  ],
  imports: [
    CommonModule,
    TeamRoutingModule,
    FormsModule ,
    sharedModule

  ]
})
export class TeamModule { }
