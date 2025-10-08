import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, tap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BlockService {
  private baseUrl = environment.apiUrl+'/block';

  constructor(private http: HttpClient) {}

  blockUser(userIdToBlock: number, currentUserId: number): Observable<void> {
    const body = {
      userIdToBlock: userIdToBlock,
      currentUserId: currentUserId,
    };
    
    return this.http.post<void>(
      `${this.baseUrl}/block`, // URL corrigée
      body
    );
  }

  unblockUser(userIdToUnblock: number, currentUserId: number): Observable<void> {
    const body = {
      userIdToUnblock: userIdToUnblock,
      currentUserId: currentUserId
    };
    const url = `${this.baseUrl}/unblock`;
    
    return this.http.post<void>(url, body);
  }

  getBlockedUsers(currentUserId: number): Observable<number[]> {
    return this.http.get<number[]>(
      `${this.baseUrl}/blocked/${currentUserId}`
    );
  }

  isUserBlocked(targetUserId: number, currentUserId: number): Observable<boolean> {
    return this.http.get<boolean>(
      `${this.baseUrl}/is-blocked/${targetUserId}/${currentUserId}`
    );
  }

  getBlockStatusBetweenUsers(currentUserId: number, targetUserId: number): Observable<{
    currentUserBlockedTarget: boolean,
    targetUserBlockedCurrent: boolean
  }> {
    return this.http.get<{
      user1BlockedUser2: boolean,
      user2BlockedUser1: boolean
    }>(`${this.baseUrl}/status-between/${targetUserId}/${currentUserId}`)  
      .pipe(
        map(response => ({
          currentUserBlockedTarget: response.user2BlockedUser1,  
          targetUserBlockedCurrent: response.user1BlockedUser2   
        }))
      );
  }
}