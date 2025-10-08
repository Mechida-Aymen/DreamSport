import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { SiteM } from 'src/app/core/models/Site/siteM';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SiteService {
 private apiUrl =environment.apiUrl+"/site";
  // HTTP Options
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }
  
    
  getSiteSettings():Observable<SiteM>{
    return this.http.get<SiteM>(this.apiUrl);
  }

  updateSiteSettings(user:any):any {
    const url = this.apiUrl;
        return this.http.put<SiteM>(url, user, this.httpOptions).pipe(
          catchError(error => {
            console.error('Error updating user:', error);
            throw error;
          })
        );
  }
  
}
