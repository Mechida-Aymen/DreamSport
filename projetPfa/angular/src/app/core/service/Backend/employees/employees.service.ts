import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { employee } from 'src/app/core/models/employee/employee';
import { AddEmployee } from 'src/app/core/models/employee/addEmployee';
import { environment } from '../../../../../environments/environment'; 
import { changePassword } from 'src/app/core/models/Users/chnagePassword';


@Injectable({
  providedIn: 'root'
})
export class EmployeesService {
  private apiUrl =environment.apiUrl+"/employee";
  

  constructor(private http: HttpClient) { }

   // HTTP Options
   private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  // Get all users
  getEmployees(): Observable<employee[]> {
    const url = this.apiUrl + '/admin';
    return this.http.get<employee[]>(url);
  }

  // Get a single user by ID
  getEmployee(id: number): Observable<employee> {
    const url = `${this.apiUrl}/get/${id}`;
    return this.http.get<employee>(url);
  }

  // Create a new user
  createEmployee(employee: AddEmployee): Observable<employee> {
    return this.http.post<employee>(this.apiUrl, employee, this.httpOptions).pipe(
      catchError(error => {
        if (error.status === 400 && error.error.errors) {
          // Return the error object with the validation errors
          throw error;
        } else {
          console.error("Error creating employee:", error);
          let errorMessage = error.error?.message || error.message || 'Unknown error';
          throw new Error(errorMessage);
        }
      })
    );
  }
  // Update an existing user
  updateEmployee( user: employee): Observable<employee> {
    const url = this.apiUrl;
    return this.http.put<employee>(url, user, this.httpOptions).pipe(
      catchError(error => {
        console.error('Error updating user:', error);
        throw error;
      })
    );
  }

  // Delete a user
  deleteEmployee(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete(url, this.httpOptions);
  }

  // Search users by any field (optional)
  searchEmployees(term: string): Observable<employee[]> {
    const url = `${this.apiUrl}/search?q=${term}`;
    return this.http.get<employee[]>(url);
  }

  chnagePassword(user:changePassword): Observable<any> {
    const url = `${this.apiUrl}/changePassword`;
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

  
}
