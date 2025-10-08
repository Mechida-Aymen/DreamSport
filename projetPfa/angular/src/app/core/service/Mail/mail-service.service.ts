import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';
import { Store } from '@ngrx/store';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class MailServiceService {
  private apiUrl =environment.apiUrl+'/Mail/send'; // URL de l'API
  mail='';
  tenantData$: Observable<any>;

  constructor(private http: HttpClient,private store: Store) { 

    this.tenantData$ = this.store.select(selectTenantData);
    this.tenantData$.subscribe(data => {
      if (data && data.siteInfo && data.siteInfo.length > 0) {
        this.mail = data.siteInfo[0].email;  
      }
    });      
  }

  sendEmail(contactForm: any, tenantId: number): Observable<any> {
    const emailRequest = {
      ToEmail: this.mail,
      Subject: contactForm.subject,
      Body: `First Name: ${contactForm.firstName}\nLast Name: ${contactForm.lastName}\nPhone: ${contactForm.phone}\nComments: ${contactForm.comments}`
    };
  
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Tenant-ID': tenantId 
    });

    console.error(tenantId);
    console.error(this.mail);

    return this.http.post(this.apiUrl, emailRequest, { headers });
  }
}
