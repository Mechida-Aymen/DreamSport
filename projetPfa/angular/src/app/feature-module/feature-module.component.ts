import { Component, OnInit } from '@angular/core';
import {
  NavigationEnd,
  NavigationStart,
  Router,
  Event as RouterEvent,
} from '@angular/router';
import { CommonService, SidebarService, url } from '../core/core.index';
import * as AOS from 'aos';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

export interface routerlink {
  id?: number;
  type?: number;
  url: string;
  urlAfterRedirects?: string;
}
@Component({
  selector: 'app-feature-module',
  templateUrl: './feature-module.component.html',
  styleUrls: ['./feature-module.component.scss'],
})
export class FeatureModuleComponent implements OnInit {
  showMiniSidebar = false;
  base = '';
  page = '';
  last = '';
  end = '';
  url?: Array<string>;
  index!: number;
  public header!: boolean;
  public mainFooter!: boolean;
  public routeStatus!: string;
  public routeStatusSub!: string;
  public strokeValue = 0;
  public progress = 0;
  private _loading = new BehaviorSubject<boolean>(false);
  public readonly loading$ = this._loading.asObservable();

  constructor(
    private Router: Router,
    private sidebar: SidebarService,
    private common: CommonService
  ) {
    this.common.base.subscribe((res: string) => {
      this.base = res;
    });
    this.common.page.subscribe((res: string) => {
      this.page = res;
    });
    this.common.last.subscribe((res: string) => {
      this.last = res;
    });
    this.Router.events.subscribe((data: RouterEvent) => {
      if (data instanceof NavigationStart) {
        this.setRouting(data);
        this.showLoader();
        this.showMiniSidebar = false;
      }
      if (data instanceof NavigationEnd) {
        this.hideLoader();
      }
    });

    this.setRouting(this.Router);

    this.common.header.subscribe((res: boolean) => {
      this.header = res;
    });
    this.common.mainFooter.subscribe((res: boolean) => {
      this.mainFooter = res;
    });
    this.sidebar.toogleSidebar.subscribe((res: string) => {
      if (res == 'true') {
        this.showMiniSidebar = true;
      } else {
        this.showMiniSidebar = false;
      }
    });
  }
  ngOnInit() {
    this.calculateScrollPercentage();
    AOS.init({ duration: 1200, once: true });
  }
  private setRouting(data: url): void {
    this.url = data.url.split('/');
    this.index = this.url.length - 1;
    this.routeStatus = data.url.split('/')[2];
    this.routeStatusSub = data.url.split('/')[3];
    this.common.end.next(this.url[this.index]);
    this.common.base.next(data.url.split('/')[2]);
    this.common.page.next(data.url.split('/')[3]);
    this.common.last.next(data.url.split('/')[4]);
    if (
      this.routeStatus === 'auth' ||
      this.routeStatusSub === 'maintenance' ||
      this.routeStatusSub === 'error404' ||
      this.routeStatusSub === 'coming-soon'
    ) {
      this.common.header.next(false);
      this.common.mainFooter.next(false);
    } else {
      this.common.header.next(true);
      this.common.mainFooter.next(true);
    }
  }

  showLoader() {
    this._loading.next(true);
  }

  hideLoader() {
    setTimeout(() => {
      this._loading.next(false);
    }, 1200);
  }

  // scroll the page to top position
  public scrollToTop(): void {
    window.scrollTo(0, 0);
  }

  // function to calculate the scroll progress
  private calculateScrollPercentage(): void {
    window.addEventListener('scroll', () => {
      const body = document.body,
        html = document.documentElement;
      //gets the total height of page till scroll
      const totalheight = Math.max(
        body.scrollHeight,
        body.offsetHeight,
        html.clientHeight,
        html.scrollHeight,
        html.offsetHeight
      );
      // calculates the total stroke value
      this.progress = totalheight;
      const currentDistance = window.scrollY / (totalheight / 3000);
      // calculates the current stroke value
      this.strokeValue = totalheight - currentDistance / 8;
      // condition to hide scroll progress if page in top
      if (window.scrollY == 0) {
        this.strokeValue = totalheight;
      }
      // condition to make scroll progress to 100 if page in bottom
      if (window.innerHeight + window.scrollY >= totalheight) {
        this.strokeValue = 0;
      }
    });
  }
}
