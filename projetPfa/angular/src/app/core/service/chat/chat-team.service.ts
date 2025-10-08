import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TeamChatReturnedDTO } from '../../models/chat/team-chat-returned.dto';
import { TeamMessageDTO } from '../../models/chat/team-message.dto';
import { SendTeamMessageDTO } from '../../models/chat/send-team-message.dto';
import { PaginatedResponse } from '../../models/chat/amis-chat-returned.dto';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatTeamService {
  private baseUrl = environment.apiUrl+'/chatteam';

  constructor(private http: HttpClient) {}

  getTeamChatInfo(teamId: number, memberId: number): Observable<TeamChatReturnedDTO> {
    return this.http.get<TeamChatReturnedDTO>(
      `${this.baseUrl}/${teamId}/members/${memberId}`
    );
  }

  getFullConversation(teamId: number, page: number = 1, pageSize: number = 20): Observable<PaginatedResponse<TeamMessageDTO>> {
    return this.http.get<PaginatedResponse<TeamMessageDTO>>(
      `${this.baseUrl}/${teamId}/conversation`,
      { params: { page: page.toString(), pageSize: pageSize.toString() } }
    );
  }
   
  markMultipleAsSeen(messageIds: number[], userId: number): Observable<void> {
    const request = {
      messageIds: messageIds,
      userId: userId
    };
  
    return this.http.post<void>(
      `${this.baseUrl}/mark-as-seen`,
      request
    );
  }

  sendMessage(messageDto: SendTeamMessageDTO): Observable<TeamMessageDTO> {
    return this.http.post<TeamMessageDTO>(`${this.baseUrl}/send`, messageDto);
  }
}