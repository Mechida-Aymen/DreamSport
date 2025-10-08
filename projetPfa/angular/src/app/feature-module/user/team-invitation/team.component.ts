import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { pageSelection, PaginationService, tablePageSize } from 'src/app/shared/custom-pagination/pagination.service';
import { InvitationService } from 'src/app/core/service/invitation/invitation-service.service';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { SignalRService } from 'src/app/core/service/signalR/signal-rservice.service';
import { ToastrService } from 'ngx-toastr';
import { finalize, interval, Subject, takeUntil } from 'rxjs';
import { TeamInvitationDTO } from 'src/app/core/models/TeamInvitationDTO.model';
import { routes } from 'src/app/core/core.index';

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamComponent implements OnInit, OnDestroy {
  public tableData: TeamInvitationDTO[] = [];
  public displayedData: TeamInvitationDTO[] = [];
  public pageSize = 10;
  public currentPage = 1;
  public totalPages = 1;
  public totalData = 0;
  public dataSource!: MatTableDataSource<TeamInvitationDTO>;
  public searchDataValue = '';
  private destroy$ = new Subject<void>();
  public signalRConnected = false;
  public routes = routes;
  public processingInvitation: number | null = null;
  public isLoading = false;

  constructor(
    private pagination: PaginationService,
    private router: Router,
    private invitationService: InvitationService,
    private auth: AuthService,
    private signalRService: SignalRService,
    private toastr: ToastrService
  ) {
    this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
      this.getTableData({ skip: res.skip, limit: res.limit });
      this.pageSize = res.pageSize;
    });
  }

  ngOnInit(): void {
    this.loadInvitations();
    this.setupSignalR();
    this.monitorConnectionStatus();
  }

  private monitorConnectionStatus(): void {
    this.signalRConnected = this.signalRService.getConnectionStatus();
    interval(5000).pipe(takeUntil(this.destroy$)).subscribe(() => {
      this.signalRConnected = this.signalRService.getConnectionStatus();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setupSignalR(): void {
    this.signalRService.teamInvitationReceived
      .pipe(takeUntil(this.destroy$))
      .subscribe((invitation: TeamInvitationDTO) => {
        if (invitation.recepteur === this.auth.getUserId()) {
          this.handleNewInvitation(invitation);
        }
      });
  }

  private handleNewInvitation(invitation: TeamInvitationDTO): void {
    const existingIndex = this.tableData.findIndex(i => i.id === invitation.id);
    
    if (existingIndex >= 0) {
      this.tableData[existingIndex] = invitation;
    } else {
      this.tableData.unshift(invitation);
      this.totalData++;
    }

    this.updateDataSource();
  
  }

  private loadInvitations(): void {
    this.isLoading = true;
    const currentUserId = this.auth.getUserId();
    
    this.invitationService.getTeamInvitations(currentUserId)
      .pipe(
        finalize(() => this.isLoading = false)
      )
      .subscribe({
        next: (response) => {
          this.tableData = response.invitations;
          this.totalData = response.totalData;
          this.updateDataSource();
        },
        error: (err) => {
          console.error('Error loading team invitations', err);
          this.toastr.error('Failed to load team invitations', 'Error');
        }
      });
  }

  private updateDataSource(): void {
    this.dataSource = new MatTableDataSource<TeamInvitationDTO>(this.tableData);
    this.totalPages = Math.ceil(this.totalData / this.pageSize);
    this.updateDisplayedData();
    
    this.pagination.calculatePageSize.next({
      totalData: this.totalData,
      pageSize: this.pageSize,
      tableData: this.tableData,
      serialNumberArray: Array.from({length: this.totalData}, (_, i) => i + 1),
    });
  }

  private updateDisplayedData(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.displayedData = this.tableData.slice(startIndex, endIndex);
  }

  private getTableData(pageOption: pageSelection): void {
    const startIndex = pageOption.skip;
    const endIndex = startIndex + pageOption.limit;
    this.displayedData = this.tableData.slice(startIndex, endIndex);
  }

  public searchData(value: string): void {
    this.searchDataValue = value.trim().toLowerCase();
    
    this.displayedData = this.searchDataValue 
      ? this.tableData.filter(invitation => 
          invitation.invitation.name.toLowerCase().includes(this.searchDataValue) ||
          (invitation.invitation.description && 
           invitation.invitation.description.toLowerCase().includes(this.searchDataValue))
        )
      : [...this.tableData];
    
    this.totalData = this.displayedData.length;
    this.currentPage = 1;
    this.updateDisplayedData();
  }

  public acceptInvitation(invitationId: number): void {
    if (!invitationId) {
      this.toastr.error('Invalid invitation ID', 'Error');
      return;
    }

    this.processingInvitation = invitationId;

    this.invitationService.acceptTeamInvitation(invitationId)
      .pipe(
        finalize(() => this.processingInvitation = null)
      )
      .subscribe({
        next: () => {
          this.toastr.success('Team invitation accepted successfully', 'Success');
          this.removeInvitationFromList(invitationId);
        },  
        error: (err) => {
          console.error('Error accepting team invitation:', err);
          this.toastr.error(
            err.error?.message || 'Failed to accept team invitation', 
            'Error',
            { timeOut: 5000 }
          );
        }
      });
  }
  
  public refuseInvitation(invitationId: number): void {
    if (!invitationId) {
      this.toastr.error('Invalid invitation ID', 'Error');
      return;
    }

    this.processingInvitation = invitationId;

    this.invitationService.refuseTeamInvitation(invitationId)
      .pipe(
        finalize(() => this.processingInvitation = null)
      )
      .subscribe({
        next: () => {
          this.toastr.success('Team invitation declined successfully', 'Success');
          this.removeInvitationFromList(invitationId);
        },
        error: (err) => {
          console.error('Error declining team invitation:', err);
          this.toastr.error(
            err.error?.message || 'Failed to decline team invitation', 
            'Error',
            { timeOut: 5000 }
          );
        }
      });
  }
  
  private removeInvitationFromList(invitationId: number): void {
    this.tableData = this.tableData.filter(inv => inv.id !== invitationId);
    this.totalData = this.tableData.length;
    this.updateDataSource();
    
    if (this.tableData.length === 0) {
      this.toastr.info('No more team invitations', 'Info');
    }
  }

  public goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updateDisplayedData();
    }
  }

  public nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updateDisplayedData();
    }
  }

  public prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updateDisplayedData();
    }
  }

  public getPages(): number[] {
    return Array.from({length: this.totalPages}, (_, i) => i + 1);
  }
}