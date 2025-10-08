import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AddInvMemberDto } from 'src/app/core/models/add-inv-member-dto';
import { MemberInvitationDTOO } from 'src/app/core/models/member-invitation-dto';
import { catchError, forkJoin, map, Observable, of, switchMap, tap, throwError } from 'rxjs';
import { UserService } from '../user/user.service';
import { SendTeamInvitationDTO, TeamInvitationDTO } from '../../models/TeamInvitationDTO.model';
import { EquipeService } from '../equipe/equipe.service';
@Injectable({
  providedIn: 'root'
})
export class InvitationService {
  private baseUrl = `${environment.apiUrl}/invitationMember`;
  private baseUrlTeam = `${environment.apiUrl}/invitationTeam`;


  constructor(private http: HttpClient , private userService:UserService , private teamService :EquipeService) { }

  sendInvitation(invitation: AddInvMemberDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/send`, invitation);
  }
  // Dans InvitationService
  checkFriendship(idMember1: number, idMember2: number): Observable<boolean> {
    return this.http.get<boolean>(`${environment.apiUrl}/chatamis/isAmisChat/${idMember1}/${idMember2}`);
  }

  acceptInvitation(invitationId: number): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/Accepter/${invitationId}`,
      {},
      { observe: 'response' }
    ).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.status === 404) {
        errorMessage = 'Invitation not found';
      } else if (error.status === 400) {
        errorMessage = 'Invalid request';
      } else if (error.status >= 500) {
        errorMessage = 'Server error occurred';
      }
      
      // Use server message if available
      if (error.error?.message) {
        errorMessage = error.error.message;
      }
    }
    
    return throwError(errorMessage);
  }

  refuseInvitation(invitationId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Refuser/${invitationId}`);
  }

  // Dans votre InvitationService
  getUserInvitations(userId: number): Observable<{invitations: MemberInvitationDTOO[], totalData: number}> {
   
    
    return this.http.get<{invitations: any[], totalData: number}>(`${this.baseUrl}/user-invitations/${userId}`)
      .pipe(
        switchMap(response => {
          // Pour chaque invitation, récupérer les infos de l'émetteur
          const invitationsWithDetails$ = response.invitations.map(invitation => 
            this.userService.getUser(invitation.emetteur).pipe(
              map(user => ({
                ...invitation,
                emetteur: {
                  id: user.id,
                  firstName: user.prenom || 'Unknown',
                  lastName: user.nom || 'User',
                  username: user.username || '',
                  imageUrl: user.imageUrl || 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg',
                  bio:user.bio 
                }
              } as MemberInvitationDTOO)),
              catchError(() => of({
                ...invitation,
                emetteur: {
                  id: invitation.emetteur,
                  firstName: 'Unknown',
                  lastName: 'User',
                  username: '',
                  imageUrl: 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg'
                }
              } as MemberInvitationDTOO))
            )
          );
          
          return forkJoin(invitationsWithDetails$).pipe(
            map(invitations => ({
              invitations,
              totalData: response.totalData
            }))
          );
        })
      );
  }

  getInvitationById(id: number): Observable<MemberInvitationDTOO> {
    return this.http.get<MemberInvitationDTOO>(`${this.baseUrl}/Get/${id}`);
  }

  getTeamInvitations(userId: number): Observable<{invitations: TeamInvitationDTO[], totalData: number}> {
    return this.http.get<{invitations: any[], totalData: number}>(`${this.baseUrlTeam}/team-invitations/${userId}`)
      .pipe(
        switchMap(response => {
          const invitationsWithDetails$ = response.invitations.map(invitation => {
            // Extraction de l'ID d'équipe - adapté à la structure de l'API
            const teamId = invitation.emetteur;
            
            if (!teamId) {
              console.warn('Invalid team ID in invitation:', invitation);
              return of(this.createFallbackTeamInvitation(invitation));
            }
            console.warn('e   e  team ID in invitation:', invitation);


            return this.teamService.getTeamDetails(teamId).pipe(
              map(team => ({
                id: invitation.id,
                invitation: {
                  id: team.id,
                  name: team.name || 'Unknown Team',
                  description: team.description || 'No description available',
                  avatar: team.avatar || 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg'
                },
                recepteur: invitation.recerpteur, 
                adminId: invitation.adminId
              } as TeamInvitationDTO),
              catchError(error => {
                console.error('Error fetching team details:', error);
                return of(this.createFallbackTeamInvitation(invitation));
              })
            ));
          });
          
          return forkJoin(invitationsWithDetails$).pipe(
            map(invitations => ({
              invitations: invitations.filter(i => i !== null),
              totalData: response.totalData
            }))
          );
        }),
        catchError(error => {
          console.error('Error fetching team invitations:', error);
          return of({ invitations: [], totalData: 0 });
        })
      );
  }

  private createFallbackTeamInvitation(invitation: any): TeamInvitationDTO {
    return {
      id: invitation.id || 0,
      invitation: {
        id: invitation.emetteur || 0,
        name: 'Unknown Team',
        description: 'Team details unavailable',
        avatar: 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg'
      },
      recepteur: invitation.recerpteur, // Adaptation à l'orthographe de l'API
      adminId: invitation.adminId
    };
  }


  acceptTeamInvitation(invitationId: number): Observable<any> {
    return this.http.post(`${this.baseUrlTeam}/accepter/${invitationId}`, {});
  }


  refuseTeamInvitation(invitationId: number): Observable<any> {
    return this.http.delete(`${this.baseUrlTeam}/Refuser/${invitationId}`);
  }

  sendTeamInvitation(invitation: SendTeamInvitationDTO): Observable<any> {

    return this.http.post(`${this.baseUrlTeam}`, invitation);
  }


}