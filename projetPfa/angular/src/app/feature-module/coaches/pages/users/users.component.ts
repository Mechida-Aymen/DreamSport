import { Component } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { PaginationService, tablePageSize } from 'src/app/shared/shared.index';
import { Router } from '@angular/router';
import { pageSelection, routes } from 'src/app/core/core.index';
import { UsersService, userBlock } from 'src/app/core/service/Backend/users/users.service'
import { finalize } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent {
  public searchDataValue = '';
  public tableShowed: userBlock[] = [];
  public tableData: userBlock[] = [];
  public routes = routes;
  public pageSize = 10;
  public totalRecords = 0;
  public isLoading = false;
  public selectedTab: string = 'all';

  public searchControl = new FormControl('');
  public selectedUser: userBlock | null = null; 
  private modal: any; 

  // Pagination variables
  public lastIndex = 0;
  public totalData = 0;
  public skip = 0;
  public limit: number = this.pageSize;
  public pageIndex = 0;
  public serialNumberArray: Array<number> = [];
  public currentPage = 1;
  public pageNumberArray: Array<number> = [];
  public pageSelection: Array<pageSelection> = [];
  public totalPages = 0;

  constructor(
    private router: Router,
    private pagination: PaginationService,
    private userService: UsersService,
    private toastr: ToastrService,
  ) {
    this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
      if (this.router.url === this.routes.coachUsers) {
        this.getTableData({ skip: res.skip, limit: res.limit });
        this.pageSize = res.pageSize;
      }
    });
  }

  ngOnInit(): void {
    this.loadInitialData();
    
    // Add debounce to search input (500ms delay)
    this.searchControl.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged()
      )
      .subscribe((value: string | null) => {
        this.searchData(value!); // The ! tells TypeScript you're sure it's not null
      });
  }

  private loadInitialData(): void {
    this.getTableData({ skip: 0, limit: this.pageSize });
  }

  public searchData(value: string | null): void {
  if (value === null) {
    value = ''; // or handle null case as you prefer
  }
  this.searchDataValue = value;
  // Reset to first page when searching
  this.currentPage = 1;
  this.skip = 0;
  this.limit = this.pageSize;
  this.getTableData({ skip: this.skip, limit: this.limit });
}

  public sortData(sort: Sort) {
    const data = this.tableShowed.slice();

    if (!sort.active || sort.direction === '') {
      this.tableShowed = data;
    } else {
      this.tableShowed = data.sort((a: any, b: any) => {
        const aValue = (a as any)[sort.active];
        const bValue = (b as any)[sort.active];
        return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
      });
    }
  }

  private getTableData(pageOption: pageSelection): void {
    this.isLoading = true;
    
    let isBlocked: boolean | undefined;
    if (this.selectedTab === 'blocked') {
      isBlocked = true;
    } else if (this.selectedTab === 'notBlocked') {
      isBlocked = false;
    }
  
    this.userService.getUsers(
      pageOption.skip, 
      pageOption.limit, 
      isBlocked,
      this.searchDataValue // Pass search term to the service
    )
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (response) => {
          this.tableData = response.users;
          this.tableShowed = [...this.tableData];
          this.totalRecords = response.totalCount;
          this.calculateTotalPages(response.totalCount, this.pageSize);
        },
        error: (error) => {
          console.error('Error fetching users:', error);
        }
      });
  }

  // Pagination methods
  public getMoreData(event: string): void {
    if (event == 'next') {
      this.currentPage++;
      this.pageIndex = this.currentPage - 1;
      this.limit += this.pageSize;
      this.skip = this.pageSize * this.pageIndex;
      this.getTableData({ skip: this.skip, limit: this.limit });
    } else if (event == 'previous') {
      this.currentPage--;
      this.pageIndex = this.currentPage - 1;
      this.limit -= this.pageSize;
      this.skip = this.pageSize * this.pageIndex;
      this.getTableData({ skip: this.skip, limit: this.limit });
    }
  }

  public moveToPage(pageNumber: number): void {
    this.currentPage = pageNumber;
    this.skip = this.pageSelection[pageNumber - 1].skip;
    this.limit = this.pageSelection[pageNumber - 1].limit;
    if (pageNumber > this.currentPage) {
      this.pageIndex = pageNumber - 1;
    } else if (pageNumber < this.currentPage) {
      this.pageIndex = pageNumber + 1;
    }
    this.getTableData({ skip: this.skip, limit: this.limit });
  }

  public changePageSize(pageSize: number): void {
    this.pageSelection = [];
    this.pageSize = pageSize;
    this.limit = pageSize;
    this.skip = 0;
    this.currentPage = 1;
    this.getTableData({ skip: this.skip, limit: this.limit });
  }

  public calculateTotalPages(totalData: number, pageSize: number): void {
    this.pageNumberArray = [];
    this.totalData = totalData;
    this.totalPages = totalData / pageSize;
    if (this.totalPages % 1 != 0) {
      this.totalPages = Math.trunc(this.totalPages + 1);
    }
    for (let i = 1; i <= this.totalPages; i++) {
      const limit = pageSize * i;
      const skip = limit - pageSize;
      this.pageNumberArray.push(i);
      this.pageSelection.push({ skip: skip, limit: limit });
    }
  }

  public blocked(): void {
    this.selectedTab = 'blocked';
    this.getTableData({ skip: 0, limit: this.pageSize });
  }

  public inblocked(): void {
    this.selectedTab = 'notBlocked';
    this.getTableData({ skip: 0, limit: this.pageSize });
  }

  public allUsers(): void {
    this.selectedTab = 'all';
    this.getTableData({ skip: 0, limit: this.pageSize });
  }

  // Add this method to open the modal
  public openStatusModal(user: userBlock): void {
    this.selectedUser = user;
    this.originalStatus = user.isBlocked; // Store original status
    this.statusChangeUserId = user.id; // Store user ID
    
    if (!this.modal) {
      this.modal = new (window as any).bootstrap.Modal(
        document.getElementById('userStatusModal')
      );
    }
    this.modal.show();
  }
private originalStatus: boolean | null = null;
  private statusChangeUserId: number | null = null;
// Add this method to handle confirmation
public confirmStatusChange(): void {
  if (!this.selectedUser) return;

  this.isLoading = true;
  this.userService.updateUserStatus(this.selectedUser.id, this.selectedUser.isBlocked)
    .pipe(finalize(() => this.isLoading = false))
    .subscribe({
      next: () => {
        this.toastr.success('The user has been updates', 'Success');
        this.selectedUser!.isBlocked = !this.selectedUser!.isBlocked;
        this.getTableData({ skip: this.skip, limit: this.limit });
      },
      error: (error) => {
        this.toastr.error('Failed to update user status', 'Error');
        this.revertStatus();
      }
    });
  }

  // Add this new method to handle "No" click
  public cancelStatusChange(): void {
    this.revertStatus();
    if (this.modal) {
      this.modal.hide();
    }
  }

  // Helper method to revert status
  private revertStatus(): void {
    if (this.statusChangeUserId && this.originalStatus !== null) {
      const user = this.tableData.find(u => u.id === this.statusChangeUserId);
      if (user) {
        user.isBlocked = this.originalStatus;
      }
      // Also update the showed table if filtered
      const showedUser = this.tableShowed.find(u => u.id === this.statusChangeUserId);
      if (showedUser) {
        showedUser.isBlocked = this.originalStatus;
      }
    }
    this.originalStatus = null;
    this.statusChangeUserId = null;
  }
}