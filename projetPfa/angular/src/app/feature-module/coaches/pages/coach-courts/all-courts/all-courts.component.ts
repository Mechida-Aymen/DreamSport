import { Component } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { forkJoin } from 'rxjs';
import { DataService, routes } from 'src/app/core/core.index';
import { SportCategory, Terrain, TerrainService, TerrainStatus } from 'src/app/core/service/terrain/terrain.service';
import { allcourts } from 'src/app/shared/model/page.model';
import { PaginationService } from 'src/app/shared/shared.index';


interface data {
  value: string;
}
@Component({
  selector: 'app-all-courts',
  templateUrl: './all-courts.component.html',
  styleUrls: ['./all-courts.component.scss']
})
export class AllCourtsComponent {
  checked!: boolean;
  public terrains: Terrain[] = []; 
  public filteredTerrains: Terrain[] = []; 
  public sportCategories: SportCategory[] = []; 
  public terrainStatuses: TerrainStatus[] = []; 
  public selectedCategoryId: number | null = null;

  selectedTab: string = 'all';
  initChecked = false;
  public tableData: Array<allcourts> = [];
  public tableShowed: Array<allcourts> = [];
  public routes = routes;
  // pagination variables
  public pageSize = 10;
  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  showFilter = false;
  dataSource!: MatTableDataSource<allcourts>;
  public searchDataValue = '';
  
  
  constructor(private toastr: ToastrService, private terrainService: TerrainService,private data: DataService,private router: Router,private pagination: PaginationService)
  {}

  ngOnInit(): void {
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

  // Charge les terrains
  loadTerrains(): void {
    this.terrainService.getTerrains().subscribe({
      next: (data: Terrain[]) => {
        this.terrains = this.mapTerrainObjects(data);
        this.filteredTerrains = [...this.terrains];
        
        this.terrains.forEach(terrain => {
          this.selectedStatuses[terrain.id] = terrain.terrainStatusId;
        });
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

  public searchData(value: string): void {
    this.dataSource.filter = value.trim().toLowerCase();
    this.tableData = this.dataSource.filteredData;
  }

  public sortData(sort: Sort) {
    const data = this.filteredTerrains.slice();

    if (!sort.active || sort.direction === '') {
      this.filteredTerrains = data;
    } else {
      this.filteredTerrains = data.sort((a, b) => {
        const aValue = (a as never)[sort.active];
        const bValue = (b as never)[sort.active];
        return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
      });
    }
  }

  // filterCourtsBySport
  selectedSport:any;
  filterCourtsBySport():void {
    this.filteredTerrains=this.terrains.filter(
      court => court.sport_Categorie.name === this.selectedSport
    );
  }

  // filterCourtsBySport
  selectedStatus:any;
  filterCourtsByStatus():void {
    this.filteredTerrains=this.terrains.filter(
      court => court.sport_Categorie.name === this.selectedSport
    );
  }
  selectedStatuses: { [id: number]: number } = {};

  StatusUpdatingList(id: number): void {
    const newStatusId = this.selectedStatuses[id];
    const terrain = this.terrains.find(t => t.id === id);
    
    if (terrain && terrain.terrainStatusId !== newStatusId) {
      this.terrainService.updateCourtStatus(id, newStatusId).subscribe({
        next: () => {
          // Update local data
          terrain.terrainStatusId = newStatusId;
          terrain.terrainStatus = this.terrainStatuses.find(s => s.id === newStatusId);
          this.toastr.success('Court updated', 'Success');
        },
        error: (err) => {
          this.toastr.error(err, 'Success');
          console.error('Error updating court status:', err)
        }
      });
    }
  }

  public activateCourts() {
    this.tableShowed = this.tableData.filter(item => item.status === true);
    this.selectedTab = 'active';
  }

  public inactiveCourts() {
    this.tableShowed = this.tableData.filter(item => item.status === false);
    this.selectedTab = 'inactive';
  } 
  public allCourts(){
    this.tableShowed = this.tableData;
    this.selectedTab = 'all';
  }
}
