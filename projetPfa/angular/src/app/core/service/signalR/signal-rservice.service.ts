import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { MemberInvitationDTOO } from 'src/app/core/models/member-invitation-dto';
import { Subject } from 'rxjs';
import { AuthService } from '../auth/authservice';
import { UserService } from '../user/user.service';
import { EquipeService } from '../equipe/equipe.service';
import { TeamInvitationDTO } from '../../models/TeamInvitationDTO.model';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  public invitationReceived = new Subject<MemberInvitationDTOO>();
  private isConnected = false;
  public teamInvitationReceived = new Subject<TeamInvitationDTO>();

  constructor(
    private auth: AuthService,
    private userService: UserService,
    private teamService: EquipeService

  ) {
    this.buildConnection();
    this.startConnection();
  }

  private buildConnection(): void {
    const userId = this.auth.getUserId().toString();
    
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`/invitationHub?userId=${userId}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .build();
  }

  private async startConnection(): Promise<void> {
    try {
      await this.hubConnection.start();
      this.isConnected = true;
      console.log('SignalR connection established');
      this.registerSignalEvents();
    } catch (err) {
      console.error('Error starting connection:', err);
      setTimeout(() => this.startConnection(), 5000); // Reconnexion après 5s
    }
  }

 

  private registerSignalEvents(): void {


    this.hubConnection.on('ReceiveInvitation', (invitation: any) => {
      console.log('SignalR - Raw invitation:', invitation);
      
      // Corrigez le nom de la propriété ici
      const correctedInvitation = {
        ...invitation,
        recepteur: invitation.Recerpteur // Mappez Recerpteur vers recepteur
      };
  
      this.userService.getUser(invitation.Emetteur).subscribe({
        next: (sender) => {
          const completeInvitation: MemberInvitationDTOO = {
            id: correctedInvitation.Id,
            emetteur: {
              id: sender.id,
              firstName: sender.prenom || 'Unknown',
              lastName: sender.nom || 'User',
              username: sender.username || '',
              imageUrl: sender.imageUrl || 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg',
              bio:sender.bio ||''
            },
            recepteur: correctedInvitation.recepteur, // Utilisez la valeur corrigée
            adminId: correctedInvitation.AdminId,
            dateCreated: new Date()
          };
          console.log('SignalR - Processed invitation:', completeInvitation);
          this.invitationReceived.next(completeInvitation);
        },
        error: () => {
          const fallbackInvitation = this.createFallbackInvitation(correctedInvitation);
          this.invitationReceived.next(fallbackInvitation);
        }
      });
    });

    //invitation team ReceiveTeamInvitation

    this.hubConnection.on('ReceiveTeamInvitation', (invitation: any) => {
      console.log('SignalR - Raw team invitation:', invitation);
      
      // Correction du nom des propriétés
      const correctedInvitation = {
        ...invitation,
        recepteur: invitation.Recepteur, // Correction de la casse
        Emetteur: invitation.Emetteur // Conservation de la valeur originale
      };
  
      this.teamService.getTeamDetails(correctedInvitation.Emetteur).subscribe({
        next: (team) => {
          const completeInvitation: TeamInvitationDTO = {
            id: correctedInvitation.Id,
            invitation: {
              id:correctedInvitation.Emetteur ,
              name: team.name || 'Unknown Team',
              description: team.description || 'No description',
              avatar: team.avatar || 'assets/img/default-team.png'
            },
            recepteur: correctedInvitation.Recerpteur,
            adminId: correctedInvitation.AdminId
          };
          console.log('SignalR - Processed team invitation:', completeInvitation);
          this.teamInvitationReceived.next(completeInvitation);
        },
        error: () => {
          const fallbackInvitation = this.createTeamFallbackInvitation(correctedInvitation);
          this.teamInvitationReceived.next(fallbackInvitation);
        }
      });
    });
  }
  
  private createTeamFallbackInvitation(invitation: any): TeamInvitationDTO {
    return {
      id: invitation.Id,
      invitation: {
        id: invitation.Emetteur, // ID de l'équipe
        name: invitation.TeamName || 'Unknown Team',
        description: 'Team information unavailable',
        avatar: 'assets/img/default-team.png'
      },
      recepteur: invitation.recepteur,
      adminId: invitation.AdminId
    };
  }
 

  private createFallbackInvitation(invitation: any): MemberInvitationDTOO {
    return {
      id: invitation.Id,
      emetteur: {
        id: invitation.Emetteur,
        firstName: 'Unknown',
        lastName: 'User',
        username: '',
        imageUrl: 'assets/img/default-avatar.png',
        bio:'',
      },
      recepteur: invitation.Recepteur,
      dateCreated: new Date()
    };
  }

  public getConnectionStatus(): boolean {
    return this.isConnected;
  }
}