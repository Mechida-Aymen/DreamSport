import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogListRoutingModule } from './blog-list-routing.module';
import { BlogListComponent } from './blog-list.component';
import { FeatherIconModule } from 'src/app/shared/model/feather.module';
import { CarouselModule } from 'ngx-owl-carousel-o';


@NgModule({
  declarations: [
    BlogListComponent
  ],
  imports: [
    CommonModule,
    BlogListRoutingModule,
    FeatherIconModule ,
    CarouselModule
  ]
})
export class BlogListModule { }
