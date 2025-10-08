import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { selectTenantData, selectTenantId } from 'src/app/core/store/tenant/tenant.selectors';
import { MailServiceService } from 'src/app/core/service/Mail/mail-service.service'; // Assurez-vous que le chemin est correct
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.scss']
})


export class ContactUsComponent {
  public routes = routes
  tenantData$: Observable<any>;
  tenantId: number | null = null;  // Initialiser tenantId comme number ou null
  contactForm: FormGroup;
  successMessage: string = '';

  
     constructor(private store: Store , private mailService: MailServiceService ,private fb: FormBuilder) {
       this.tenantData$ = this.store.select(selectTenantData);
       this.store.select(selectTenantId).subscribe((tenantId: number | null) => {
        this.tenantId = tenantId;
      });
      this.contactForm = this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phone: ['', Validators.required],
        subject: ['', Validators.required],
        comments: ['', Validators.required]
      });
     }

     ngOnInit(): void {
      
    }
     onSubmit(): void {

      if (this.contactForm.valid) {
        if (this.tenantId !== null) {
        this.mailService.sendEmail(this.contactForm.value,this.tenantId).subscribe(
          response => {
            console.log('Email sent successfully', response);
  
            // Message de succès en anglais
            this.successMessage = 'Email sent successfully!';
  
            // Réinitialiser le formulaire après l'envoi
            this.contactForm.reset();
          },
          error => {
            console.error('Error sending email', error);
  
            // Message d'erreur
            this.successMessage = 'There was an error sending the email.';
          }
        );
         }else {
          console.error('Tenant ID is missing');
         }
      
      
      }else {
        this.successMessage = 'Please fill out all fields correctly.';
      }
    }
    

}
