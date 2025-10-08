import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeatureModuleRoutingModule } from './feature-module-routing.module';
import { FeatureModuleComponent } from './feature-module.component';
import { HeaderComponent } from './common-component/header/header.component';
import { FooterComponent } from './common-component/footer/footer.component';
import { sharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr'; // Importer ToastrModule
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';

@NgModule({
  declarations: [
    FeatureModuleComponent,
    HeaderComponent,
    FooterComponent,
  ],
  imports: [
    CommonModule,
    FeatureModuleRoutingModule,
    sharedModule,
    FormsModule,
    ToastrModule.forRoot() ,
     ReactiveFormsModule,
        MatOptionModule,
        MatFormFieldModule,  // Nécessaire pour mat-select


    ]
})
export class FeatureModuleModule { }