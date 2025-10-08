import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { Faq } from 'src/app/core/models/Site/Faq';
import { FaqAdd } from 'src/app/core/models/Site/FaqAdd';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FaqsService {
private apiUrl =environment.apiUrl+"/faq";
  

  constructor(private http: HttpClient) { }

   // HTTP Options
   private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  // Get all Faqs
    getFaqs(): Observable<Faq[]> {
      const url = this.apiUrl ;
      return this.http.get<Faq[]>(url);
    }
  
    // Get a single user by ID
    getFaq(id: number): Observable<Faq> {
      const url = `${this.apiUrl}/${id}`;
      return this.http.get<Faq>(url);
    }

    // Create a new faq
      createFaq(faq: FaqAdd): Observable<Faq> {
        return this.http.post<Faq>(this.apiUrl, faq, this.httpOptions).pipe(
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
    // Delete an FAQ
  deleteFaq(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.delete(url, this.httpOptions);
  }
}
