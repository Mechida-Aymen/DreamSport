import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { Annonce } from 'src/app/core/models/Site/Annonce';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnnoncesService {
private apiUrl =environment.apiUrl+"/annonces";
  

  constructor(private http: HttpClient) { }

   // HTTP Options
   private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  // Get all Faqs
    getAnnonces(): Observable<Annonce[]> {
      const url = this.apiUrl ;
      return this.http.get<Annonce[]>(url);
    }
  
    // Get a single user by ID
    getAnnonce(id: number): Observable<Annonce> {
      const url = `${this.apiUrl}/${id}`;
      return this.http.get<Annonce>(url);
    }

    // Create a new Annonce
      createAnnonce(faq: any): Observable<Annonce> {
        return this.http.post<Annonce>(this.apiUrl, faq, this.httpOptions).pipe(
          catchError(error => {
            if (error.status === 400 && error.error.errors) {
              // Return the error object with the validation errors
              throw error;
            } else {
              console.error("Error creating FAQ:", error);
              let errorMessage = error.error?.message || error.message || 'Unknown error';
              throw new Error(errorMessage);
            }
          })
        );
      }

 
}
