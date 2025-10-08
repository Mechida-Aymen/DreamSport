import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { AuthService } from '../auth/authservice';
import { AmisMessageDTO } from '../../models/chat/amis-message.dto';
import { TeamMessageDTO } from '../../models/chat/team-message.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatSignalRService {
  private hubConnection!: signalR.HubConnection;
  public amisMessageReceived = new Subject<AmisMessageDTO>();
  public teamMessageReceived = new Subject<TeamMessageDTO>();
  public messageSeenUpdate = new Subject<{ messageIds: number[], seenByUserId: number }>();
  public typingStatusUpdate = new Subject<{ userId: number, isTyping: boolean, isTeamChat: boolean, chatId: number ,username?:string}>();
  public connectionEstablished = new Subject<boolean>();

  constructor(private auth: AuthService) {
    this.initializeConnection();
  }

  private initializeConnection(): void {
    console.log('[SignalR] Initialisation de la connexion...');
    this.buildConnection();
    this.startConnection();
    this.registerServerEvents();
  }

  private buildConnection(): void {
    const userId = this.auth.getUserId().toString();
    console.log('[SignalR] Construction de la connexion avec userId =', userId);

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`/chatHub?userId=${userId}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        logger: signalR.LogLevel.Trace
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .configureLogging(signalR.LogLevel.Trace)
      .build();
  }

  private startConnection(): void {
    this.hubConnection.start()
      .then(() => {
        console.log('[SignalR] Connexion établie avec succès');
        console.log('[SignalR] État de la connexion:', this.hubConnection.state);
        this.registerServerEvents();
        this.joinGroups();
        this.connectionEstablished.next(true);
      })
      .catch(err => {
        console.error('[SignalR] Erreur lors de la connexion:', err);
        setTimeout(() => this.startConnection(), 5000);
      });
  }

  private joinGroups(): void {
    const userId = this.auth.getUserId();
    console.log('[SignalR] Tentative de rejoindre le groupe avec userId =', userId);

    this.hubConnection.invoke('JoinUserGroup', userId)
      .then(() => console.log('[SignalR] Groupe rejoint avec succès '))
      .catch(err => console.error('[SignalR] Erreur lors de la jointure du groupe:', err));
    

  }
  public joinTeamGroup(teamId: number): Promise<void> {
    console.log('[SignalR] Joining team group:', teamId);
    return this.hubConnection.invoke('JoinTeamGroup', teamId)
        .then(() => console.log('[SignalR] Successfully joined team group'))
        .catch(err => console.error('[SignalR] Error joining team group:', err));
}
 
  private registerServerEvents(): void {
    console.log('[SignalR] Enregistrement des événements Serveur...');

    this.hubConnection.on('ReceiveAmisMessage', (message: AmisMessageDTO) => {
      console.log('[SignalR] AmisMessage reçu :', message);
      this.amisMessageReceived.next(message);
    });

    this.hubConnection.on('ReceiveTeamMessage', (message: TeamMessageDTO) => {
      console.log('[SignalR] TeamMessage reçu :', message);
      this.teamMessageReceived.next(message);
    });

    this.hubConnection.on('MessagesSeen', (messageIds: number[], seenByUserId: number) => {
      console.log('[SignalR] Messages vus :', messageIds, 'par', seenByUserId);
      this.messageSeenUpdate.next({ messageIds, seenByUserId });
    });

    this.hubConnection.on('UserTyping', (userId: number, isTyping: boolean, isTeamChat: boolean, chatId: number , username:string) => {
      console.log('[SignalR] UserTyping reçu => User:', userId, 'Typing:', isTyping, 'TeamChat:', isTeamChat, 'ChatId:', chatId);
      this.typingStatusUpdate.next({ userId, isTyping, isTeamChat, chatId,username });
    });
  }

  public sendAmisMessage(message: AmisMessageDTO): Promise<void> {
    console.log('[SignalR] Envoi d\'un AmisMessage :', message);
    return this.hubConnection.invoke('SendAmisMessage', message)
      .catch(err => console.error('[SignalR] Erreur envoi AmisMessage:', err));
  }

  public sendTeamMessage(message: TeamMessageDTO): Promise<void> {
    console.log('[SignalR] Envoi d\'un TeamMessage :', message);
    return this.hubConnection.invoke('SendTeamMessage', message)
      .catch(err => console.error('[SignalR] Erreur envoi TeamMessage:', err));
  }

  public markMessagesAsSeen(messageIds: number[], userId: number): Promise<void> {
    console.log('[SignalR] Marquage des messages comme vus - IDs:', messageIds, 'par userId:', userId);
    return this.hubConnection.invoke('MarkMessagesAsSeen', messageIds, userId)
      .catch(err => console.error('[SignalR] Erreur marquage messages comme vus:', err));
  }

  

  public notifyTyping(isTyping: boolean, isTeamChat: boolean, chatId: number, receiverId: number,username?:string): Promise<void> {
    const userId = this.auth.getUserId();
    console.log('[SignalR][Typing] Envoi notification typing:', {
        userId,
        isTyping,
        isTeamChat,
        chatId,
        receiverId,
        timestamp: new Date().toISOString(),
        username
    });
    
    return this.hubConnection.invoke('NotifyTyping', isTyping, isTeamChat, chatId, userId, receiverId,username)
        .then(() => {
            console.log('[SignalR][Typing] Notification envoyée avec succès');
        })
        .catch(err => {
            console.error('[SignalR][Typing] Erreur lors de l\'envoi:', err);
            throw err;
        });
}
}
