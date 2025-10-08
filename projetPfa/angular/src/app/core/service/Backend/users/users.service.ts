import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { userRes } from 'src/app/core/models/Users/userRes';
import { changePassword } from 'src/app/core/models/Users/chnagePassword';
import { User } from 'src/app/core/models/user.model';
export interface userBlock{
  id: number,
  firstName: string,
  lastName: string,
  email: string,
  imageUrl: string,
  username: string,
  isBlocked: boolean,
  phoneNumber: string
}
interface BackendUser {
  id: number;
  prenom: string;
  nom: string;
  email: string;
  imageUrl: string;
  username: string;
  phoneNumber: string;
  bio?:string;
}
@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private apiUrl =environment.apiUrl+"/users";
  private httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

  constructor(private http: HttpClient) { }

  // Get a single user by ID
    getUser(id: number): Observable<userRes> {
      const url = `${this.apiUrl}/get-right/${id}`;
      return this.http.get<userRes>(url);
    }
    getUsers(skip: number, limit: number, isBlocked?: boolean, searchTerm?: string): Observable<{ users: userBlock[]; totalCount: number }> {
      const url = `${this.apiUrl}/pagination`;
      
      // Construct the request body
      const body: any = {
        skip,
        limit,
      };
    
      // Add optional parameters if defined
      if (isBlocked !== undefined) {
        body.isBlocked = isBlocked;
      }
      if (searchTerm) {
        body.searchTerm = searchTerm;
      }
    
      return this.http.post<{ users: userBlock[]; totalCount: number }>(url, body);
    }

    updateUserStatus(userId: number, isBlocked: boolean): Observable<any> {
      return this.http.put(`${this.apiUrl}/${userId}/status`, { isBlocked });
    }


    deleteUser(userId: number) {
      return this.http.delete(`${this.apiUrl}/${userId}`);
    }

    chnagePassword(user:changePassword): Observable<any> {
        const url = `${this.apiUrl}/changePassworduser`;
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

    geetUser(id: number): Observable<User> {
      const url = `${this.apiUrl}/get/${id}`;
      return this.http.get<BackendUser>(url).pipe(
        map(backendUser => this.mapToFrontend(backendUser))
      );
    }

    updateUser(user:User): Observable<any>{
      const backendUser = this.mapToBackend(user);
      return this.http.put(this.apiUrl, user, this.httpOptions);
    }


    private mapToFrontend(backendUser: BackendUser): User {

      return {
        id: backendUser.id,
        firstName: backendUser.prenom,
        lastName: backendUser.nom,
        email: backendUser.email,
        imageUrl: backendUser.imageUrl,
        username: backendUser.username,
        phoneNumber: backendUser.phoneNumber,
        isBlocked:false,
        Bio:backendUser.bio
        // Map other fields as needed
      };
    }
  
    private mapToBackend(frontendUser: User): BackendUser {
      return {
        id: frontendUser.id,
        prenom: frontendUser.firstName,
        nom: frontendUser.lastName,
        email: frontendUser.email,
        imageUrl: frontendUser.imageUrl,
        username: frontendUser.username,
        phoneNumber: frontendUser.phoneNumber
        // Map other fields as needed
      };
    }
}

