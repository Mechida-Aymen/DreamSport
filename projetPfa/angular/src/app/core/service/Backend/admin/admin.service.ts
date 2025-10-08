import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, tap } from 'rxjs';
import { editAdmin } from 'src/app/core/models/admin/editAdmin';
import { changePassword } from 'src/app/core/models/Users/chnagePassword';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
private apiUrl =environment.apiUrl+"/admin";
  

  constructor(private http: HttpClient) { }

   // HTTP Options
   private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  chnagePassword(user:changePassword): Observable<any> {
      const url = `${this.apiUrl}/changeAdminPassword`;
      return this.http.put<any>(url, user, this.httpOptions).pipe(
        catchError(error => {
          if (error.status === 400 ) {
            // Return the error object with the validation errors
            throw error;
          } else {
            let errorMessage = error.error?.message || error.message || 'Unknown error';
            throw new Error(errorMessage);
          }
        })
      );
    }

    getAdmin(): Observable<editAdmin> {
      return this.http.get<editAdmin>(this.apiUrl).pipe(
        tap(response => console.log('API Response:', response)),
        catchError(error => {
          console.error('API Error:', error);
          throw error;
        })
      );
    }

  updateAdmin(admin:editAdmin) {
    return this.http.put<any>(this.apiUrl,admin,this.httpOptions);
  }
}
