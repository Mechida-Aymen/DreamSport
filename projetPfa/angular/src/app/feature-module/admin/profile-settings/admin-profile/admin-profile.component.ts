import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { routes } from 'src/app/core/core.index';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AdminService } from 'src/app/core/service/Backend/admin/admin.service';
import { AuthService } from 'src/app/core/service/auth/authservice';

@Component({
  selector: 'app-admin-profile',
  templateUrl: './admin-profile.component.html',
  styleUrls: ['./admin-profile.component.scss']
})
export class AdminProfileComponent implements OnInit {
  public routes = routes;
  profileForm: FormGroup;
  isLoading = false;
  isEditing = false;
  backendErrors: any = {};

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private snackBar: MatSnackBar,
  ) {
    this.profileForm = this.fb.group({
      id: [''],
      nom: ['', Validators.required],
      prenom: ['', Validators.required],
      login: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadAdminProfile();
    console.log('Form controls:', this.profileForm.controls);
  }

  loadAdminProfile(): void {
    this.isLoading = true;

    this.adminService.getAdmin().subscribe({
      next: (admin) => {
        // Directly patch the received values
        this.profileForm.patchValue(admin);
        this.isLoading = false;
        this.isEditing = true;
      },
      error: (err) => {
        console.error('Failed to load admin profile', err);
        this.snackBar.open('Failed to load profile data', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.loadAdminProfile(); // Reload original data if canceling edit
    }
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      this.snackBar.open('Please fill all required fields correctly', 'Close', { duration: 3000 });
      return;
    }

    this.isLoading = true;
    const updatedAdmin = this.profileForm.value;

    this.adminService.updateAdmin(updatedAdmin).subscribe({
      next: () => {
        this.snackBar.open('Profile updated successfully', 'Close', { duration: 3000 });
        this.isEditing = false;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 400 && err.error.errors) {
          this.backendErrors = err.error.errors; // Store backend errors
          // Mark fields as touched to show errors
          Object.keys(this.backendErrors).forEach(field => {
            this.profileForm.get(field.toLowerCase())?.markAsTouched();
          });
        } else {
          this.snackBar.open('Failed to update profile', 'Close', { duration: 3000 });
        }
      }
    });
  }

  onReset(): void {
    this.backendErrors = {};
    this.profileForm.reset();
    this.loadAdminProfile();
    Object.keys(this.profileForm.controls).forEach(key => {
      this.profileForm.get(key)?.markAsUntouched();
    });
  }
}