import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SiteRoutingModule } from './site-routing.module';
import { SiteComponent } from './site.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    SiteComponent
  ],
  imports: [
    CommonModule,
    SiteRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    
  ]
})
export class SiteModule { }
