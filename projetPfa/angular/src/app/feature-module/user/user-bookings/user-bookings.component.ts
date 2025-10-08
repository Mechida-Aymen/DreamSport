import { Component } from '@angular/core';
import { routes } from 'src/app/core/core.index';

@Component({
  selector: 'app-user-bookings',
  templateUrl: './user-bookings.component.html',
  styleUrl: './user-bookings.component.scss'
})
export class UserBookingsComponent {
public routes = routes;

}
