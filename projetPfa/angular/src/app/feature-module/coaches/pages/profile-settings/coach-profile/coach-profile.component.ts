import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { routes } from 'src/app/core/core.index';
import { employee } from 'src/app/core/models/employee/employee';
import { ToastrService } from 'ngx-toastr';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';
import { EmployeesService } from 'src/app/core/service/Backend/employees/employees.service';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { from } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-coach-profile',
  templateUrl: './coach-profile.component.html',
  styleUrls: ['./coach-profile.component.scss']
})
export class CoachProfileComponent implements OnInit {
  public routes = routes;
  employeeForm: FormGroup;
  employeeData: employee = {} as employee;
  selectedFile: File | null = null;
  isLoading = false;
  backendErrors: any = {};

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeesService,
    private toastr: ToastrService,
    private cloudflareService: CloudflareService,
    private authService: AuthService,
    private router:Router 
  ) {
    this.employeeForm = this.fb.group({
      nom: ['', Validators.required],
      prenom: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      imageUrl: ['']
    });
  }

  ngOnInit(): void {
    this.loadEmployeeData();
  }

  get avatarUrl(): string {
    return this.employeeData?.imageUrl || 'assets/img/icons/img-icon.svg';
  }

  loadEmployeeData(): void {
    const employeeId = this.authService.getUserId();
    
    this.employeeService.getEmployee(employeeId).subscribe({
      next: (data) => {
        this.employeeData = data;
        this.employeeForm.patchValue({
          nom: data.nom,
          prenom: data.prenom,
          username: data.username,
          email: data.email,
          phoneNumber: data.phoneNumber,
          imageUrl: data.imageUrl
        });
      },
      error: (err) => {
        this.toastr.error('Failed to load employee data');
        console.error(err);
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;

    if (!file.type.match('image.*')) {
      this.toastr.warning('Only images are allowed (JPG, PNG, SVG)');
      return;
    }

    const reader = new FileReader();
    reader.onload = (e: any) => {
      const img = new Image();
      img.src = e.target.result;
      
      img.onload = () => {
        if (img.width < 150 || img.height < 150) {
          this.toastr.warning('Image must be at least 150×150 pixels');
          return;
        }

        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        const size = Math.min(img.width, img.height);
        
        canvas.width = 150;
        canvas.height = 150;
        
        const offsetX = (img.width - size) / 2;
        const offsetY = (img.height - size) / 2;
        
        ctx?.drawImage(img, offsetX, offsetY, size, size, 0, 0, 150, 150);
        
        this.employeeData.imageUrl = canvas.toDataURL('image/jpeg', 0.9);
        this.selectedFile = file;
      };
    };
    reader.readAsDataURL(file);
  }

  saveChanges(): void {
    if (!this.employeeData) {
      console.error('Employee data not loaded!');
      return;
    }
  
    if (this.employeeForm.invalid) {
      this.markFormGroupTouched(this.employeeForm);
      this.toastr.warning('Please fill all required fields correctly');
      return;
    }
  
    this.isLoading = true;
    console.log('Starting save operation...');
  
    // Create updated data object with form values
    const updatedData: employee = {
      ...this.employeeData, // Keep existing data
      ...this.employeeForm.value, // Override with form values
      id: this.employeeData.id, // Ensure ID is preserved
      adminId: this.employeeData.adminId // Preserve adminId if exists
    };
  
    if (this.selectedFile) {
      console.log('File selected for upload:', {
        name: this.selectedFile.name,
        type: this.selectedFile.type,
        size: this.selectedFile.size
      });
  
      from(this.cloudflareService.uploadFile(this.selectedFile)).subscribe({
        next: (imageUrl) => {
          console.log('✅ Cloudflare upload successful! URL:', imageUrl);
          updatedData.imageUrl = imageUrl; // Update with new image URL
          this.updateEmployeeData(updatedData);
        },
        error: (err) => {
          console.error('❌ Cloudflare upload failed:', err);
          this.isLoading = false;
          this.toastr.error('Image upload failed');
        }
      });
    } else {
      console.log('No file selected, skipping upload');
      this.updateEmployeeData(updatedData);
    }
  }
  
  private updateEmployeeData(data: employee): void {
    console.log('Updating employee data with:', data);
    
    this.employeeService.updateEmployee(data).subscribe({
      next: (updatedEmployee) => {
        console.log('✅ Profile update successful:', updatedEmployee);
        this.employeeData = updatedEmployee; // Update local data with server response
        this.isLoading = false;
        this.selectedFile = null;
        const storedUser = localStorage.getItem('user_data');
        if (storedUser) {
          // Parse the JSON string to an object
          const user = JSON.parse(storedUser);
        
          // Modify only the 'name' property
          user.Nom = data.nom;
          user.Prenom = data.prenom;
          user.ImageUrl = data.imageUrl;
        
          // Save the updated object back to localStorage
          localStorage.setItem('user_data', JSON.stringify(user));
          this.router.navigateByUrl(this.routes.userProfile).then(() => {
            window.location.reload();
          }); 
        }
        this.toastr.success('Profile updated successfully');
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 400 && err.error.errors) {
          this.backendErrors = err.error.errors;
          Object.keys(this.backendErrors).forEach(field => {
            this.employeeForm.get(field.toLowerCase())?.markAsTouched();
          });
        } else {
          this.toastr.error('Failed to update profile');
        }
        console.error('❌ Profile update failed:', err);
      }
    });
  }
  
  // Add this helper method to mark all form fields as touched
  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  resetForm(): void {
    this.backendErrors = {};
    this.employeeForm.reset();
    this.loadEmployeeData();
    this.selectedFile = null;
  }
}