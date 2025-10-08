import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FAQsRoutingModule } from './faqs-routing.module';
import { FAQsComponent } from './faqs.component';
import { MatSortModule } from '@angular/material/sort';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    FAQsComponent
  ],
  imports: [
    CommonModule,
    FAQsRoutingModule,
    MatSortModule,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class FAQsModule { }
