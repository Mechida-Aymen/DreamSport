import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';
import { SportCategory, TerrainService } from 'src/app/core/service/terrain/terrain.service';
import { EquipeService } from 'src/app/core/service/equipe/equipe.service';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { Router } from '@angular/router';
import { routes } from 'src/app/core/core.index';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-team-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  createTeamForm: FormGroup;
  selectedFile: File | null = null;
  sportCategories: SportCategory[] = [];
  previewImage: string | ArrayBuffer | null = 'assets/img/icons/img-icon.svg';
  isLoading = false;
  public routes = routes;
  
  constructor(
    private fb: FormBuilder,
    private cloudflareService: CloudflareService,
    private terrainService: TerrainService,
    private equipeService: EquipeService,
    private authService: AuthService,
    private Router: Router,
    private toastr: ToastrService,
    

  ) {
    this.createTeamForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      sportId: ['', Validators.required],
      avatar: ['']
    });
  }

  ngOnInit(): void {
    this.loadSportCategories();
  }

  loadSportCategories(): void {
    this.terrainService.getSportCategories().subscribe({
      next: (categories) => {
        this.sportCategories = categories;
      },
      error: (err) => console.error('Error loading sports:', err)
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;

    if (!file.type.match('image.*')) {
      alert('Seules les images sont autorisées (JPG, PNG, SVG)');
      return;
    }

    this.selectedFile = file;

    // Aperçu de l'image
    const reader = new FileReader();
    reader.onload = (e) => {
      this.previewImage = e.target?.result || null;
    };
    reader.readAsDataURL(file);
  }

  onSubmit(): void {
    if (this.createTeamForm.invalid) {
      this.toastr.error('Please fill all required fields');
      return;
    }
  
    this.isLoading = true;
    const formData = this.createTeamForm.value;
    const DEFAULT_AVATAR = 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg';
  
    const uploadProcess = this.selectedFile 
      ? this.cloudflareService.uploadFile(this.selectedFile)
      : Promise.resolve(DEFAULT_AVATAR);
  
    uploadProcess.then((imageUrl) => {
      const teamData = {
        AdminId: this.authService.getUserId(),
        SportId: formData.sportId,
        Name: formData.name,
        Description: formData.description,
        Avatar: imageUrl || DEFAULT_AVATAR, 
        CaptainId: this.authService.getUserId() 
      };
  
      this.equipeService.createTeam(teamData).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.toastr.success('Team created successfully!');
          this.Router.navigate([this.routes.userTeam]).then(() => {
            window.location.reload();
          });
        },
        error: (err) => {
          this.isLoading = false;
          console.error('Error creating team:', err);
          this.toastr.error('Failed to create team');
        }
      });
    }).catch(err => {
      this.isLoading = false;
      console.error('Upload error:', err);
      this.toastr.error('Image upload failed');
    });
  }
}