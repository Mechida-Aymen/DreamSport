import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { DataService, pageSelection, routes } from 'src/app/core/core.index';
import { Router } from '@angular/router';
import { PaginationService, tablePageSize } from 'src/app/shared/custom-pagination/pagination.service';
import { InvitationService } from 'src/app/core/service/invitation/invitation-service.service';
import { MemberInvitationDTOO } from 'src/app/core/models/member-invitation-dto';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { SignalRService } from 'src/app/core/service/signalR/signal-rservice.service';
import { ToastrService } from 'ngx-toastr';
import { filter, interval, Subject, takeUntil, tap } from 'rxjs';

@Component({
  selector: 'app-Invitation',
  templateUrl: './Invitation.component.html',
  styleUrls: ['./Invitation.component.scss']
})
export class InvitationComponent implements OnInit, OnDestroy {
  public tableData: MemberInvitationDTOO[] = [];
  public displayedData: MemberInvitationDTOO[] = [];
  public routes = routes;
  public pageSize = 10;
  public currentPage = 1;
  public totalPages = 1;
  public serialNumberArray: number[] = [];
  public totalData = 0;
  public dataSource!: MatTableDataSource<MemberInvitationDTOO>;
  public searchDataValue = '';
  private destroy$ = new Subject<void>();
  public signalRConnected = false;
  public invitationReceived = new Subject<MemberInvitationDTOO>();

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
    
    interval(5000).pipe(
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.signalRConnected = this.signalRService.getConnectionStatus();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setupSignalR(): void {
    this.signalRService.invitationReceived
      .pipe(takeUntil(this.destroy$))
      .subscribe((invitation: MemberInvitationDTOO) => {
        const recepteurId = invitation.recepteur ?? invitation.Recerpteur;
        
        if (recepteurId === this.auth.getUserId()) {
          this.handleNewInvitation({
            ...invitation,
            recepteur: recepteurId
          });
        } else {
          console.warn('Invitation not for current user:', invitation);
        }
      });
  }

  private handleNewInvitation(invitation: MemberInvitationDTOO): void {
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
    const currentUserId = this.auth.getUserId();
    this.invitationService.getUserInvitations(currentUserId).subscribe({
      next: (response) => {
        this.tableData = response.invitations;
        this.totalData = response.totalData;
        this.updateDataSource();
      },
      error: (err) => {
        console.error('Error loading invitations', err);
        this.toastr.error('Failed to load invitations', 'Error');
      }
    });
  }

  private updateDataSource(): void {
    this.dataSource = new MatTableDataSource<MemberInvitationDTOO>(this.tableData);
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
    
    if (this.searchDataValue) {
      this.displayedData = this.tableData.filter(invitation => 
        invitation.emetteur.firstName.toLowerCase().includes(this.searchDataValue) ||
        invitation.emetteur.lastName.toLowerCase().includes(this.searchDataValue) ||
        (invitation.emetteur.username && invitation.emetteur.username.toLowerCase().includes(this.searchDataValue)) ||
        invitation.id.toString().includes(this.searchDataValue)
      );
    } else {
      this.displayedData = [...this.tableData];
    }
    
    this.totalData = this.displayedData.length;
    this.currentPage = 1;
    this.updateDisplayedData();
  }

  public acceptInvitation(invitationId: number): void {
    this.invitationService.acceptInvitation(invitationId).subscribe({
      next: () => {
        this.toastr.success('Invitation accepted successfully', 'Success');
        this.tableData = this.tableData.filter(inv => inv.id !== invitationId);
        this.totalData--;
        this.updateDataSource();
      },
      error: (err) => {
        console.error('Error accepting invitation', err);
        this.toastr.error(err.error?.message || 'Failed to accept invitation', 'Error');
      }
    });
  }

  public refuseInvitation(invitationId: number): void {
    this.invitationService.refuseInvitation(invitationId).subscribe({
      next: () => {
        this.toastr.success('Invitation declined successfully', 'Success');
        this.tableData = this.tableData.filter(inv => inv.id !== invitationId);
        this.totalData--;
        this.updateDataSource();
      },
      error: (err) => {
        console.error('Error refusing invitation', err);
        this.toastr.error(err.error?.message || 'Failed to decline invitation', 'Error');
      }
    });
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
}