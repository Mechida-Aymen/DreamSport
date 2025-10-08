// equipe.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CloudflareService } from '../Cloudflare/cloudflare.service';
import { environment } from 'src/environments/environment';

interface ChangerCapitaineEquipeDTO {
  idEquipe: number;
  idCapitain: number;
}


@Injectable({
  providedIn: 'root'
})
export class EquipeService {
  private apiUrl = environment.apiUrl;
  
  public teamid:any ;

  constructor(private http: HttpClient , private cloudflareService:CloudflareService) { }

  checkMembership(userId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/equipe/check-membership/${userId}`);
    
  }

  updateTeamDetails(teamData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/equipe`, teamData);
  }

  getTeamDetails(teamId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/equipe/get/${teamId}`);
  }

  removeMember(teamId: number, memberId: number): Observable<any> {
    const deleteMemberDTO = {
      UserId: memberId,
      EquipeId: teamId
    };
    
    return this.http.delete(`${this.apiUrl}/members`, {
      body: deleteMemberDTO
    });
  }

  createTeam(teamData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/equipe`, teamData);
  }

  transferCaptaincy(transferDto: ChangerCapitaineEquipeDTO): Observable<any> {
    return this.http.put(`${this.apiUrl}/equipe/ChangerCapitaine`, transferDto);
  }

  deleteTeam(teamId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/equipe/${teamId}`);
  }

  capitaineQuitteEquipe(equipeId: number, capitaineId: number): Observable<any> {
    const dto = {
      CapitaineId: capitaineId,
      EquipeId: equipeId
    };
    return this.http.post(`${this.apiUrl}/equipe/capitaine-quitte`, dto);
  }
}