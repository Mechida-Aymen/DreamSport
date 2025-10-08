import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgApexchartsModule } from 'ng-apexcharts';
import { DataService } from '../core/core.index';
import { SpinnerInterceptor } from '../core/interceptor/spinner/spinner.interceptor';
import { MaterialModules } from './material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { CustomPaginationModule } from './custom-pagination/custom-pagination.module';
import { MatSortModule } from '@angular/material/sort';
import { NgxbootstrapModule } from 'src/app/shared/ngx-bootstrap/ngxbootstrap.module';
import { CountUpModule } from 'ngx-countup';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { FeatherIconModule } from './model/feather.module';
import { LightboxModule } from 'ngx-lightbox';
import { TimepickerActions, TimepickerModule } from 'ngx-bootstrap/timepicker';
import { DatePipe } from '@angular/common';
import { GoogleMapsModule } from '@angular/google-maps';
import {
  BsDatepickerModule,
  BsDatepickerConfig,
} from 'ngx-bootstrap/datepicker';
import { MatSliderModule } from '@angular/material/slider';
import { TruncatePipe } from './shared.index';

@NgModule({ exports: [
        FeatherIconModule,
        NgApexchartsModule,
        FormsModule,
        ReactiveFormsModule,
        CarouselModule,
        CustomPaginationModule,
        DragDropModule,
        MatSortModule,
        ReactiveFormsModule,
        MaterialModules,
        CountUpModule,
        NgxbootstrapModule,
        LightboxModule,
        TimepickerModule,
        GoogleMapsModule,
        BsDatepickerModule,
        MatSliderModule,
    ], imports: [FeatherIconModule,
        NgApexchartsModule,
        FormsModule,
        ReactiveFormsModule,
        CarouselModule,
        CustomPaginationModule,
        DragDropModule,
        MatSortModule,
        ReactiveFormsModule,
        MaterialModules,
        CountUpModule,
        NgxbootstrapModule,
        LightboxModule,
        TimepickerModule,
        GoogleMapsModule,
        BsDatepickerModule.forRoot(),
        MatSliderModule], providers: [
        DataService,
        DatePipe,
        TimepickerActions,
        BsDatepickerConfig,
        { provide: HTTP_INTERCEPTORS, useClass: SpinnerInterceptor, multi: true },
        provideHttpClient(withInterceptorsFromDi()),
    ] })
export class sharedModule {}
