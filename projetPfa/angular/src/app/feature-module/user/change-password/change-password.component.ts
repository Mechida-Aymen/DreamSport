import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { routes } from 'src/app/core/core.index';
import { changePassword } from 'src/app/core/models/Users/chnagePassword';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { UsersService } from 'src/app/core/service/Backend/users/users.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent {
  public routes = routes;
    passwordForm: FormGroup;
    isLoading = false;
    oldPasswordError = '';
  
    constructor(
      private fb: FormBuilder,
      private snackBar: MatSnackBar,
      private authService: AuthService,
      private userService: UsersService,
    ) {
      this.passwordForm = this.fb.group({
        oldPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', [Validators.required]]
      }, { validator: this.passwordMatchValidator });
    }
  
    passwordMatchValidator(form: FormGroup) {
      return form.get('newPassword')?.value === form.get('confirmPassword')?.value 
        ? null 
        : { mismatch: true };
    }
  
    onSubmit() {
      if (this.passwordForm.invalid) {
        this.snackBar.open('Please fill all fields correctly', 'Close', { duration: 3000 });
        return;
      }
    
      this.isLoading = true;
      this.oldPasswordError = ''; // Clear previous error
    
      const payload: changePassword = {
        OldPassword: this.passwordForm.value.oldPassword,
        NewPassword: this.passwordForm.value.newPassword,
        EmployerId: this.authService.getUserId()
      };
    
      this.userService.chnagePassword(payload).subscribe({
        next: () => {
          this.snackBar.open('Password changed successfully', 'Close', { duration: 3000 });
          this.passwordForm.reset();
          this.isLoading = false;
        },
        error: (err) => {
          this.isLoading = false;
          console.warn("DAs ",err.status);
          // Handle incorrect old password (400 Bad Request)
          if (err.status === 400) {
            this.oldPasswordError = 'The old password you entered is incorrect';
            this.passwordForm.get('oldPassword')?.setErrors({ incorrect: true });
            this.passwordForm.get('oldPassword')?.markAsTouched();
          } 
          // Handle other errors
          else {
            this.snackBar.open('Failed to change password. Please try again.', 'Close', { duration: 3000 });
          }
        }
      });
    }
  
    onReset() {
      this.passwordForm.reset();
      this.oldPasswordError = '';
    }
}
