import { Component, ElementRef, HostListener, ViewChild, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { CommonService, DataService, SidebarService } from 'src/app/core/core.index';
import { routes } from 'src/app/core/helpers/routes';
import { sideBar } from 'src/app/shared/model/header.model';
import { selectTenantData } from 'src/app/core/store/tenant/tenant.selectors';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  public routes = routes;
  @ViewChild('stickyMenu')
  menuElement!: ElementRef;
  headerpage = false;
  sticky = false;
  elementPosition!: number;
  public headerClass = true;
  base = '';
  page = '';
  last = '';
  headerMenuactive = '';
  imageUser = 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg';
  Username = '';
  tenantData$: Observable<any>;
  imageUrl: string | null = null;
  public white_bg = false;
  sidebar: sideBar[];
  
  isLoggedIn = false;
  userRole: string | null = null;

  constructor(
    private common: CommonService,
    private data: DataService,
    private sidebarService: SidebarService,
    private store: Store,
    private auth: AuthService,
    private router: Router
  ) {
    this.sidebar = this.data.sideBar;
    this.tenantData$ = this.store.select(selectTenantData);
  }

  ngOnInit(): void {
    this.checkAuthStatus();
    
    this.tenantData$.subscribe(data => {
      if (data && data.siteInfo && data.siteInfo.length > 0) {
        this.imageUrl = data.siteInfo[0].logo;
      }
    });

    this.common.base.subscribe((res: string) => {
      this.base = res;
    });
    this.common.page.subscribe((res: string) => {
      this.page = res;
    });
    this.common.last.subscribe((res: string) => {
      this.last = res;
    });
  }

  private checkAuthStatus(): void {
    const userString = localStorage.getItem('user_data');
    if (userString) {
      const user = JSON.parse(userString);
      this.isLoggedIn = true;
      this.userRole = user.Role;
      this.imageUser = user.avatar || user.ImageUrl || '';
      this.Username = user.Nom + ' ' + user.Prenom;
    } else {
      this.isLoggedIn = false;
      this.userRole = null;
    }
  }

  @HostListener('window:scroll', ['$event'])
  handleScroll() {
    const windowScroll = window.pageYOffset;
    if (windowScroll >= this.elementPosition) {
      this.sticky = true;
    } else {
      this.sticky = false;
    }
    if (windowScroll == 0) {
      this.white_bg = false;
    } else {
      this.white_bg = true;
    }
  }

  public toggleSidebar(): void {
    this.sidebarService.openSidebar();
  }

  public hideSidebar(): void {
    this.sidebarService.closeSidebar();
  }

  logout(): void {
    this.auth.logout();
    this.isLoggedIn = false;
    this.userRole = null;
    this.router.navigate([routes.login]);

  }
}