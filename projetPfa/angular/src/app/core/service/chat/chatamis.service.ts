import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AmisChatReturnedDTO, PaginatedResponse } from '../../models/chat/amis-chat-returned.dto';
import { SendAmisMessageDTO } from '../../models/chat/send-amis-message.dto';
import { AmisMessageDTO } from '../../models/chat/amis-message.dto';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ChatAmisService {
  private baseUrl = environment.apiUrl+'/chatamis';

  constructor(private http: HttpClient) {}

  getAmisChatInfo(userId: number): Observable<AmisChatReturnedDTO[]> {
    return this.http.get<AmisChatReturnedDTO[]>(`${this.baseUrl}/${userId}`);
  }

  sendMessage(messageDto: SendAmisMessageDTO): Observable<AmisMessageDTO> {
    return this.http.post<AmisMessageDTO>(`${this.baseUrl}/send`, messageDto);
  }

  getConversation(chatAmisId: number, page: number = 1, pageSize: number = 20): Observable<PaginatedResponse<AmisMessageDTO>> {
    return this.http.get<PaginatedResponse<AmisMessageDTO>>(
      `${this.baseUrl}/${chatAmisId}/conversation`,
      { params: { page: page.toString(), pageSize: pageSize.toString() } }
    );
  }
  markMultipleAsSeen(messageIds: number[], userId: number): Observable<void> {
    const request = {
      messageIds: messageIds,
      userId: userId
    };
  
    return this.http.post<void>(
      environment.apiUrl+`/chatteam/mark-as-seen`,
      request
    );
  }
}