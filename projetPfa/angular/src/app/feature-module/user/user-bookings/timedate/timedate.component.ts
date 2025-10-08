import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OwlOptions, CarouselComponent } from 'ngx-owl-carousel-o';
import { routes } from 'src/app/core/core.index';
import { TerrainService } from 'src/app/core/service/terrain/terrain.service';
import { ReservationService } from 'src/app/core/service/reservation/reservation.service';
import { format, parseISO, isSameDay } from 'date-fns';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-timedate',
  templateUrl: './timedate.component.html',
  styleUrls: ['./timedate.component.scss'],
})
export class TimedateComponent implements OnInit, AfterViewInit {
  @ViewChild('carousel') carousel!: CarouselComponent;

  public routes = routes;
  public dates: { date: string; day: string; fullDate: Date }[] = [];
  public selectedDate: { date: string; day: string; fullDate: Date } | null = null;
  public selectedTime: string | null = null;
  public selectedTimes: string[] = [];
  public endTime: string = '';
  public totalHours: number = 0;
  public subtotal: number = 0;
  public terrainId: number | null = null;
  public reservations: any[] = [];
  public availableTimes: string[] = [];
  public disabledTimes: string[] = [];
  public errorMessage: string | null = null;
  public userId: string | null = null;
  public isLoading = true;

  public coachTimeDateOptions: OwlOptions = {
    loop: true,
    margin: 24,
    nav: true,
    dots: false,
    autoplay: false,
    smartSpeed: 2000,
    navText: ["<i class='feather icon-chevron-left'></i>", "<i class='feather icon-chevron-right'></i>"],
    responsive: {
      0: {
        items: 1,
      },
      768: {
        items: 1,
      },
      1000: {
        items: 1,
      },
    },
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private terrainService: TerrainService,
    private reservationService: ReservationService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {

    this.isLoading = true; 

    const userString = localStorage.getItem('user_data');
    if (userString) {
      const user = JSON.parse(userString);
      this.userId = user.nameid;
    } else {
      console.warn('No user found in localStorage');
    }

    this.generateDates();
    this.generateAvailableTimes();
    this.selectTodayDate();

    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.terrainId = +id;
        this.loadReservations();
      }else{
        this.isLoading = false; 

      }
    });
  }

  ngAfterViewInit(): void {
    if (this.carousel) {
      this.carousel.translated.subscribe((event) => {
        if (event.startPosition !== undefined) {
          const activeSlideIndex = event.startPosition;
          if (activeSlideIndex >= 0 && activeSlideIndex < this.dates.length) {
            const selectedDate = this.dates[activeSlideIndex];
            if (selectedDate) {
              this.selectDate(selectedDate);
            }
          }
        }
      });
    } else {
      console.error('Carousel is not defined');
    }
  }

  generateDates(): void {
    const today = new Date();
    for (let i = 0; i < 7; i++) {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      this.dates.push({
        date: date.toLocaleDateString('en-US', { day: 'numeric', month: 'short' }),
        day: date.toLocaleDateString('en-US', { weekday: 'long' }),
        fullDate: date,
      });
    }
  }

  generateAvailableTimes(): void {
    for (let hour = 10; hour <= 24; hour++) {

      if(hour===24){
        this.availableTimes.push(`00:00`);
        return;
      }
      this.availableTimes.push(`${hour}:00`);
    }
  }

  selectTodayDate(): void {
    const today = new Date();
    const todayDate = this.dates.find((d) => isSameDay(d.fullDate, today));
    if (todayDate) {
      this.selectDate(todayDate);
    } else {
      console.warn('Today\'s date not found in the dates array');
    }
  }

  updateDisabledTimes(): void {
    this.disabledTimes = [];

    if (!this.selectedDate) {
      console.log('No date selected');
      return;
    }

    const selectedDateISO = format(this.selectedDate.fullDate, 'yyyy-MM-dd');

    this.reservations.forEach((reservation) => {
      const reservationDateISO = format(parseISO(reservation.dateRes), 'yyyy-MM-dd');
      if (reservationDateISO === selectedDateISO) {
        const reservationTime = format(parseISO(reservation.dateRes), 'HH:mm');
        console.log('Reservation time for selected date:', reservationTime);
        this.disabledTimes.push(reservationTime);
      }
    });

    console.log('Disabled times for selected date:', this.disabledTimes);
  }

  loadReservations(): void {
    if (this.terrainId) {
      this.terrainService.getReservationsForTerrain(this.terrainId).subscribe(
        (reservations) => {
          this.reservations = reservations;
          this.updateDisabledTimes();
          this.isLoading = false; 

        },
        (error) => {
          console.error('Error loading reservations:', error);
          this.isLoading = false; 

        }
      );
    } else {
      console.error('Terrain ID is not defined');
      this.isLoading = false; 

    }
  }

  isTimeDisabled(time: string): boolean {
    const isPastTime = this.isPast(time);

    const isReserved = this.disabledTimes.includes(time);

    return isPastTime || isReserved;
  }

  isPast(time: string): boolean {
    if (!this.selectedDate) {
      return false; 
    }

    const selectedDateTime = new Date(this.selectedDate.fullDate);
    const [hours, minutes] = time.split(':').map(Number);
    selectedDateTime.setHours(hours, minutes, 0, 0); 

    const now = new Date();
    return selectedDateTime < now;
  }

  selectDate(date: { date: string; day: string; fullDate: Date }): void {
    if (this.selectedDate && this.selectedDate.fullDate.getTime() === date.fullDate.getTime()) {
      return;
    }

    this.selectedDate = date;
    console.log('Selected date:', date.fullDate);
    this.updateDisabledTimes();
    this.selectedTimes = this.availableTimes;
    this.selectedTime = null;
    this.endTime = '';
    this.totalHours = 0;
  }

  selectTime(time: string): void {
    if (!this.isTimeDisabled(time)) {
      this.selectedTime = time;
      const timeIndex = this.selectedTimes.indexOf(time);
      this.endTime = this.selectedTimes[timeIndex + 1] || '01:00';
      this.totalHours = 1;
      this.subtotal = this.totalHours * 100;
    }
  }

  bookReservation(): void {
    if (!this.selectedDate || !this.selectedTime || !this.terrainId) {
      this.errorMessage = 'Please select a date and time before booking.';
      return;
    }

    const userIdNumber = Number(this.userId);

    const reservationData = {
      dateRes: `${this.selectedDate.fullDate.toISOString().split('T')[0]}T${this.selectedTime}:00`,
      idUtilisateur: userIdNumber,
      idTerrain: this.terrainId,
    };

    this.reservationService.createReservation(reservationData).subscribe(
      (response) => {
        console.log('Reservation successful! Response:', response);
        this.toastr.success('Reservation successful!').onHidden.subscribe(() => {
          this.router.navigate([routes.userBookings]);
        });
      },
      (error) => {
        console.error('Error during reservation:', error);
        this.toastr.error(error.error.message || 'An error occurred while booking.');
      }
    );
  }
}