import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserBookingsRoutingModule } from './user-bookings-routing.module';
import { UserBookingsComponent } from './user-bookings.component';

@NgModule({
  declarations: [UserBookingsComponent],
  imports: [
    CommonModule,
    UserBookingsRoutingModule,
  ],
})
export class UserBookingsModule {}