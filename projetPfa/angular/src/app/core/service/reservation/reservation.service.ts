import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { getReservation } from '../../models/reservations/getReservation';
import { AuthService } from '../auth/authservice';

@Injectable({
  providedIn: 'root',
})
export class ReservationService {
  private apiUrl = environment.apiUrl+"/reservation";
 
  constructor(private http: HttpClient,private authService:AuthService) {}

  // Méthode pour effectuer une réservation
  createReservation(reservationData: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.http.post<any>(this.apiUrl, reservationData, { headers });
  }

  getRequestsList(): Observable<getReservation[]>{
    const url = this.apiUrl+"/requests-list";
    return this.http.get<getReservation[]>(url);
  }

  rejectReservation(resiD:number): Observable<any>{
    const url = this.apiUrl+"/update-status";
    const employeeId = this.authService.getUserId(); //need to be extravted from the token
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.http.put<any>(url,{id:resiD , status:'Canceled', employeeId: employeeId}, {headers});
  }

  acceptReservation(resiD:number): Observable<any>{
    const url = this.apiUrl+"/update-status";
    const employeeId = this.authService.getUserId(); //need to be extravted from the token
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.http.put<any>(url,{id:resiD , status:'Confirmed', employeeId: employeeId}, {headers});
  }
  
}