import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DataService, routes } from 'src/app/core/core.index';
import { TerrainService, Terrain, TerrainStatus, SportCategory } from 'src/app/core/service/terrain/terrain.service';

@Component({
  selector: 'app-coach-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss'],
})
export class DetailsComponent implements OnInit {
  public routes = routes;
  public terrains: Terrain[] = []; // Liste des terrains
  public filteredTerrains: Terrain[] = []; // Liste filtrée des terrains
  public sportCategories: SportCategory[] = []; // Liste des catégories de sport
  public terrainStatuses: TerrainStatus[] = []; // Liste des statuts de terrain
  public selectedCategoryId: number | null = null; // Catégorie sélectionnée

  constructor(
    private router: Router,
    private dataservice: DataService,
    private terrainService: TerrainService
  ) {}

  ngOnInit(): void {
    this.loadTerrains(); // Charge les terrains
    this.loadSportCategories(); // Charge les catégories de sport
    this.loadTerrainStatuses(); // Charge les statuts de terrain
  }

  // Charge les terrains
  loadTerrains(): void {
    this.terrainService.getTerrains().subscribe(
      (data: Terrain[]) => {
        this.terrains = data;
        this.filteredTerrains = [...this.terrains]; // Initialise la liste filtrée
      },
      (error) => {
        console.error('Error loading terrains:', error);
      }
    );
  }

  // Charge les catégories de sport
  loadSportCategories(): void {
    this.terrainService.getSportCategories().subscribe(
      (data: SportCategory[]) => {
        this.sportCategories = data;
      },
      (error) => {
        console.error('Error loading sport categories:', error);
      }
    );
  }

  // Charge les statuts de terrain
  loadTerrainStatuses(): void {
    this.terrainService.getTerrainStatuses().subscribe(
      (data: TerrainStatus[]) => {
        this.terrainStatuses = data;
      },
      (error) => {
        console.error('Error loading terrain statuses:', error);
      }
    );
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
}