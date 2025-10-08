import { Component, OnInit , NgZone, ViewEncapsulation} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';
import { UserType } from '../../../core/contantes/UserType';
import { Router } from '@angular/router';
import { SpinnerService } from 'src/app/core/core.index';


declare const FB: any;
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None  // Ajoute ceci pour éviter l'encapsulation qui casse la cascade

})
export class LoginComponent implements OnInit {
  public routes = routes;
  public show_password = true;
  public show_password1 =true;
  public show_password_admin = true;
  public user_error = null;
  public employee_error = null;
  public admin_error = null;

  

  tenantData$: Observable<any>;
  imageUrl: string | null = null;

 

  form = new FormGroup({
    email: new FormControl('', [
      Validators.email,
      Validators.required,
    ]),
    password: new FormControl('', [Validators.required]),
  });

   // Separate form for admin
   adminForm = new FormGroup({
    login: new FormControl('', [
      Validators.required,
      // Add any other username validators you need (e.g., minLength, pattern)
    ]),
    password: new FormControl('', [
      Validators.required
    ]),
  });

  get f() {
    return this.form.controls;
  }

  constructor(private auth: AuthService ,private store: Store,private _ngZone: NgZone, 
    private router: Router, public spinner: SpinnerService) {
    this.tenantData$ = this.store.select(selectTenantData);
    this.tenantData$.subscribe(data => {
      if (data && data.siteInfo && data.siteInfo.length > 0) {
        this.imageUrl = data.siteInfo[0].logo;
      }
    });     
  }

  user() {
    if (this.form.valid) {
      this.spinner.show();
      const email = this.form.value.email || '';
      const password = this.form.value.password || '';
      this.auth.login(email, password, UserType.CLIENT).subscribe(
        (response) => {
          console.log("Login Successful", response);
        },
        (error) => {
          this.user_error=error.error;
        }
      );;  // Example API call
    } else {
      this.form.markAllAsTouched();
    }
  }

  employee() {
    if (this.form.valid) {
      this.spinner.show();
      const email = this.form.value.email || '';
      const password = this.form.value.password || '';
      this.auth.login(email, password, UserType.EMPLOYEE).subscribe(
        (response) => {
          console.log("Login Successful", response);
        },
        (error) => {
          this.employee_error=error.error;
        }
      );;  // Example API call;
    } else {
      this.form.markAllAsTouched();
    }
  }
  
  admin() {
    if (this.adminForm.valid) {
      this.spinner.show();
      const email = this.adminForm.value.login || '';
      const password = this.adminForm.value.password || '';
      this.auth.login(email, password, UserType.ADMIN).subscribe(
        (response) => {
          console.log("Login Successful", response);
        },
        (error) => {
          this.admin_error=error.error;
          console.warn("hsant", error.error);
        }
      );;  // Example API call;;
    } else {
      this.adminForm.markAllAsTouched();
    }
  }
  
  togglePassword() {
    this.show_password = !this.show_password;
    this.show_password1 =!this.show_password1;
  }
  

 
  async facebookLogin() {
    console.warn("haha")
    FB.login(async (response: any) => {
      if (response.authResponse) {
        this.auth.LoginWithFacebook(response.authResponse.accessToken)
          .subscribe(
            (res: any) => {
              this._ngZone.run(() => {
                console.warn(response.authResponse);
                
              });
            },
            (error: any) => {
              console.error('Facebook login error:', error);
            }
          );
      } else {
        console.error('User canceled login or did not fully authorize.');
      }
    }, { scope: 'email' });
  }
  ngOnInit(): void {
    if (localStorage.getItem('authenticated')) {
      localStorage.removeItem('authenticated');
    }// need to be removed
    this.auth.initializeGoogleSignIn();
    
  }
  
  Googlelogin(): void {
    this.auth.GoogleSignIn();
  }
  
  // Add to your component
forgotPasswordForm = new FormGroup({
  email: new FormControl('', [Validators.required, Validators.email])
});

currentUserType: any;
lastPasswordResetTime: number | null = null;
resetCooldown = 60; // 60 seconds cooldown
timeRemaining = 0;
showSuccessMessage = false;
successEmail :any;

  setUserType(userType: string) {
    this.currentUserType = userType;
    
    // Check if cooldown is active
    if (this.lastPasswordResetTime) {
      const secondsSinceLastRequest = Math.floor((Date.now() - this.lastPasswordResetTime) / 1000);
      if (secondsSinceLastRequest < this.resetCooldown) {
        this.timeRemaining = this.resetCooldown - secondsSinceLastRequest;
        this.startCooldownTimer();
        return;
      }
    }
  }

  startCooldownTimer() {
    const timer = setInterval(() => {
      this.timeRemaining--;
      
      if (this.timeRemaining <= 0) {
        clearInterval(timer);
      }
    }, 1000);
  } 

onForgotPasswordSubmit() {
  if (this.timeRemaining > 0) {
    return; // Prevent submission during cooldown
  }
  if (this.forgotPasswordForm.valid && this.currentUserType) {
    this.spinner.show();
    const email = this.forgotPasswordForm.value.email;
    
    this.auth.forgotPassword(email, this.currentUserType).subscribe({
      next: () => {
        this.spinner.hide();
        this.lastPasswordResetTime = Date.now();
        this.timeRemaining = this.resetCooldown;
        this.startCooldownTimer();
        this.successEmail = email;
        this.showSuccessMessage = true;
        // Close modal
        const modal = document.getElementById('forgotPasswordModal');
        
        // Reset form
        this.forgotPasswordForm.reset();
      },
      error: (error) => {
        this.spinner.hide();
        console.error('Password reset failed:', error);
      }
    });
  }
}

}
