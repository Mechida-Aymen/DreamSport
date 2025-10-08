import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { selectTenantId } from '../../store/tenant/tenant.selectors';
import { Store } from '@ngrx/store';
import { environment } from 'src/environments/environment';


export interface Terrain {
  id: number;
  title: string;
  description: string;
  image: string;
  terrainStatusId: number;
  idAdmin: number;
  idSport_Categorie: number;
  sport_Categorie: any;
  terrainStatus: any;
}

export interface TerrainStatus {
  id: number;
  libelle: string;
}

export interface SportCategory {
  id: number;
  name: string;
  nombreMax: number;
}

export interface IUpdateStatus {
  id: number;
  statusId: number;
}

@Injectable({
  providedIn: 'root',
})
export class TerrainService {
  private apiUrl = environment.apiUrl+'/terrain';

  constructor(private http: HttpClient) {
 
  }

  getTerrains(): Observable<Terrain[]> {
   

    return this.http.get<Terrain[]>(`${this.apiUrl}`);
  }

  getTerrain(id:number): Observable<Terrain>{
    const url = `${this.apiUrl}/by-id/${id}`;
    return this.http.get<Terrain>(url);
  }

  getTerrainStatuses(): Observable<TerrainStatus[]> {
    
   return this.http.get<TerrainStatus[]>(this.apiUrl+'/terrainstatus');

  }

  getSportCategories(): Observable<SportCategory[]> {
  
    return this.http.get<SportCategory[]>(this.apiUrl+'/SportCategorie/execute');
  }

  // Dans TerrainService
getReservationsForTerrain(terrainId: number): Observable<any[]> {
 
  return this.http.get<any[]>(this.apiUrl+`/reservation/upcoming/${terrainId}`);
}

private httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

deleteCourt(id: number): Observable<any> {
  const url = `${this.apiUrl}/${id}`;
  return this.http.delete(url, this.httpOptions);
}

createCourt(faq: any): Observable<any> {
        return this.http.post<any>(this.apiUrl, faq, this.httpOptions).pipe(
          catchError(error => {
            
              console.error("Error creating Court:", error.error.errorMessage);
              let errorMessage = error.error.errorMessage || 'Unknown error';
              throw new Error(errorMessage);
            
          })
        );
}

updateCourtStatus(id:number,statusId:number): Observable<any>{
   const url = this.apiUrl+"/update-status";
   console.log(url);
   const UpdateStatusDto: IUpdateStatus = {id, statusId};
   console.log("obj : ",UpdateStatusDto);
      return this.http.put<any>(url, UpdateStatusDto, this.httpOptions).pipe(
        catchError(error => {
          console.error('Error updating user:', error);
          throw error;
        })
      );
}

}