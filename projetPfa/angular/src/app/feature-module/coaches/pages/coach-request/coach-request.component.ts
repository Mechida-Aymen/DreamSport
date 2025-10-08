import { Component } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, map } from 'rxjs';
import { DataService,  } from 'src/app/core/core.index';
import { getReservation } from 'src/app/core/models/reservations/getReservation';
import { UsersService } from 'src/app/core/service/Backend/users/users.service';
import { ReservationService } from 'src/app/core/service/reservation/reservation.service';
import { TerrainService } from 'src/app/core/service/terrain/terrain.service';
import { coachRequest } from 'src/app/shared/model/page.model';

@Component({
  selector: 'app-coach-request',
  templateUrl: './coach-request.component.html',
  styleUrls: ['./coach-request.component.scss'],
})
export class CoachRequestComponent {
  dataSource!: MatTableDataSource<coachRequest>;
  public searchDataValue = '';
  reservations: getReservation[] = [];
  selectedReservation: any;
  isLoading: boolean = true;
  rejectingRes:any;

  constructor(public data: DataService,private reservationService:ReservationService, 
    private userService:UsersService, private terrainService:TerrainService,
  private toastr: ToastrService,)
  {
    
  }

  public sortData(sort: Sort) {
    const data = this.reservations.slice();

    if (!sort.active || sort.direction === '') {
      this.reservations = data;
    } else {
      this.reservations = data.sort((a, b) => {
        const aValue = (a as never)[sort.active];
        const bValue = (b as never)[sort.active];
        return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
      });
    }
  }

  public searchData(value: string): void {
  }

  // In your component

// Example data mapping
ngOnInit() {
  this.loadReservations();
  
}
loadReservations() {
  this.isLoading = true;
  
  this.reservationService.getRequestsList().subscribe({
    next: (reservations) => {
      // Create an array of observables to get user and terrain for each reservation
      const requests = reservations.map(reservation => {

        console.warn(reservations);
        return forkJoin({

          user: this.userService.getUser(reservation.idUtilisateur),
          terrain: this.terrainService.getTerrain(reservation.idTerrain),
        }).pipe(
          map(({user, terrain}) => {
            return {
              ...reservation,
              user: user,
              terrain: terrain,
            };
          })
        );
      });

      console.warn(reservations[0].user);

      // Execute all requests in parallel
      forkJoin(requests).subscribe({
        next: (completeReservations) => {
          this.reservations = completeReservations;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error loading complete reservation data:', err);
          this.isLoading = false;
        }
      });
    },
    error: (err) => {
      console.error('Error loading reservations:', err);
      this.isLoading = false;
    }
  });
  console.log("list : ",this.reservations);
}

getReservations():void{
  this.reservationService.getRequestsList().subscribe({
    next: (reservations) => {
      this.reservations = reservations;
    },
    error: (err) => console.error(err)
  });
}

acceptReservation(id: number) {
  // Implement acceptance logic
}

openRejectModal(reservation: getReservation) {
  this.rejectingRes=reservation;
}

onRejectSubmit(idRes:number){
  this.reservationService.rejectReservation(idRes).subscribe({
    next: (res) => {
      this.toastr.success('success', 'Reservation rejected');
      this.reservations=this.reservations.filter(item => item.id !== idRes);
    },
    error: (err) => this.toastr.error('error', 'Please try agian')
  })
}

onAccepteSubmit(idRes:number){
  this.reservationService.acceptReservation(idRes).subscribe({
    next: (res) => {
      this.toastr.success('success', 'Reservation accepted');
      this.reservations=this.reservations.filter(item => item.id !== idRes);
    },
    error: (err) => this.toastr.error('error', 'Please try agian')
  })
}
}
