import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';
import { Router } from '@angular/router';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  public routes = routes;
  public CustomControler!: string | number;
  public isValidConfirmPassword = false;
  public show_password1= true;
  public show_password2 = true;
  public show_password3= true;
  public show_password4 = true;
  public confirmPassword = true;
  tenantData$: Observable<any>;
    imageUrl: string | null = null;
    
  errors: Record<string, string> = {};
    
  form = new FormGroup({
    nom: new FormControl('', Validators.required),
    prenom: new FormControl('', Validators.required),
    username: new FormControl('', [Validators.required]),
    birthday: new FormControl('', [
      Validators.required // Ensures the date format is YYYY-MM-DD
    ]),
    genre: new FormControl('', Validators.required),
    phoneNumber: new FormControl('', Validators.required),
    email: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),
    password: new FormControl('', [Validators.required]),
    passwordConfirmed: new FormControl('', [Validators.required]),
  });

  get f() {
    return this.form.controls;
  }

    constructor(private auth: AuthService ,private store: Store, private router: Router) {
      this.tenantData$ = this.store.select(selectTenantData);
      this.tenantData$.subscribe(data => {
        if (data && data.siteInfo && data.siteInfo.length > 0) {
          this.imageUrl = data.siteInfo[0].logo;
        }
      });     
    }

   
   
  signup() {
   console.error("haha ",this.form.value.password, " haha ",this.form.value.passwordConfirmed, "haha ",this.form);
    if (
      this.form.value.password === this.form.value.passwordConfirmed &&
      this.form.valid
    ) {
      this.confirmPassword = true;
      this.auth.manualSignup(this.form.value).subscribe(
        (response) => {
          console.log('Signup successful:', response);
          // Handle successful signup (redirect to login or home page)
          this.router.navigate([routes.login]);
        },
        (error) => {
          this.errors = error;
          console.error('Signup error:', error);
          // Handle error (e.g., show error message to the user)
        }
      );
    } else{
      this.form.markAllAsTouched();
    }
  }


}
