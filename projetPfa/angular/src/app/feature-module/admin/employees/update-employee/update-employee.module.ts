import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UpdateEmployeeRoutingModule } from './update-employee-routing.module';
import { UpdateEmployeeComponent } from './update-employee.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    UpdateEmployeeComponent
  ],
  imports: [
    CommonModule,
    UpdateEmployeeRoutingModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    
  ]
})
export class UpdateEmployeeModule { }
