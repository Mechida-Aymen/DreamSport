import { Component } from '@angular/core';
import { CommonService, DataService, adminPages, routes, url } from 'src/app/core/core.index';
import { NavigationStart, Router, Event as RouterEvent } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent {
  public routes = routes;
  base = "";
  page = "";
  last = "";
  end = "";
  currentUrl = '';
  
  adminPages: Array<adminPages> = [];
  
  constructor(private Router: Router, private dataservice: DataService,private common: CommonService,) {
    this.common.base.subscribe((res: string) => {
      this.base = res?.replace('-', ' ');
    });
    this.common.page.subscribe((res: string) => {
      this.page = res?.replace('-', ' ');
    });
    this.common.last.subscribe((res: string) => {
      this.last = res?.replace('-', ' ');
    });
    this.common.end.subscribe((res: string) => {
      this.end = res?.replaceAll('-', ' ');
    });
    this.dataservice.getAdminPages.subscribe((res: Array<adminPages>) => {
      this.adminPages = res;
    });
    this.Router.events.subscribe((data: RouterEvent) => {
      if (data instanceof NavigationStart) {
        this.setRouting(data);
        console.log("currentUrl",this.currentUrl);
        console.log("page",this.page);
      }
      
    });
  
  }
  private setRouting(data: url): void {
    this.currentUrl = data.url;
    this.common.base.next(data.url.split('/')[2]);
    this.common.page.next(data.url.split('/')[3]);
    this.common.last.next(data.url.split('/')[4]);
    this.common.end.next(data.url.split('/')[5]);
  
  }
}
