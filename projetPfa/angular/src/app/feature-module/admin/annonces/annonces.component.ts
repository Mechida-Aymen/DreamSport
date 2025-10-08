import { DatePipe } from '@angular/common';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { ToastrService } from 'ngx-toastr';
import { Annonce } from 'src/app/core/models/Site/Annonce';
import { AnnoncesService } from 'src/app/core/service/Backend/SIte/annonces.service';
@Component({
  selector: 'app-annonces',
  templateUrl: './annonces.component.html',
  styleUrl: './annonces.component.scss'
})
export class AnnoncesComponent {
  public searchDataValue = '';
  public tableShowed:Annonce[]=[];
  public tableData:Annonce[] = [];
  public isLoading = false;
  public selectedTab = 'all';

  constructor(private annonceService: AnnoncesService,private datePipe: DatePipe,private toastr: ToastrService,) { }
    
    ngOnInit(): void {
      this.loadData();
    }

  public loadData():void {
    this.isLoading = true;
    this.annonceService.getAnnonces().subscribe({
      next: (list) => {
        const formattedList = list.map(item => ({
          ...item,
          launchedAt: this.formatDate(item.launchedAt),
          endDate: this.formatDate(this.calculateEndDate(item.launchedAt, item.lifeTimeBySeconds))
        }));
        this.tableData = formattedList;
        this.tableShowed = formattedList;
        this.isLoading = false;
        
      },
      error: (err) => {
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  @ViewChild('startDate') startDate!: ElementRef;
  @ViewChild('endDate') endDate!: ElementRef;
  dateError = false;

  validateDates() {
    const start = new Date(this.startDate.nativeElement.value);
    const end = new Date(this.endDate.nativeElement.value);
    this.dateError = end <= start;
  }

  addNewAnnonce(formData: any) {
    if (this.dateError) return;
    
    const launchDate = new Date(formData.launchedAt);
    const endDate = new Date(formData.endDate);
    const durationInSeconds = Math.floor((endDate.getTime() - launchDate.getTime()) / 1000);
    
    const newAnnonce = {
      description: formData.description,
      launchedAt: launchDate.toISOString(),
      lifeTimeBySeconds: durationInSeconds,
    };
    
    // Call your service to save the announcement
    this.annonceService.createAnnonce(newAnnonce).subscribe({
      next: (response) => {
        this.loadData();  // Add new FAQ to the list
        this.toastr.success('success', 'Annonce added successfully');
      },
      error: (err) => {
        this.toastr.error('error', 'An error happen while adding your Annonce');
      }
    });
  }


  public searchData(value: string): void {
    //search methode 
  }

  public sortData(sort: Sort) {
        const data = this.tableShowed.slice();
    
        if (!sort.active || sort.direction === '') {
          this.tableShowed = data;
        } else {
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          this.tableShowed = data.sort((a: any, b: any) => {
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            const aValue = (a as any)[sort.active];
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            const bValue = (b as any)[sort.active];
            return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
          });
        }
    }
  




    public displayAll():void {
      this.tableShowed=this.tableData;
      this.selectedTab = 'all';
    }
    public displayActive(): void {
      const now = new Date();
      
      this.tableShowed = this.tableData.filter(annonce => {
        // Convert formatted dates back to Date objects for comparison
        const launchDate = new Date(annonce.launchedAt);
        const endDate = new Date(launchDate.getTime() + (annonce.lifeTimeBySeconds * 1000));
        return now >= launchDate && now <= endDate;
      });
      this.selectedTab = 'active';
    }
    public displayComing():void {
      const now = new Date();
      
      this.tableShowed = this.tableData.filter(annonce => {
        // Convert formatted dates back to Date objects for comparison
        const launchDate = new Date(annonce.launchedAt);
        
        return now < launchDate;
      });
      this.selectedTab = 'Coming';
    }

    private formatDate(dateString: string | undefined): string {
      if (!dateString) return '';
      return this.datePipe.transform(dateString, 'MMM EEE dd yyyy') || '';
      // Now outputs: "Feb Sun 23 2025"
    }
    private calculateEndDate(launchedAt: string, seconds: number): string {
      const launchDate = new Date(launchedAt);
      const endDate = new Date(launchDate.getTime() + (seconds * 1000));
      return endDate.toISOString(); // Or format as needed
    }

}
