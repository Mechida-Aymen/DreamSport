import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';
import { SportCategory, Terrain, TerrainService, TerrainStatus } from 'src/app/core/service/terrain/terrain.service';


@Component({
  selector: 'app-courts',
  templateUrl: './courts.component.html',
  styleUrl: './courts.component.scss'
})
export class CourtsComponent {
    public terrains: Terrain[] = []; // Liste des terrains
    public filteredTerrains: Terrain[] = []; // Liste filtrée des terrains
    public sportCategories: SportCategory[] = []; // Liste des catégories de sport
    public terrainStatuses: TerrainStatus[] = []; // Liste des statuts de terrain
    public selectedCategoryId: number | null = null; // Catégorie sélectionnée
    
    courtForm: FormGroup;
    selectedFile: File | null = null;
    imagePreview: string | ArrayBuffer | null = null;
    isUploading = false;

  constructor(  private terrainService: TerrainService, private toastr: ToastrService,
    private fb: FormBuilder, private cloudflareService:CloudflareService) 
    {  
      this.courtForm = this.fb.group({
        title: ['', Validators.required],
        description: ['', Validators.required],
        idSport_Categorie: ['', Validators.required],
        terrainStatusId: ['', Validators.required],
        imageUrl: ['']
      });
    console.log('CourtsComponent constructor called'); // Debug 1
    }
  
    ngOnInit(): void {
      console.log('CourtsComponent ngOnInit called'); // Debug 2

      this.loadData();
    }
  
    loadData(): void {
      // First load categories and statuses
      forkJoin([
        this.terrainService.getSportCategories(),
        this.terrainService.getTerrainStatuses()
      ]).subscribe({
        next: ([categories, statuses]) => {
          this.sportCategories = categories;
          this.terrainStatuses = statuses;
          
          // Then load terrains and map them with the related objects
          this.loadTerrains();
        },
        error: (err) => console.error('Error loading initial data:', err)
      });
    }
    selectedSport:any;
    // filterCourtsBySport
    filterCourtsBySport():void {
      this.filteredTerrains=this.terrains.filter(
        court => court.sport_Categorie.name === this.selectedSport
      );
    }

    // Charge les terrains
    loadTerrains(): void {
      this.terrainService.getTerrains().subscribe({
        next: (data: Terrain[]) => {
          this.terrains = this.mapTerrainObjects(data);
          this.filteredTerrains = [...this.terrains];
          console.log("Terrains with full objects:", this.terrains);
        },
        error: (err) => console.error('Error loading terrains:', err)
      });
    }
  
    private mapTerrainObjects(terrains: Terrain[]): Terrain[] {
      return terrains.map(terrain => ({
        ...terrain,
        sport_Categorie: this.sportCategories.find(cat => cat.id === terrain.idSport_Categorie),
        terrainStatus: this.terrainStatuses.find(status => status.id === terrain.terrainStatusId)
      }));
    }
  
    // Filtre les terrains par catégorie de sport
    onSportCategoryChange(categoryId: number | null): void {
      this.selectedCategoryId = categoryId;
      this.updateFilteredTerrains();
    }
  
    // Met à jour la liste filtrée des terrains
    updateFilteredTerrains(): void {
      this.filteredTerrains = this.terrains.filter(terrain => {
        const categoryMatch = !this.selectedCategoryId || 
                              terrain.idSport_Categorie === this.selectedCategoryId;
        return categoryMatch;
      });
    }
  
    // Vérifie si un terrain est désactivé
    isTerrainDisabled(terrain: Terrain): boolean {
      const disabledStatuses = [2, 3]; // IDs des statuts désactivés
      return disabledStatuses.includes(terrain.terrainStatusId);
    }
  
    // Retourne le libellé du statut du terrain
    getStatusLabel(statusId: number): string {
      const status = this.terrainStatuses.find(s => s.id === statusId);
      return status ? status.libelle : 'Unknown status';
    }
  
    // Retourne le nom de la catégorie de sport
    getSportCategoryName(categoryId: number): string {
      const category = this.sportCategories.find(c => c.id === categoryId);
      return category ? category.name : 'Unknown category';
    }

     deletingCourt:any;
    setSelectedCourt(court: Terrain) {
      this.deletingCourt = court;
    }

    deleteCourt(id: number): void {
      this.terrainService.deleteCourt(id).subscribe({
        next: (response) => {
          // Remove the deleted FAQ from the list
          this.terrains = this.terrains.filter(faq => faq.id !== id);
          this.filteredTerrains = this.terrains;
          this.toastr.success('Success', 'Court deleted successfully');
        },
        error: (err) => {
          this.toastr.error('Error', 'An error encoutred while deleting the court');
        }
      });
    }

    newCourt: any = {
      
      title: '',
      description: '',
      idSport_Categorie: '',
      terrainStatusId: '',
    };
  
    onFileSelected(event: any): void {
      const file = event.target.files[0];
      if (!file) return;
  
      if (!file.type.match('image.*')) {
        this.toastr.warning('Warning', 'Only images are allowed (JPG, PNG, SVG)');
        return;
      }
  
      const reader = new FileReader();
      reader.onload = (e: any) => {
        const img = new Image();
        img.src = e.target.result;
        
        img.onload = () => {
          if (img.width < 150 || img.height < 150) {
            this.toastr.warning('Warning', 'Image must be at least 150×150 pixels');
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
          
          this.imagePreview = canvas.toDataURL('image/jpeg', 0.9);
          this.selectedFile = file;
        };
      };
      reader.readAsDataURL(file);
    }
    // Function to handle form submission
    async addCourt() {
      if (this.courtForm.invalid) {
        this.courtForm.markAllAsTouched();
        return;
      }
  
      this.isUploading = true;
      const newCourt = this.courtForm.value;
  
      try {
        // Upload image if selected
        if (this.selectedFile) {
          const imageUrl = await this.cloudflareService.uploadFile(this.selectedFile);
          newCourt.imageUrl = imageUrl;
        }
  
        this.terrainService.createCourt(newCourt).subscribe({
          next: (response) => {
            this.toastr.success('Success', 'Court added successfully');
            this.courtForm.reset();
            this.imagePreview = null;
            this.selectedFile = null;
            this.closeModal('addCourtModal');
            this.loadTerrains();
            this.isUploading = false;
          },
          error: (err) => {
            console.error('Error adding court:', err);
            this.toastr.error('Error', 'Error adding court');
            this.isUploading = false;
          }
        });
      } catch (error) {
        console.error('Error uploading image:', error);
        this.toastr.error('Error', 'Image upload failed');
        this.isUploading = false;
      }
    }
    
  
    // Reset the form
    resetForm() {
      this.newCourt = { name: '', sportType: '' };
    }
  
    // Close the modal programmatically (if needed)
    closeModal(modalId: string): void {
      // Remove the modal backdrop
      const backdrop = document.querySelector('.modal-backdrop');
      if (backdrop) {
        backdrop.remove();
      }
    
      // Remove modal-open class from body
      document.body.classList.remove('modal-open');
      document.body.style.paddingRight = '';
    
      // Hide the modal
      const modal = document.getElementById(modalId);
      if (modal) {
        modal.classList.remove('show');
        modal.style.display = 'none';
        modal.setAttribute('aria-hidden', 'true');
        modal.removeAttribute('aria-modal');
        modal.removeAttribute('role');
      }
    }


  isClassAdded: boolean[] = [false, false, false];

  
  toggleClass(index: number) {
    this.isClassAdded[index] = !this.isClassAdded[index];
  }
}
