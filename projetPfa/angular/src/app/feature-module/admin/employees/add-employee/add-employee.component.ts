import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AddEmployee } from 'src/app/core/models/employee/addEmployee';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service'; // Add this if using Cloudflare

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.scss']
})
export class AddEmployeeComponent {
  @Output() onSubmitSuccess = new EventEmitter<AddEmployee>();
  addForm: FormGroup;
  apiErrors: any = {};
  isSubmitting = false;
  isUploading = false;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;
  maxBirthDate: Date;


  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddEmployeeComponent>,
    private snackBar: MatSnackBar,
    private cloudflareService: CloudflareService, // Inject if using Cloudflare
    @Inject(MAT_DIALOG_DATA) public data: AddEmployee
  ) {

    this.maxBirthDate = new Date();

    this.addForm = this.fb.group({
      prenom: ['', Validators.required],
      nom: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      cin: ['', Validators.required],
      username: ['', Validators.required],
      salaire: [null, [Validators.required, Validators.min(0)]],
      birthday: [null, Validators.required],
      imageFile: [null]
    });

  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;
  
    // Validate file type
    if (!file.type.match('image.*')) {
      this.apiErrors['image'] = 'Only JPG, PNG or SVG images are allowed';
      return;
    }
  
    // Validate file size (max 2MB)
    const maxSizeMB = 2;
    if (file.size > maxSizeMB * 1024 * 1024) {
      this.apiErrors['image'] = `Image must be less than ${maxSizeMB}MB`;
      return;
    }
  
    this.selectedFile = file;
    this.addForm.patchValue({ imageFile: file });
    this.addForm.get('imageFile')?.updateValueAndValidity();
  
    // Create preview
    const reader = new FileReader();
    reader.onload = (e) => {
      this.imagePreview = e.target?.result || null;
      
      // Validate image dimensions
      const img = new Image();
      img.onload = () => {
        if (img.width < 150 || img.height < 150) {
          this.apiErrors['image'] = 'Image must be at least 150×150 pixels';
          this.imagePreview = null;
          this.selectedFile = null;
        } else {
          this.apiErrors['image'] = null;
        }
      };
      img.src = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

  async onSubmit(): Promise<void> {
    if (this.addForm.valid) {
      this.isSubmitting = true;
      this.apiErrors = {};

      try {
        // First upload image if selected
        let imageUrl = '';
        if (this.selectedFile) {
          this.isUploading = true;
          imageUrl = await this.cloudflareService.uploadFile(this.selectedFile);
          this.isUploading = false;
        }

        // Create the AddEmployee object
        const employeeData: AddEmployee = {
          prenom: this.addForm.value.prenom,
          nom: this.addForm.value.nom,
          email: this.addForm.value.email,
          phoneNumber: this.addForm.value.phoneNumber,
          cin: this.addForm.value.cin,
          username: this.addForm.value.username,
          salaire: this.addForm.value.salaire,
          birthday: new Date(this.addForm.value.birthday).toISOString(),
          imageUrl: imageUrl || undefined // Only include if we have a URL
        };

        // Emit the complete employee data
        this.onSubmitSuccess.emit(employeeData);

      } catch (error) {
        this.isSubmitting = false;
        this.isUploading = false;
        this.snackBar.open('Error uploading image', 'Close', { duration: 3000 });
        console.error('Upload error:', error);
      }
    } else {
      // Mark all fields as touched to show validation messages
      Object.values(this.addForm.controls).forEach(control => {
        control.markAsTouched();
      });
    }
  }

  setErrors(errors: any): void {
    this.apiErrors = errors;
    this.isSubmitting = false;
    this.isUploading = false;
    
    Object.keys(errors).forEach(key => {
      const control = this.addForm.get(key);
      if (control) {
        control.setErrors({ apiError: true });
        control.markAsTouched();
      }
    });
  }

  close(): void {
    this.dialogRef.close();
  }
}