import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { employee } from 'src/app/core/models/employee/employee';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';

@Component({
  selector: 'app-update-employee',
  templateUrl: './update-employee.component.html',
  styleUrls: ['./update-employee.component.scss']
})
export class UpdateEmployeeComponent {
  @Output() onSubmitSuccess = new EventEmitter<any>();
  updateForm: FormGroup;
  apiErrors: any = {};
  isSubmitting = false;
  isUploading = false;
  selectedFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<UpdateEmployeeComponent>,
    private cloudflareService: CloudflareService,
    @Inject(MAT_DIALOG_DATA) public data: { employee: employee }
  ) {
    this.updateForm = this.fb.group({
      nom: [data.employee.nom, Validators.required],
      prenom: [data.employee.prenom, Validators.required],
      email: [data.employee.email, [Validators.required, Validators.email]],
      phoneNumber: [data.employee.phoneNumber, Validators.required],
      salaire: [data.employee.salaire, [Validators.required, Validators.min(0)]],
      birthday: [new Date(data.employee.birthday), Validators.required],
      imageFile: [null]
    });

    this.imagePreview = data.employee.imageUrl;
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;
  
    // Validate file type
    const validTypes = ['image/jpeg', 'image/png'];
    if (!validTypes.includes(file.type)) {
      this.apiErrors['image'] = 'Only JPG or PNG images are allowed';
      return;
    }
  
    // Validate file size (max 2MB)
    const maxSizeMB = 2;
    if (file.size > maxSizeMB * 1024 * 1024) {
      this.apiErrors['image'] = `Image must be less than ${maxSizeMB}MB`;
      return;
    }
  
    this.selectedFile = file;
    this.updateForm.patchValue({ imageFile: file });
    this.updateForm.get('imageFile')?.updateValueAndValidity();
  
    // Create preview with fixed dimensions
    const reader = new FileReader();
    reader.onload = (e: any) => {
      const img = new Image();
      img.onload = () => {
        // Create canvas to resize image
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        
        // Set canvas dimensions to our fixed size
        canvas.width = 150;
        canvas.height = 150;
        
        // Draw image centered and cropped to square
        const offsetX = (img.width - Math.min(img.width, img.height)) / 2;
        const offsetY = (img.height - Math.min(img.width, img.height)) / 2;
        const size = Math.min(img.width, img.height);
        
        ctx?.drawImage(img, offsetX, offsetY, size, size, 0, 0, 150, 150);
        
        // Set preview to canvas result
        this.imagePreview = canvas.toDataURL('image/jpeg');
        
        // Clear any previous errors
        this.apiErrors['image'] = null;
      };
      img.src = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  async onSubmit(): Promise<void> {
    if (this.updateForm.valid) {
      this.isSubmitting = true;
      this.apiErrors = {};
  
      try {
        let imageUrl = this.data.employee.imageUrl; // Start with original URL
  
        // Only upload if new file selected
        if (this.selectedFile) {
          this.isUploading = true;
          imageUrl = await this.cloudflareService.uploadFile(this.selectedFile);
          this.isUploading = false;
        }
  
        // Create the update data object
        const updateData = {
          ...this.updateForm.value,
          birthday: new Date(this.updateForm.value.birthday).toISOString(),
          imageUrl: imageUrl, // Always include imageUrl
          id: this.data.employee.id
        };
  
        // Emit the complete update data
        this.onSubmitSuccess.emit(updateData);
  
      } catch (error) {
        this.isSubmitting = false;
        this.isUploading = false;
        this.apiErrors['image'] = 'Error uploading image';
        console.error('Upload error:', error);
      }
    } else {
      Object.values(this.updateForm.controls).forEach(control => {
        control.markAsTouched();
      });
    }
  }

  setErrors(errors: any): void {
    this.apiErrors = errors;
    this.isSubmitting = false;
    this.isUploading = false;
    
    Object.keys(errors).forEach(key => {
      const control = this.updateForm.get(key);
      if (control) {
        control.setErrors({ apiError: true });
        control.markAsTouched();
      }
    });
  }
}