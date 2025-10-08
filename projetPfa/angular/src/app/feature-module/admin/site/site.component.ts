import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SiteService } from 'src/app/core/service/Backend/SIte/site.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { SiteM } from 'src/app/core/models/Site/siteM';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';
import { from } from 'rxjs';
import { Route, Router } from '@angular/router';
import { routes } from 'src/app/core/core.index';

@Component({
  selector: 'app-site',
  templateUrl: './site.component.html',
  styleUrls: ['./site.component.scss']
})
export class SiteComponent implements OnInit {
  siteForm: FormGroup;
  isSaving = false;
  isUploading = false;
  currentUploadField: string | null = null;
  backendErrors: any = {};
  private Site: any;
  public routes = routes;
  

  constructor(
    private fb: FormBuilder,
    private siteService: SiteService,
    private toastr: ToastrService,
    private cloudflareService: CloudflareService,
    private router:Router
  ) {
    this.siteForm = this.fb.group({
      Id: [''],
      Name: ['', Validators.required],
      Logo: [''],
      Description: [''],
      Email: ['', [Validators.email]],
      PhoneNumber: [''],
      AboutUs: [''],
      CouleurPrincipale: ['#3f51b5', [Validators.pattern(/^#([0-9A-F]{3}){1,2}$/i)]],
      CouleurSecondaire: ['#ff4081', [Validators.pattern(/^#([0-9A-F]{3}){1,2}$/i)]],
      Background: [''],
      Addresse: [''],
      DomainName: ['', [Validators.pattern(/^[a-zA-Z0-9][a-zA-Z0-9-]{1,61}[a-zA-Z0-9](?:\.[a-zA-Z]{2,})+$/)]]
    });
  }

  ngOnInit(): void {
    this.loadSiteSettings();
  }

  loadSiteSettings(): void {
    this.siteService.getSiteSettings().subscribe({
      next: (settings) => {
        const siteSettings = Array.isArray(settings) ? settings[0] : settings;
        this.Site = siteSettings;

        this.siteForm.patchValue({
          Id: siteSettings.id,
          Name: siteSettings.name,
          Logo: siteSettings.logo,
          Description: siteSettings.description,
          Email: siteSettings.email,
          PhoneNumber: siteSettings.phoneNumber,
          AboutUs: siteSettings.aboutUs,
          CouleurPrincipale: siteSettings.couleurPrincipale,
          CouleurSecondaire: siteSettings.couleurSecondaire,
          Background: siteSettings.background,
          Addresse: siteSettings.addresse,
          DomainName: siteSettings.domainName
        });
      },
      error: (err: HttpErrorResponse) => {
        console.error("Error loading settings:", err);
        this.toastr.error('Failed to load site settings', 'Error');
      }
    });
  }

  onFileSelected(event: Event, field: string): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    if (!file.type.match('image.*')) {
        this.toastr.warning('Please select an image file', 'Invalid File');
        return;
    }

    this.isUploading = true;
    this.currentUploadField = field;

    const reader = new FileReader();
    reader.onload = (e: any) => {
        const img = new Image();
        img.src = e.target.result;
        
        img.onload = () => {
           
            from(this.cloudflareService.uploadFile(file)).subscribe({
                next: (imageUrl) => {
                    this.siteForm.get(field)?.setValue(imageUrl);
                    this.toastr.success('Image uploaded successfully');
                    this.backendErrors[field] = null;
                },
                error: (err) => {
                    console.error('Upload failed:', err);
                    this.toastr.error('Failed to upload image');
                    this.backendErrors[field] = 'Image upload failed';
                },
                complete: () => {
                    this.isUploading = false;
                    this.currentUploadField = null;
                    input.value = '';
                }
            });
        };
    };
    reader.readAsDataURL(file);
}

  removeBackground(): void {
    this.siteForm.get('Background')?.setValue('');
    this.backendErrors['Background'] = null;
  }

  resetForm(): void {
    this.backendErrors = {};
    this.siteForm.reset();
    this.loadSiteSettings();
  }

  saveSiteSettings(): void {
    if (this.siteForm.invalid) {
      this.markFormGroupTouched(this.siteForm);
      this.toastr.warning('Please fill all required fields correctly');
      return;
    }

    this.isSaving = true;
    this.siteService.updateSiteSettings(this.siteForm.value).subscribe({
      next: () => {
        this.toastr.success('Site settings saved successfully!');

        this.backendErrors = {}; // Clear all errors on success
        this.isSaving = false;
        this.router.navigateByUrl(this.routes.userTeam).then(() => {
          window.location.reload();
        });      } 
       ,
      error: (err: HttpErrorResponse) => {
        console.warn("das ",err);
        this.isSaving = false;
        if (err.status === 400 && err.error.errors) {
          this.backendErrors = err.error.errors;
          console.log("thi :", this.backendErrors);
        } else {
          this.toastr.error('Failed to save site settings');
        }
        console.error('Save error:', err);
      }
    });
  }

  onColorChange(field: string, event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input) {
      this.siteForm.get(field)?.setValue(input.value);
      this.backendErrors[field] = null;
    }
  }

  onTextColorChange(field: string, event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    
    if (/^#([0-9A-F]{3}){1,2}$/i.test(value)) {
      this.siteForm.get(field)?.setValue(value);
      this.backendErrors[field] = null;
    } else {
      this.toastr.warning('Please enter a valid hex color code');
    }
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}