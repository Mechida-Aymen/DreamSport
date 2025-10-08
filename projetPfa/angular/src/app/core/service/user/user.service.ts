// src/app/core/services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface User {
  id: number;
  nom: string;
  prenom: string;
  username: string | null;
  email: string;
  imageUrl: string | null;
  bio?: string;
  sending?: boolean; 
  isMemberOfTeam?: boolean; 
  areFriends?:boolean;
  hasPendingInvitation?:boolean;

}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl+'/users';

  constructor(private http: HttpClient) { }

  searchUsers(query: string,id :number): Observable<any[]> {
    // Encoder le query pour les espaces et caractères spéciaux
    const encodedQuery = encodeURIComponent(query);
    return this.http.get<any[]>(`${this.apiUrl}/search/${encodedQuery}/${id}`);
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/get/${id}`).pipe(
      catchError(error => {
        console.error('Error fetching user:', error);
        return of({
          id: id,
          nom: 'Unknown',
          prenom: 'User',
          username: null,
          email: '',
          imageUrl: null
        } as User);
      })
    );
  }
  

}