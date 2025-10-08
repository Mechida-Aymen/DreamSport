import { Component, OnInit } from '@angular/core';
import { Observable, catchError, from, map, of } from 'rxjs';
import { routes } from 'src/app/core/core.index';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { EquipeService } from 'src/app/core/service/equipe/equipe.service';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.scss']
})
export class SettingComponent implements OnInit {
  public routes = routes;
  public teamId: number | undefined;
  public teamDetails: TeamDetails | undefined;
  public selectedFile: File | null = null;
  public isLoading = false;

  constructor(
    private equipeService: EquipeService,
    private auth: AuthService,
    private cloudflareService: CloudflareService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadTeamData();
  }

  private loadTeamData(): void {
    this.isLoading = true;
    this.getTeamId(this.auth.getUserId()).subscribe({
      next: (teamId) => {
        this.teamId = teamId;
        if (this.teamId) {
          this.equipeService.getTeamDetails(this.teamId).subscribe({
            next: (data) => {
              this.teamDetails = data;
              this.isLoading = false;
            },
            error: (err) => {
              console.error('Error fetching team details:', err);
              this.isLoading = false;
            }
          });
        }
      },
      error: (err) => {
        console.error('Error getting team ID:', err);
        this.isLoading = false;
      }
    });
  }

  getTeamId(userId: number): Observable<number> {
    return this.equipeService.checkMembership(userId).pipe(
      map(response => response.equipeId),
      catchError(err => {
        console.error('Erreur lors de la vérification du statut', err);
        return of(0);
      })
    );
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;

    if (!file.type.match('image.*')) {
      alert('Seules les images sont autorisées (JPG, PNG, SVG)');
      return;
    }

    const reader = new FileReader();
    reader.onload = (e: any) => {
      const img = new Image();
      img.src = e.target.result;
      
      img.onload = () => {
        if (img.width < 150 || img.height < 150) {
          alert('L\'image doit faire au moins 150×150 pixels');
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
        
        if (this.teamDetails) {
          this.teamDetails.avatar = canvas.toDataURL('image/jpeg', 0.9);
          this.selectedFile = file;
        }
      };
    };
    reader.readAsDataURL(file);
  }

  resetChanges(): void {
    if (this.teamId) {
      this.isLoading = true;
      this.equipeService.getTeamDetails(this.teamId).subscribe({
        next: (data) => {
          this.teamDetails = data;
          this.selectedFile = null;
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.isLoading = false;
        }
      });
    }
  }

  saveChanges(): void {
    if (!this.teamDetails) return;
  
    this.isLoading = true;
  
    if (this.selectedFile) {
      // Correction du nom de méthode et conversion de la Promesse en Observable
      from(this.cloudflareService.uploadFile(this.selectedFile)).subscribe({
        next: (imageUrl) => {
          this.teamDetails!.avatar = imageUrl;
          this.updateTeamData();
        },
        error: (err) => {
          console.error('Image upload error:', err);
          this.isLoading = false;
        }
      });
    } else {
      this.updateTeamData();
    }
  }

  private updateTeamData(): void {
    if (!this.teamDetails) return;

    this.equipeService.updateTeamDetails(this.teamDetails).subscribe({
      next: (response) => {
        this.selectedFile = null;
        this.isLoading = false;
        this.toastr.success('Team updated successfully!', 'Success');
      },
      error: (err) => {
        this.isLoading = false;
        this.toastr.error('Failed to update team', 'Error');

      }
    });
  }
}

interface TeamDetails {
  id: number;
  adminId: number | null;
  sportId: number;
  name: string;
  description: string;
  avatar: string;
  captainId: number;
  membres: TeamMemberRelation[];
}

interface TeamMemberRelation {
  userId: number;
  equipeId: number;
}