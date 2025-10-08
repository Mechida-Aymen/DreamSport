import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { routes } from 'src/app/core/helpers/routes';
import { ToastrService } from 'ngx-toastr';
import { UsersService } from 'src/app/core/service/Backend/users/users.service';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { from } from 'rxjs';
import { User } from 'src/app/core/models/user.model';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  public routes = routes;
  profileForm: FormGroup;
  userData: User = {} as User;
  selectedFile: File | null = null;
  isLoading = false;
  backendErrors: any = {};

  constructor(
    private fb: FormBuilder,
    private usersService: UsersService,
    private toastr: ToastrService,
    private cloudflareService: CloudflareService,
    private authService: AuthService,
    private router:Router
  ) {
    this.profileForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      imageUrl: [''],
      bio: [''] 
    });
  }

  ngOnInit(): void {
    this.loadUserData();
  }

  get avatarUrl(): string {
    return this.userData?.imageUrl || 'assets/img/icons/img-icon.svg';
  }

  loadUserData(): void {
    const userId = this.authService.getUserId();
    
    this.usersService.geetUser(userId).subscribe({
      next: (data) => {
        this.userData = data;
        this.profileForm.patchValue({
          firstName: data.firstName,
          lastName: data.lastName,
          username: data.username,
          email: data.email,
          phoneNumber: data.phoneNumber,
          imageUrl: data.imageUrl,
          bio: data.Bio 
        });
      },
      error: (err) => {
        this.toastr.error('Failed to load user data');
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
        
        this.userData.imageUrl = canvas.toDataURL('image/jpeg', 0.9);
        this.selectedFile = file;
      };
    };
    reader.readAsDataURL(file);
  }

  saveChanges(): void {
    if (!this.userData) {
      console.error('User data not loaded!');
      return;
    }
  
    if (this.profileForm.invalid) {
      this.markFormGroupTouched(this.profileForm);
      this.toastr.warning('Please fill all required fields correctly');
      return;
    }
  
    this.isLoading = true;
    console.log('Starting save operation...');
  
    const updatedData: User = {
      ...this.userData,
      ...this.profileForm.value,
      id: this.userData.id
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
          updatedData.imageUrl = imageUrl;
          this.updateUserData(updatedData);
        },
        error: (err) => {
          console.error('❌ Cloudflare upload failed:', err);
          this.isLoading = false;
          this.toastr.error('Image upload failed');
        }
      });
    } else {
      console.log('No file selected, skipping upload');
      this.updateUserData(updatedData);
    }
  }
  
  private updateUserData(data: User): void {
    console.log('Updating user data with:', data);
    
    this.usersService.updateUser(data).subscribe({
      next: (updatedUser) => {
        console.log('✅ Profile update successful:', updatedUser);
        this.userData = updatedUser;
        this.isLoading = false;
        this.selectedFile = null;
        const storedUser = localStorage.getItem('user_data');
        if (storedUser) {
          // Parse the JSON string to an object
          const user = JSON.parse(storedUser);
          console.log("up : ", data)
          // Modify only the 'name' property
          user.Nom = data.lastName;
          user.Prenom = data.firstName;
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
            this.profileForm.get(field.toLowerCase())?.markAsTouched();
          });
        } else {
          this.toastr.error('Failed to update profile');
        }
        console.error('❌ Profile update failed:', err);
      }
    });
  }
  
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
    this.profileForm.reset();
    this.loadUserData();
    this.selectedFile = null;
  }
}