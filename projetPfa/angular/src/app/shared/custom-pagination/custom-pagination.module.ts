import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomPaginationComponent } from './custom-pagination.component';
import { FeatherIconModule } from '../model/feather.module';

@NgModule({
  declarations: [CustomPaginationComponent],
  imports: [CommonModule, FormsModule, ReactiveFormsModule,FeatherIconModule],
  exports: [CustomPaginationComponent],
})
export class CustomPaginationModule {}
