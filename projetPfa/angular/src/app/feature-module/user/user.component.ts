import { Component, OnDestroy, OnInit } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { Location } from '@angular/common';
import { OwlOptions } from 'ngx-owl-carousel-o';
import {
  CommonService,
  DataService,
  routes,
  url,
  usermenu,
} from 'src/app/core/core.index';
import { MemberInvitationDTOO } from 'src/app/core/models/member-invitation-dto';
import { ToastrService } from 'ngx-toastr';
import { SignalRService } from 'src/app/core/service/signalR/signal-rservice.service';
import { interval, takeUntil ,Subject } from 'rxjs';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { TeamInvitationDTO } from 'src/app/core/models/TeamInvitationDTO.model';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
})
export class UserComponent implements OnInit, OnDestroy {
  public routes = routes;
  breadcrum = true;
  base = '';
  page = '';
  last = '';
  currentUrl = '';

  courtBg = true;
  userMenus: Array<usermenu> = [];
  url: string;
  private destroy$ = new Subject<void>();
  public signalRConnected = false;

  setactive = false;
  constructor(
    private Router: Router,
    location: Location,
    private dataservice: DataService,
    private common: CommonService,
    private toastr: ToastrService,
    private signalRService: SignalRService,
    private auth: AuthService,
    
  ) {
    this.common.base.subscribe((res: string) => {
      this.base = res?.replaceAll('-', ' ');
    });
    this.common.page.subscribe((res: string) => {
      this.page = res?.replaceAll('-', ' ');
    });
    this.common.last.subscribe((res: string) => {
      this.last = res?.replaceAll('-', ' ');
    });

    this.dataservice.getuserMenus.subscribe((res: Array<usermenu>) => {
      this.userMenus = res;
    });
    Router.events.subscribe((event: object) => {
      if (event instanceof NavigationStart) {
        this.getRoutes(event);
      }
    });
    this.getRoutes(this.Router);
    this.url = location.path();
  }
  getRoutes(event: url) {
    const splitVal = event.url.split('/');
    this.currentUrl = event.url;
    this.common.base.next(splitVal[2]);
    this.common.page.next(splitVal[3]);
    this.common.last.next(splitVal[4]);

    if (this.page === 'dashboard') {
      this.courtBg = false;
    }
    if (this.currentUrl == routes.teamInvitation) {
      this.page = 'Invitation';
    }
    if (this.currentUrl == routes.userOthersettings) {
      this.page = 'user profile';
    }
    if (this.currentUrl == routes.sendInvitation) {
      this.page = 'Invitation';
    }
    if (this.currentUrl == routes.userTeam) {
      this.page = 'Team';
    }
  }
  setActive(menu: string) {
    sessionStorage.setItem('title', menu);
    this.userMenus.map((title) => {
      const active = sessionStorage.getItem('title');
      if (active == title.page) {
        this.setactive = true;
      } else {
        this.setactive = false;
      }
    });
  }
  public venuedetailstwoOwlOptions: OwlOptions = {
    margin: 10,
    nav: true,
    loop: true,
    dots: false,
    smartSpeed: 2000,
    navText: [
      "<i class='feather icon-chevron-left'></i>",
      "<i class='feather icon-chevron-right'></i>",
    ],
    responsive: {
      0: {
        items: 1,
      },
      500: {
        items: 2,
      },
      768: {
        items: 3,
      },
      1000: {
        items: 3,
      },
    },
  };
  gallery = [
    {
      id: 1,
      image: 'assets/img/gallery/gallery2/gallery-01.jpg',
    },
    {
      id: 2,
      image: 'assets/img/gallery/gallery2/gallery-02.jpg',
    },
    {
      id: 3,
      image: 'assets/img/gallery/gallery2/gallery-03.jpg',
    },
    {
      id: 4,
      image: 'assets/img/gallery/gallery2/gallery-01.jpg',
    },
    {
      id: 5,
      image: 'assets/img/gallery/gallery2/gallery-02.jpg',
    },
    {
      id: 6,
      image: 'assets/img/gallery/gallery2/gallery-01.jpg',
    },
  ];

 private setupSignalRInvUser(): void {
    this.signalRService.invitationReceived
      .pipe(takeUntil(this.destroy$))
      .subscribe((invitation: MemberInvitationDTOO) => {
        const recepteurId = invitation.recepteur ?? invitation.Recerpteur;
        
        if (recepteurId === this.auth.getUserId()) {
          this.handleNewInvitationUser({
            ...invitation,
            recepteur: recepteurId
          });
        } else {
          console.warn('Invitation not for current user:', invitation);
        }
      });
  }
  ngOnInit(): void {
    this.setupSignalRInvUser();
    this.setupSignalRTeam();
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

  private handleNewInvitationUser(invitation: MemberInvitationDTOO): void {
      const senderName = `${invitation.emetteur.firstName} ${invitation.emetteur.lastName}`;
      this.toastr.info(`New invitation from ${senderName}`, 'New Invitation', {
        timeOut: 5000,
        tapToDismiss: true,
        extendedTimeOut: 0, 
        disableTimeOut: false, 
      }).onTap.subscribe(() => { 
        this.Router.navigateByUrl(this.routes.userInvitation); 
      });
    }


    private setupSignalRTeam(): void {
      this.signalRService.teamInvitationReceived
        .pipe(takeUntil(this.destroy$))
        .subscribe((invitation: TeamInvitationDTO) => {
          if (invitation.recepteur === this.auth.getUserId()) {
            this.handleNewInvitationTeam(invitation);
          }
        });
    }
    private handleNewInvitationTeam(invitation: TeamInvitationDTO): void {
        this.toastr.info(
        `New team invitation from ${invitation.invitation.name}`,
        'New Team Invitation',
        {    timeOut: 5000,
          tapToDismiss: true,
          extendedTimeOut: 0, 
          disableTimeOut: false, 
        }).onTap.subscribe(() => { 
          this.Router.navigateByUrl(this.routes.teamInvitation); 
        });
    }


}
